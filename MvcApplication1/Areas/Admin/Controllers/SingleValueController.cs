using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Llprk.DataAccess.Models;
using Llprk.Web.UI.Controllers;

namespace Llprk.Web.UI.Areas.Admin.Controllers
{
    [Authorize, T4MVC(false)]
    public abstract class SingleValueController<T, IDT> : ApplicationController where T: class
    {
        private Func<Entities, DbSet<T>> _CollectionFn;

        /// <summary>
        /// Brauchen wir nicht, aber T4MVC schon.
        /// </summary>
        public SingleValueController()
        {
            //throw new NotImplementedException();
        }

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
            if (_CollectionFn != null)
            {
                var xs = _CollectionFn(db);
                return View(xs.ToList());
            }
            return View(new List<T>());
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

        public ActionResult Edit(IDT id = default(IDT))
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

        public ActionResult Delete(IDT id = default(IDT))
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
        public ActionResult DeleteConfirmed(IDT id)
        {
            var xs = _CollectionFn(db);
            var entity = xs.Find(id);
            xs.Remove(entity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}