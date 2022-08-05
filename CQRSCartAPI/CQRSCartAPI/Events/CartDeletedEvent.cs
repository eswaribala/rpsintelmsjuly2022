using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSCartAPI.Events
{
    public class CartDeletedEvent : IEvent
    {
        public long CartId { get; set; }
    }
}
