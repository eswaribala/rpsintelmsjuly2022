using CQRSCartAPI.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Commands
{
    public class DeleteCartCommand : Command
    {
        internal CartDeletedEvent ToCartEvent()
        {
            return new CartDeletedEvent
            {
                CartId = this.Id
            };
        }
    }
}
