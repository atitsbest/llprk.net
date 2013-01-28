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
            return View(viewModel);
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

            product.Picture1 = c > 0 ? _GetPictureById(gs[0]) : null;
            product.Picture2 = c > 1 ? _GetPictureById(gs[1]) : null;
            product.Picture3 = c > 2 ? _GetPictureById(gs[2]) : null;
            product.Picture4 = c > 3 ? _GetPictureById(gs[3]) : null;
            product.Picture5 = c > 4 ? _GetPictureById(gs[4]) : null;
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
            var guids = pictureIds
                .Split(new char[] { ',' })
                .Select(s => Guid.Parse(s));
            return guids;
        }

    }
}