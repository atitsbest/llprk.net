using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;
using AutoMapper;
using Llprk.Web.UI.Application;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public class OrdersController : ApplicationController
    {
        public enum Filter
        {
            All, Paid, Shipped, Ready, Overdue
        }

        //
        // GET: /Orders/
        public ActionResult Index(Filter? filter, string q)
        {
            IEnumerable<Order> result;
            var sortedOrders = db.Orders.OrderByDescending(o => o.Id);

            if (filter == null) {
                result = sortedOrders.Include(i => i.OrderLines).ToList();
            }
            else {
                switch (filter) {
                    case Filter.Overdue:
                        result = sortedOrders
                                    .Include(i => i.OrderLines)
                                    .ToList()
                                    .Where(o => o.PaidAt == null && (DateTime.Now - o.CreatedAt).Days > 10);
                        break;
                    case Filter.Ready:
                        result = sortedOrders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.PaidAt != null && o.ShippedAt == null);
                        break;
                    case Filter.Paid:
                        result = sortedOrders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.PaidAt != null);
                        break;
                    case Filter.Shipped:
                        result = sortedOrders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.ShippedAt != null);
                        break;
                    default:
                        result = sortedOrders.Include(i => i.OrderLines);
                        break;
                }
            }
            if (!string.IsNullOrWhiteSpace(q)) { 
				// Suche...
                result = result
                    .Where(p => p.OrderNumber.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || (p.Address1 ?? "").IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || (p.Address2 ?? "").IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || p.Email.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || p.City.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || p.Zip.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
							// Suche nach dem Produkt das bestellt wurde.
                            || p.OrderLines.Any(ol => ol.Product.Name.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1)
                            || p.Firstname.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || p.Name.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1);
							
            }
            ViewBag.q = q;
            return View(result);
        }

        //
        // GET: /Orders/Details/5
        public ActionResult Details(int id = 0)
        {
            var order = db.Orders
                .Include(i => i.OrderLines)
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (order == null) {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // GET: /Orders/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        //
        // POST: /Orders/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        /// <summary>
        /// GET: orders/pay/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Pay(int id)
        {
            var order = db.Orders.Find(id);
            var renderedMailBody = Nustache.Core.Render.StringToString(db.Parameters.First().MailMessagePaid, order);

            return View("StatusChange", new OrderStatusChange() { 
                Order = order,
                ActionButtonText = "Invoice has been PAID",
                ChangeActionName = "paid",
                Headline = string.Format("Invoice for Order {0} has been paid.", order.OrderNumber),
                MailBody = renderedMailBody
            });
        }

        /// <summary>
        /// Setzt den Status der Rechnung auf bezahlt.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Paid(int id, string mailBody)
        {
            var order = db.Orders.Find(id);
            new ShopService().PayOrder(db, order, mailBody);
            return RedirectToAction("details", new { id });
        }


        /// <summary>
        /// GET: orders/ship/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Ship(int id)
        {
            var order = db.Orders
                .Include(o => o.OrderLines)
				.Include("OrderLines.Product")
				.Include("OrderLines.Product.ShippingCategory")
                .Single(o => o.Id == id);
            var renderedMailBody = Nustache.Core.Render.StringToString(db.Parameters.First().MailMessageShipped, order);

            return View("StatusChange", new OrderStatusChange() { 
                Order = order,
                ActionButtonText = "Order has been SHIPPED",
                ChangeActionName = "shipped",
                Headline = string.Format("Order {0} has been shipped.", order.OrderNumber),
                MailBody = renderedMailBody
            });
        }

        /// <summary>
        /// Setzt den Status der Rechnung auf bezahlt.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Shipped(int id, string mailBody)
        {
            var order = db.Orders.Find(id);
            new ShopService().ShipOrder(db, order, mailBody);
            return RedirectToAction("details", new { id });
        }

        /// <summary>
        /// Die Notizen zu einer Bestellung speichern.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateInput(false)]
        public ActionResult Comment(int id, string comment) 
        {
            var order = db.Orders.Find(id);
            order.Comment = comment;
            db.SaveChanges();
            return RedirectToAction("details", new { id });
        }

    }
}