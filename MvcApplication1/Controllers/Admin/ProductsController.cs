using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;

namespace MvcApplication1.Controllers.Admin
{
    public class ProductsController : Controller
    {
        private ShopDb db = new ShopDb();

        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View(db.Products
                .Include(i => i.Category)
                .Include(i => i.Pictures));
        }

        //
        // GET: /Products/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products
                .Include(i => i.Pictures)
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            _PopulateCategoriesDropDownList();
            return View();
        }

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(Product product, string[] pictures)
        {
            if (ModelState.IsValid)
            {
                // Produkt mit den Bildern anreichern.
                var guids = Request.Form["pictures[]"]
                    .Replace("[", "")
                    .Replace("]", "")
                    .Replace("\"", "")
                    .Split(new char[] { ',' })
                    .Select(s => Guid.Parse(s));

                var ps = from p in db.Pictures
                         where guids.Contains(p.Id)
                         select p;

                product.Pictures = new HashSet<Picture>();
                foreach (var p in ps) { product.Pictures.Add(p); }
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Products/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            _PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            _PopulateCategoriesDropDownList(product.CategoryId);
            return View(product);
        }

        //
        // GET: /Products/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Products/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void _PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var departmentsQuery = from d in db.Categories
                                   orderby d.Name
                                   select d;
            ViewBag.CategoryId = new SelectList(departmentsQuery, "Id", "Name", selectedCategory);
        }
    }
}