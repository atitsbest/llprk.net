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

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize]
    public class ProductsController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Products/

        public ActionResult Index()
        {
            return View(db.Products
                .Include(i => i.Category));
        }

        //
        // GET: /Products/Details/5

        public ActionResult Details(int id = 0)
        {
            Product product = db.Products
                .Where(p => p.Id == id)
                .FirstOrDefault();
            if (product == null) {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Products/Create

        public ActionResult Create()
        {
            var viewModel = new ProductCreate();
            viewModel.AllPictures = db.Pictures.ToArray();
            _PopulateCategoriesDropDownList();
            ViewBag.Title = "New Product";
            return View("edit", viewModel);
        }

        //
        // POST: /Products/Create

        [HttpPost]
        public ActionResult Create(Product product, string pictureIds)
        {
            if (ModelState.IsValid) {
                // Produkt mit den Bildern anreichern.
                var guids = _stringToGuids(pictureIds).ToArray();
                _SetProductPictures(product, guids);
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
            Product product = db.Products
                .Where(x => x.Id == id)
                .Single();
            if (product == null) {
                return HttpNotFound();
            }

            var viewModel = new ProductEdit();
            Mapper.Map(product, viewModel);
            viewModel.AllPictures = db.Pictures.ToArray();

            _PopulateCategoriesDropDownList(product.CategoryId);
            ViewBag.Title = "Edit";
            return View(viewModel);
        }

        //
        // POST: /Products/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product, string pictureIds)
        {
            if (ModelState.IsValid) {
                var p = db.Products
                    .Where(x => x.Id == product.Id)
                    .Single();

                p.CategoryId = product.CategoryId;
                p.Description = product.Description;
                p.IsPublished = product.IsPublished;
                p.Name = product.Name;
                p.Price = product.Price;
                p.Available = product.Available;

                var guids = _stringToGuids(pictureIds);
                _SetProductPictures(p, guids);

                db.Entry(p).State = EntityState.Modified;
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
            if (product == null) {
                return HttpNotFound();
            }
            return View(new ProductDelete() { 
                Product = product,
                Orders = from o in db.Orders
                         from ol in o.OrderLines
                         where ol.ProductId == id
                         select o
            });
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

        /// <summary>
        /// Kategorie-Auswahl befüllen.
        /// </summary>
        /// <param name="selectedCategory"></param>
        private void _PopulateCategoriesDropDownList(object selectedCategory = null)
        {
            var departmentsQuery = from d in db.Categories
                                   orderby d.Name
                                   select d;
            ViewBag.CategoryId = new SelectList(departmentsQuery, "Id", "Name", selectedCategory);
        }

        /// <summary>
        /// Bilder zum Produkt hinzufügen.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="pictureIds">Komma-getrennte Guids.</param>
        private void _SetProductPictures(Product product, IEnumerable<Guid> guids)
        {
            var gs = guids.ToArray();
            var c = guids.Count();

            if (c > 0) { product.Picture1 = _GetPictureById(gs[0]); product.Picture1Id = gs[0]; }
            else { product.Picture1 = null;  product.Picture1Id = null; }
            if (c > 1) { product.Picture2 = _GetPictureById(gs[1]); }
            else { product.Picture2 = null;  product.Picture2Id = null; }
            if (c > 2) { product.Picture3 = _GetPictureById(gs[2]); }
            else { product.Picture3 = null;  product.Picture3Id = null; }
            if (c > 3) { product.Picture4 = _GetPictureById(gs[3]); }
            else { product.Picture4 = null;  product.Picture4Id = null; }
            if (c > 4) { product.Picture5 = _GetPictureById(gs[4]); }
            else { product.Picture5 = null;  product.Picture5Id = null; }
        }

        /// <summary>
        /// Liefert das Bild per Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Picture _GetPictureById(Guid id) {
            return db.Pictures.FirstOrDefault(p => p.Id == id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pictureIds"></param>
        /// <returns></returns>
        private static IEnumerable<Guid> _stringToGuids(string pictureIds)
        {
            if (string.IsNullOrWhiteSpace(pictureIds)) { return new Guid[] { }; }
            var guids = pictureIds
                .Split(new char[] { ',' })
                .Select(s => Guid.Parse(s));
            return guids;
        }

    }
}