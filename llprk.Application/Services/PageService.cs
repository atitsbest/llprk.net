using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface IPageService
    {
        void CreatePage(NewPageResponse info);

        EditPageRequest GetPageForEdit(int id);

        void UpdatePage(EditPageResponse info);
    }

    public class PageService : IPageService
    {
        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="info"></param>
        public void CreatePage(NewPageResponse info)
        {
            using (var db = new Entities())
            {
                var page = Mapper.Map<Page>(info);
                page.CreatedAt = DateTime.Now;

                db.Pages.Add(page);
                db.SaveChanges();
            }

        }

        /// <summary>
        /// Get infos to start editing a page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditPageRequest GetPageForEdit(int id)
        {
            using(var db = new Llprk.DataAccess.Models.Entities()) {
                var page = _GetPage(db, id);

                return Mapper.Map<EditPageRequest>(page);
            }
        }

        /// <summary>
        /// Update page with given infos.
        /// </summary>
        /// <param name="info"></param>
        public void UpdatePage(EditPageResponse info)
        {
            if (info == null) throw new ArgumentNullException("info");

            using (var db = new Llprk.DataAccess.Models.Entities())
            {
                var page = _GetPage(db, info.Id);

                Mapper.Map(info, page);
                db.Entry(page).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
            }
        }


        /// <summary>
        /// Get the requested Page.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns>throws ArguementException if not found.</returns>
        private Page _GetPage(Llprk.DataAccess.Models.Entities db, int id)
        {
            var result = db.Pages.SingleOrDefault(p => p.Id == id);
            if (result == null)
            {
                throw new ArgumentException(string.Format("Cannot find page ({0})!", id));
            }

            return result;
        }
    }
}
