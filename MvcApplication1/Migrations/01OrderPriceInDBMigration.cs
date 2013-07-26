using Llprk.Web.UI.Application;
using Llprk.Web.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace Llprk.Web.UI.Migrations
{
    public class _01OrderPriceInDBMigration
    {
        public void Execute() {
            using (var db = new Entities()) {
				// Welche Bestellungen müssen den geändert werden?
                var ordersToUpdate = db.Orders
                                        .Include(o => o.OrderLines)
                                        .Include("OrderLines.Product")
                                        .Include("OrderLines.Product.ShippingCategory")
                                        .Where(o => o.SubTotalPrice == 0 || o.ShippingCosts == 0)
                                        .ToList();

				var shopService = new ShopService();

				// Für die gewählten Bestellungen die Preise berechnen...
                foreach (var order in ordersToUpdate) {
                    order.SubTotalPrice = shopService.CalculateSubTotalPrice(order);
                    order.ShippingCosts = shopService.CalculateShippingCosts(order);
                }

				// ...und speichern.
                db.SaveChanges();
            }	
        }
    }
}