using AutoMapper;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;

namespace Llprk.Application.Services
{
    public interface IPageService
    {
        void CreatePage(NewPageResponse info);
    }

    public class PageService : IPageService
    {
        /// <summary>
        /// Creates a new page.
        /// </summary>
        /// <param name="info"></param>
        public void CreatePage(NewPageResponse info)
        {
            var db = new Llprk.DataAccess.Models.Entities();
            var page = Mapper.Map<Page>(info);

            db.Pages.Add(page);
            db.SaveChanges();

        }
    }
}
