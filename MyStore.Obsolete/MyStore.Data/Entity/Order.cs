using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using MyStore.Data.Entity.Support;

namespace MyStore.Data.Entity
{
    public record Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        public DateTimeOffset CreateTimeOffset { get; set; }

        [JsonIgnore]
        public ICollection<OrderedProduct> OrderedProducts { get; set; }

        [JsonIgnore]
        public Ticket? SupportTicket { get; set; }
    }
}
