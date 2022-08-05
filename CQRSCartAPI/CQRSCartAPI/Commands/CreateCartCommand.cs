using CQRSCartAPI.Events;
using CQRSCartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Commands
{
    public class CreateCartCommand : Command
    {
        public string CartName { get; set; }
        
        public List<CreateProductCommand> ProductList { get; set; }
        public CartCreatedEvent ToCartEvent(long id)
        {
            return new CartCreatedEvent
            {
                CartId = id,
                CartName = this.CartName,
                
                ProductList = this.ProductList.Select(product => 
                new ProductCreatedEvent {
                    ProductName=product.ProductName,
                    Cost = product.Cost,
                    ProductNo=product.ProductId
                }).ToList()
            };
        }
        public Cart ToCartRecord()
        {
            return new Cart
            {
                CartName = this.CartName,
               ProductList= this.ProductList.Select
                (product => new Product{ Cost = product.Cost,
                  ProductName=product.ProductName,
                  ProductId=product.ProductId,
                }).ToList()
            };
        }
    }
}
