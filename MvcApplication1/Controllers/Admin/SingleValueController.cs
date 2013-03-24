using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.Web.UI.Models;

namespace Llprk.Web.UI.Controllers.Admin
{
    [Authorize]
    public abstract class SingleValueController<T> : ApplicationController where T: class
    {
        private Func<Entities, DbSet<T>> _CollectionFn;

        /// <summary>
        /// CTR
        /// </summary>
        /// <param name="collectionFn"></param>
        public SingleValueController(Func<Entities, DbSet<T>> collectionFn)
        {
            if (collectionFn == null) { throw new ArgumentNullException("collectionFn"); }
            _CollectionFn = collectionFn;
        }
        //
        // GET: /Categories/

        public ActionResult Index()
        {
            var xs = _CollectionFn(db);
            return View(xs.ToList());
        }

        //
        // GET: /Categories/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Categories/Create

        [HttpPost]
        public ActionResult Create(T entity)
        {
            if (ModelState.IsValid)
            {
                var xs = _CollectionFn(db);
                xs.Add(entity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(entity);
        }

        //
        // GET: /Categories/Edit/5

        public ActionResult Edit(int id = 0)
        {
            var xs = _CollectionFn(db);
            var entity = xs.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        //
        // POST: /Categories/Edit/5

        [HttpPost]
        public ActionResult Edit(T entity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(entity);
        }

        //
        // GET: /Categories/Delete/5

        public ActionResult Delete(int id = 0)
        {
            var xs = _CollectionFn(db);
            var entity = xs.Find(id);
            if (entity == null)
            {
                return HttpNotFound();
            }
            return View(entity);
        }

        //
        // POST: /Categories/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var xs = _CollectionFn(db);
            var entity = xs.Find(id);
            xs.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}