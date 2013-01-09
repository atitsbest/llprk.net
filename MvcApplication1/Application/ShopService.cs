﻿using MvcApplication1.Application.Exceptions;
using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MvcApplication1.Application
{
    public class ShopService
    {
        /// <summary>
        /// Bestellung abgeben.
        /// </summary>
        /// <param name="order"></param>
        /// <param name="productIdsAndQtys"></param>
        public void PlaceOrder(ShopDb db, Order order, IDictionary<int, int> productIdsAndQtys)
        {
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
    }
}