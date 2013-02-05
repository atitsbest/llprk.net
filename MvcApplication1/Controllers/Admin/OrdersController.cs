using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;
using Llprk.Web.UI.ViewModels;
using AutoMapper;
using Llprk.Web.UI.Application;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize]
    public class OrdersController : ApplicationController
    {
        public enum Filter
        {
            All,
            Paid,
            Shipped,
            Ready,
            Overdue
        }

        //
        // GET: /Orders/
        public ActionResult Index(Filter? filter)
        {
            IEnumerable<Order> result;

            if (filter == null) {
                result = db.Orders.Include(i => i.OrderLines).ToList();
            }
            else {
                switch (filter) {
                    case Filter.Overdue:
                        result = db.Orders
                                    .Include(i => i.OrderLines)
                                    .ToList()
                                    .Where(o => o.PaidAt == null && (DateTime.Now - o.CreatedAt).Days > 10);
                        break;
                    case Filter.Ready:
                        result = db.Orders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.PaidAt != null && o.ShippedAt == null);
                        break;
                    case Filter.Paid:
                        result = db.Orders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.PaidAt != null);
                        break;
                    case Filter.Shipped:
                        result = db.Orders
                                    .Include(i => i.OrderLines)
                                    .Where(o => o.ShippedAt != null);
                        break;
                    default:
                        result = db.Orders.Include(i => i.OrderLines);
                        break;
                }
            }
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
                Headline = string.Format("Invoice for Order {0} has been paid.", order.Id),
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
            var order = db.Orders.Find(id);
            var renderedMailBody = Nustache.Core.Render.StringToString(db.Parameters.First().MailMessageShipped, order);

            return View("StatusChange", new OrderStatusChange() { 
                Order = order,
                ActionButtonText = "Order has been SHIPPED",
                ChangeActionName = "shipped",
                Headline = string.Format("Order {0} has been shipped.", order.Id),
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