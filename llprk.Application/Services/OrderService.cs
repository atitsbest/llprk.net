using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Transactions;
using System.Data.Entity;

namespace Llprk.Application.Services
{
    public interface IOrderService
    {
        Order CreateOrder(int cartId, CreateOrderRequest request);
    }

    public class OrderService : IOrderService
    {
        private IShippingService _ShippingService;
        private ITaxService _TaxService;

        private static string _orderLock = "order";

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="shipping"></param>
        public OrderService(IShippingService shipping, ITaxService taxes)
        {
            _ShippingService = shipping;
            _TaxService = taxes;
        }

        /// <summary>
        /// Place a new order from an existing cart.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public Order CreateOrder(int cartId, CreateOrderRequest request)
        {
            // Validate request.
            var vc = new ValidationContext(request);
            Validator.ValidateObject(request, vc, true);

            lock (_orderLock) // Only one order at a time.
            {
                using (var tx = new TransactionScope())
                {
                    using (var db = new Entities())
                    {
                        // Get cart.
                        var cart = db.Carts
                            .Include(i => i.LineItems.Select(l => l.Product))
                            .Single(c => c.Id == cartId);

                        if (cart.OrderId.HasValue) {
                                throw new ApplicationException(string.Format(
                                    "Dieser Warenkorb ({0} wurde bereits bestellt!",
                                    cartId));
                        }

                        // Calcualte costs and taxes.
                        var shippingCosts = _ShippingService.CalculateShippingCosts(cartId, request.DeliveryAddress.CountryId);
                        var tax = _TaxService.TaxForCountry(cartId, request.DeliveryAddress.CountryId);

                        var da = request.DeliveryAddress;

                        // Create new Order.
                        var order = new Order
                        {
                            // Adressen
                            Address1 = da.Address1,
                            Address2 = da.Address2,
                            City = da.City,
                            CountryId = da.CountryId,
                            Email = da.Email,
                            Firstname = da.Firstname,
                            Name = da.Lastname,
                            Salutation = da.Salutation,
                            Zip = da.Zip,

                            ShippingCosts = shippingCosts,
                            Tax = tax,
                            SubTotalPrice = cart.Subtotal,
                            Total = cart.Subtotal + shippingCosts + tax,

                            CreatedAt = DateTime.UtcNow
                        };

                        // Order speicher.
                        db.Orders.Add(order);
                        db.SaveChanges();

                        // Alle Lineitems der Order zuweisen.
                        foreach (var lineItem in cart.LineItems)
                        {
                            if (lineItem.Product == null)
                            {
                                throw new ApplicationException(string.Format(
                                    "Das Produkt mit der Id {0} ist nicht verfügbar!",
                                    lineItem.ProductId));
                            }
                            if (lineItem.Product.Available < lineItem.Qty)
                            {
                                throw new ApplicationException(string.Format(
                                    "Von {0} ist/sind nur noch {1} Stück auf Lager. Bitte Menge anpassen.",
                                    lineItem.Product.Name,
                                    lineItem.Product.Available));
                            }

                            lineItem.Product.Available -= lineItem.Qty;
                            lineItem.OrderId = order.Id;
                        }

                        // Mark cart as ordered.
                        cart.OrderId = order.Id;

                        // Bestätigungsmail verschicken.
                        //var mailBody = Nustache.Core.Render.StringToString(db.Parameters.First().MailMessageOrdered, order);
                        //MailService.SendMailToCustomer(order.Email, "Deine Bestellung bei lillypark.com", mailBody);

                        // Commit!
                        tx.Complete();

                        return order;
                    }
                }
            }
        }
    }
}
