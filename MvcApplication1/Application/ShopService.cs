using Llprk.Web.UI.Application.Exceptions;
using Llprk.Web.UI.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;

namespace Llprk.Web.UI.Application
{
    public class ShopService
    {
        /// <summary>
        /// Bestellung abgeben.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="productIdsAndQtys"></param>
        public void PlaceOrder(Entities db, Order order, IDictionary<int, int> productIdsAndQtys)
        {
            using (var transaction = new TransactionScope()) {
                // Bestellung mit Datum versehen und in DB speichern.
                order.CreatedAt = DateTime.Now;
				// Die Bestellung muss erstmal ohne Bestellzeilen gespeichert werden,
				// damit wir eine BestellID aus der DB haben.
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var id in productIdsAndQtys.Keys) {
                    var product = db.Products
                        .Include(p => p.ShippingCategory) // Brauchen wird für die Versandkostenberechnung.
                        .FirstOrDefault(x => x.Id == id);
                    var qty = productIdsAndQtys[id];
                    if (product == null) {
                        throw new AppException(string.Format(
                            "Das Produkt mit der Id {0} ist nicht verfügbar!",
                            id));
                    }
                    // Nicht mehr genug vom Produkt auf Lager?
                    if (product.Available < qty) {
                        throw new AppException(string.Format(
                            "Von {0} ist/sind nur noch {1} Stück auf Lager. Bitte Menge anpassen.",
                            product.Name,
                            product.Available));
                    }

                    // Menge abziehen.
                    product.Available -= qty;

                    // Produkte zur Bestellung hinzufügen.
                    order.OrderLines.Add(new OrderLine() {
                        OrderId = order.Id,
                        ProductId = id,
						Product = product,
                        Qty = qty
                    });
                }
				// ...speichern.
                db.SaveChanges();

				// Preis berechnen
                order.SubTotalPrice = CalculateSubTotalPrice(order);
				// Versandkosten berechnen
                order.ShippingCosts = CalculateShippingCosts(order);

                // Validierung.
                var vc = new ValidationContext(order, null, null);
                Validator.ValidateObject(order, vc, true);

				// ...speichern.
                db.SaveChanges();

                // Bestätigungsmail verschicken.
                var mailBody = Nustache.Core.Render.StringToString(db.Parameters.First().MailMessageOrdered, order);
                MailService.SendMailToCustomer(order.Email, "Deine Bestellung bei lillypark.com", mailBody);

                transaction.Complete();
            }

			// Benachrichtgung an den Shopinhaber, dass eine neue
			// Bestellung eingegangen ist.
			// INFO: Wenn das Verschicken fehlschlägt, soll die Bestellung dennoch angenommen werden.
            var ownerMailBody = string.Format(@"
Hi Lilly/Hoonie,

guckst Du <a href=""http://lillypark.com/orders/details/{0}"">hier</a>.

Liebe Grüße,

Dein WebShop", order.Id);
            MailService.SendMailToOwner("Eine neue Bestellung ist eingegangen.", ownerMailBody);
        }

        /// <summary>
        /// Versandkosten berechnen.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public decimal CalculateShippingCosts(Order order)
        {
            if (order.Country == null) { throw new AppException("Für die Versandkosten muss ein Land angegeben sein!"); }
            return order.OrderLines.Sum(ol => order.Country.ShippingCost(ol.Product.ShippingCategory));
        }

		/// <summary>
		/// Berechnet den Preis aller Bestellzeilen (ohne Versandkosten oder ähnlichem).
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        public decimal CalculateSubTotalPrice(Order order)
        {
            return order.OrderLines
                .Select(ol => ol.Product.Price * ol.Qty)
                .Sum();
        }

        /// <summary>
        /// Markiert einen Auftrag als bezahlt.
        /// </summary>
        /// <param name="order"></param>
        public void PayOrder(Entities db, Order order, string mailBody)
        {
            if (order == null) { throw new ArgumentNullException("order"); }

            order.PaidAt = DateTime.Now; // TODO: Vielleicht hat der Kunde schon vorher gezahlt?

            // Email an den Kunden schicken.
            MailService.SendMailToCustomer(order.Email, "Wir haben Deine Bezahlung erhalten", mailBody);

            db.SaveChanges();
        }

        /// <summary>
        /// Markiert einen Auftrag als verschickt.
        /// </summary>
        /// <param name="order"></param>
        public void ShipOrder(Entities db, Order order, string mailBody)
        {
            if (order == null) { throw new ArgumentNullException("order"); }

            order.ShippedAt = DateTime.Now;

            // Email an den Kunden schicken.
            MailService.SendMailToCustomer(order.Email, "Deine Bestellung wurde verschickt", mailBody);
            db.SaveChanges();
        }

    }
}