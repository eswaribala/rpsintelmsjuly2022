
using CQRSCartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Events
{
    public class CartCreatedEvent : IEvent
    {
        public long CartId { get; set; }
        public string CartName { get; set; }
      
        public List<ProductCreatedEvent> ProductList { get; set; }
        public CartEntity ToCartEntity()
        {
            return new CartEntity
            {
                CartId = this.CartId,
                CartName = this.CartName,
                ProductList = this.ProductList.Select(product => new ProductEntity
                {
                    ProductName = product.ProductName,
                    Cost = product.Cost,
                    ProductId=product.ProductNo
                }).ToList()
            };
        }
    }
}
