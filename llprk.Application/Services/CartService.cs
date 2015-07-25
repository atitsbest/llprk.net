using AutoMapper;
using Llprk.Application.DTOs.Requests;
using Llprk.Application.DTOs.Responses;
using Llprk.DataAccess.Models;
using System;
using System.Linq;

namespace Llprk.Application.Services
{
    public interface ICartService
    {
        Cart CreateCart();
        Cart GetCart(int id);

        void AddProduct(int cartId, int productId, int qty);
    }

    public class CartService : ICartService
    {
        public Cart CreateCart()
        {
            var db = new Entities();
            var cart = new Cart();
            db.Carts.Add(cart);
            db.SaveChanges();

            return cart;
        }


        public Cart GetCart(int id)
        {
            var db = new Entities();
            return db.Carts.SingleOrDefault(c => c.Id == id);
        }


        public void AddProduct(int cartId, int productId, int qty)
        {
            var db = new Entities();
            var product = db.Products.Single(p => p.Id == productId);
            var lineItem = new LineItem
            {
                CartId = cartId,
                ProductId = productId,
                Qty = qty,
                Price = product.Price
            };

            db.LineItems.Add(lineItem);
            db.SaveChanges();    
        }
    }
}
