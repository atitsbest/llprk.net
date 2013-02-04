﻿using Llprk.Web.UI.Application.Exceptions;
using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;

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
            order.CreatedAt = DateTime.Now;
            db.Orders.Add(order);
            db.SaveChanges();

            foreach (var id in productIdsAndQtys.Keys) {
                var product = db.Products.FirstOrDefault(x => x.Id == id);
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

                order.OrderLines.Add(new OrderLine() {
                    OrderId = order.Id,
                    ProductId = id,
                    Qty = qty
                });
            }
            db.SaveChanges();
        }

        /// <summary>
        /// Markiert einen Auftrag als bezahlt.
        /// </summary>
        /// <param name="order"></param>
        public void PayOrder(Entities db, Order order, string mailBody)
        {
            if (order == null) { throw new ArgumentNullException("order"); }

            order.PaidAt = DateTime.Now; // TODO: Vielleicht hat der Kunde schon vorher gezahlt?

            // TODO: Email an den Kunden schicken.
            _SendMailToCustomer(order, "Wir haben Deine Bezahlung erhalten", mailBody);

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

            // TODO: Email an den Kunden schicken.
            _SendMailToCustomer(order, "Deine Bestellung wurde verschickt", mailBody);
            db.SaveChanges(); 
        }

        /// <summary>
        /// Schickt eine HTML-Mail an die Email-Adresse aus dem Auftrag.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        private static void _SendMailToCustomer(Order order, string subject, string mailBody)
        {
            var message = new MailMessage("shop@lillypark.com", order.Email, subject, mailBody);
            message.IsBodyHtml = true;
            message.BodyEncoding = Encoding.UTF8;

            var mp = new SmtpMailProvider();
            mp.SendMail(message);

        }
    }
}