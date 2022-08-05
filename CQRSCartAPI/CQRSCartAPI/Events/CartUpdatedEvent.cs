using CQRSCartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Events
{
    public class CartUpdatedEvent : IEvent
    {
        public long CartId { get; set; }
        public string CartName { get; set; }
      
        public List<ProductCreatedEvent> ProductList{ get; set; }
        public CartEntity ToCartEntity(CartEntity CartEntity)
        {
            return new CartEntity
            {
                CartId = this.CartId,
                CartName = CartEntity.CartName.Equals(this.CartName) ? CartEntity.CartName : this.CartName,
                ProductList = GetNewOnes(CartEntity.ProductList)
                .Select(product => new ProductEntity { 
                    ProductId = product.ProductId ,
                    ProductName = product.ProductName,  
                    Cost = product.Cost
                   }).ToList()
            };
        }
        private List<ProductEntity> GetNewOnes(List<ProductEntity> ProductList)
        {
            return ProductList.Where(a => !this.ProductList.Any(x => x.ProductNo== a.ProductId
                && x.ProductName == a.ProductName
                && x.Cost== a.Cost)).ToList<ProductEntity>();
        }
    }
}
