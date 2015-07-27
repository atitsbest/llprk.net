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

        int AddProduct(int cartId, int productId, int qty);

        void UpdateLineItemQty(int cartId, int lineItemId, int qty);
        void UpdateLineItemQty(int cartId, UpdateLineItemQtyRequest[] update);
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
            return db.Carts.Include("LineItems").Include("LineItems.Product").SingleOrDefault(c => c.Id == id);
        }


        public int AddProduct(int cartId, int productId, int qty)
        {
            var db = new Entities();
            var product = db.Products.Single(p => p.Id == productId);
            // Find lineitem with this product.
            var lineItem = product.LineItems.SingleOrDefault(li => li.CartId == cartId);

            if (lineItem != null)
            {
                lineItem.Qty += qty;
            }
            else
            {
                lineItem = new LineItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Qty = qty,
                    Price = product.Price
                };

                db.LineItems.Add(lineItem);
            }
            db.SaveChanges();

            return lineItem.Id;
        }


        public void UpdateLineItemQty(int cartId, int lineItemId, int qty)
        {
            UpdateLineItemQty(cartId, new UpdateLineItemQtyRequest[] { 
                new UpdateLineItemQtyRequest {
                    Id = lineItemId,
                    Qty = qty
                }
            });
        }

        public void UpdateLineItemQty(int cartId, UpdateLineItemQtyRequest[] updates)
        {
            var db = new Entities();

            foreach (var update in updates) {
                var lineItem = db.LineItems.Single(l => l.Id == update.Id);
                if (update.Qty == 0)
                {
                    db.LineItems.Remove(lineItem);
                }
                else
                {
                    lineItem.Qty = update.Qty;
                    db.Entry(lineItem).State = System.Data.Entity.EntityState.Modified;
                }
            }

            db.SaveChanges();
        }
    }
}
