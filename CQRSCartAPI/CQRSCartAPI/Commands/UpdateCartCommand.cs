using CQRSCartAPI.Events;
using CQRSCartAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Commands
{
    public class UpdateCartCommand : Command
    {
        public string CartName { get; set; }
       
        public List<CreateProductCommand> ProductList { get; set; }
        public CartUpdatedEvent ToCartEvent()
        {
            return new CartUpdatedEvent
            {
               CartId=this.Id,
               CartName=this.CartName,

               ProductList = this.ProductList.Select(product => new ProductCreatedEvent
                {
                   ProductName=product.ProductName,
                   Cost=product.Cost,
                   ProductNo=this.Id
                }).ToList()
            };
        }
        public Cart ToCartRecord(Cart record)
        {
            record.CartName = this.CartName;
            record.ProductList = this.ProductList.Select(product => new Product
            {
                Cost=product.Cost,
                ProductId=product.ProductId,
             ProductName=product.ProductName   

            }).ToList();
                
            return record;
        }
    }
}
