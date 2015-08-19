using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface IProductService
    {
        //void CreateProduct(NewProductRequest info);

        EditProductResponse GetProductForEdit(int id);

        void UpdateProduct(int id, UpdateProductRequest request);
    }

    public class ProductService : IProductService
    {
        /// <summary>
        /// Get infos to start editing a page.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public EditProductResponse GetProductForEdit(int id)
        {
            using(var db = new Llprk.DataAccess.Models.Entities()) {
                var product = _GetProduct(db, id);

                return Mapper.Map<EditProductResponse>(product);
            }
        }

        /// <summary>
        /// Update product.
        /// </summary>
        /// <param name="request"></param>
        public void UpdateProduct(int id, UpdateProductRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");

            using(var db = new Llprk.DataAccess.Models.Entities()) {
                var product = _GetProduct(db, id);

                Mapper.Map(request, product);
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Get the requested Product.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="id"></param>
        /// <returns>throws ArguementException if not found.</returns>
        private Product _GetProduct(Llprk.DataAccess.Models.Entities db, int id)
        {
            var result = db.Products.SingleOrDefault(p => p.Id == id);
            if (result == null)
            {
                throw new ArgumentException(string.Format("Cannot find product ({0})!", id));
            }

            return result;
        }
    }
}
