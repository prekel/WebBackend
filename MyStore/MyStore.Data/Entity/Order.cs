using System;
using System.Collections.Generic;

using MyStore.Data.Entity.Support;

namespace MyStore.Data.Entity
{
    public record Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public DateTimeOffset CreateTimeOffset { get; set; }

        public ICollection<OrderedProduct> OrderedProducts { get; set; }
        
        public Ticket? SupportTicket { get; set; }
    }
}
