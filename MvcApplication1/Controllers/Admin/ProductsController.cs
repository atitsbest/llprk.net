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
    [Authorize]
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
        public ActionResult Create(Product product, string pictureIds)
        {
            if (ModelState.IsValid)
            {
                // Produkt mit den Bildern anreichern.
                _AddPicturesToProduct(product, pictureIds);
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
                .Include(i => i.Pictures)
                .Where(x => x.Id == id)
                .Single();
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
        public ActionResult Edit(Product product, string pictureIds)
        {
            if (ModelState.IsValid)
            {
                var p = db.Products
                    .Include(i => i.Pictures)
                    .Where(x => x.Id == product.Id)
                    .Single();

                p.CategoryId = product.CategoryId;
                p.Description = product.Description;
                p.IsPublished = product.IsPublished;
                p.Name = product.Name;
                p.Pice = product.Pice;

                _UpdateProductPictures(_stringToGuids(pictureIds), p);

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
        private void _AddPicturesToProduct(Product product, string pictureIds)
        {
            var guids = _stringToGuids(pictureIds);

            var ps = from p in db.Pictures
                     where guids.Contains(p.Id)
                     select p;

            product.Pictures = new HashSet<Picture>();
            foreach (var p in ps) { product.Pictures.Add(p); }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pictureIds"></param>
        /// <param name="product"></param>
        private void _UpdateProductPictures(IEnumerable<Guid> pictureIds, Product product)
        {
            if (pictureIds == null)
            {
                product.Pictures = new List<Picture>();
                return;
            }

            var selectedPictures = new HashSet<Guid>(pictureIds);
            var oldPictures = new HashSet<Guid>(product.Pictures.Select(c => c.Id));
            foreach (var picture in db.Pictures)
            {
                if (selectedPictures.Contains(picture.Id))
                {
                    if (!oldPictures.Contains(picture.Id))
                    {
                        product.Pictures.Add(picture);
                    }
                }
                else
                {
                    if (oldPictures.Contains(picture.Id))
                    {
                        product.Pictures.Remove(picture);
                    }
                }
            }
        }
    }
}