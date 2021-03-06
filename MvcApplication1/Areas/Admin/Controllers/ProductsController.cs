﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using AutoMapper;
using Llprk.Web.UI.Controllers;
using Llprk.Web.UI.Areas.Admin.Models;
using Llprk.Application.Services;
using Llprk.Web.UI.Controllers.Results;
using System.Web;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize]
    public partial class ProductsController : ApplicationController
    {
        private IProductService _ProductService;


        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="products"></param>
        public ProductsController(IProductService products)
        {
            _ProductService = products;
        }

        //
        // GET: /Products/
        public virtual ActionResult Index(string q)
        {
            var products = db.Products.Include(p => p.Category)
                .OrderByDescending(p => p.IsPublished) // Die auch im Shop angezeigt werden zuerst.
                .AsEnumerable();
            if (!string.IsNullOrWhiteSpace(q)) { 
				// Suche...
                products = products
                    .Where(p => p.Name.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1
                            || p.Id.ToString().IndexOf(q) != -1
                            || (p.Description ?? "").IndexOf(q, StringComparison.CurrentCultureIgnoreCase) != -1);
							
            }
            ViewBag.q = q;
            return View(products);
        }

        //
        // GET: /Products/Details/5

        public virtual ActionResult Details(int id = 0)
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

        public virtual ActionResult Create()
        {
            var viewModel = new ProductCreate();
            //viewModel.AllTags = db.Tags.ToArray();
            _PopulateCategoriesDropDownList();
            _PopulateShippingCategoriesDropDownList();
            ViewBag.Title = "New Product";
            return View("edit", viewModel);
        }

        //
        // POST: /Products/Create

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Create(Product product, string pictureIds, int[] tagIds)
        {
            if (ModelState.IsValid) {
                // Produkt mit den Bildern anreichern.
                var guids = _stringToGuids(pictureIds).ToArray();

                _SetProductPictures(product, guids);
                _SetProductTags(product, tagIds);

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        //
        // GET: /Products/Edit/5

        public virtual ActionResult Edit(int id = 0)
        {
            var vm = _ProductService.GetProductForEdit(id);
            return View(Mapper.Map<ProductEdit>(vm));
        }

        //
        // POST: /Products/Edit/5

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult Update(int id, ProductUpdate data)
        {
            if (ModelState.IsValid)
            {
                _ProductService.UpdateProduct(id, data);
                return new EntityResult(MVC.Admin.Products.Index(), "Product saved.");
            }
            else {
                return new Http400Result(ModelState);
            }
            //if (ModelState.IsValid) {
            //    var p = db.Products
            //        .Where(x => x.Id == product.Id)
            //        .Single();

            //    p.CategoryId = product.CategoryId;
            //    p.ShippingCategoryId = product.ShippingCategoryId;
            //    p.Description = product.Description;
            //    p.IsPublished = product.IsPublished;
            //    p.Name = product.Name;
            //    p.Price = product.Price;
            //    p.Available = product.Available;

            //    var guids = _stringToGuids(pictureIds);

            //    _SetProductPictures(p, guids);
            //    _SetProductTags(p, tagIds);

            //    db.Entry(p).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //_PopulateCategoriesDropDownList(product.CategoryId);
            //_PopulateShippingCategoriesDropDownList(product.ShippingCategoryId);
            return View();
        }

        public virtual ActionResult NewImage(HttpPostedFileBase[] file)
        {
            return Content("");
        }

        //
        // GET: /Products/Delete/5

        public virtual ActionResult Delete(int id = 0)
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
        public virtual ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
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
        /// ShippingKategorie-Auswahl befüllen.
        /// </summary>
        /// <param name="selectedCategory"></param>
        private void _PopulateShippingCategoriesDropDownList(object selectedCategory = null)
        {
            var departmentsQuery = from d in db.ShippingCategories
                                   orderby d.Name
                                   select d;
            ViewBag.ShippingCategoryId = new SelectList(departmentsQuery, "Id", "Name", selectedCategory);
        }


        /// <summary>
        /// Tags zum Produkt hinzufügen.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="tagIds"></param>
        private void _SetProductTags(Product product, int[] tagIds) 
        {
            var toRemove = product.Tags.Where(p => !tagIds.Contains(p.Id)).ToArray();
            foreach (var t in toRemove) { product.Tags.Remove(t); }

            foreach (var id in (tagIds ?? new int[]{})) {
                if (!product.Tags.Any(t => t.Id == id)) {
                    product.Tags.Add(db.Tags.Find(id));
                }
            }
        }

        /// <summary>
        /// Bilder zum Produkt hinzufügen.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="pictureIds">Komma-getrennte Guids.</param>
        private void _SetProductPictures(Product product, IEnumerable<Guid> guids)
        {
            var gs = guids.ToArray();

            // Erstmal alle Bilder vom Produkt entfernen, die nicht mehr dazu gehören.
            var toRemove = product.Pictures.Where(p => !guids.Contains(p.Id)).ToArray();
            foreach (var p in toRemove) { product.Pictures.Remove(p); }

            for(var i=0; i<gs.Count(); i+=1) {
                var id = gs[i];
                var pp = product.Pictures.FirstOrDefault(p => p.Id == id);
                if (pp == null) {
                    pp = new Picture() {
                        ProductId = product.Id,
                        //Picture = _GetPictureById(id)
                    };

                    throw new NotImplementedException();

                    product.Pictures.Add(pp);
                }

                // Bilder reihen.
                pp.Pos = i;
            }
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