using System;
using System.Collections.Generic;

using MyStore.Data.Support;

namespace MyStore.Data.Shop
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
