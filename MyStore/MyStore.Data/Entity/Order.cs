using System;
using System.Collections.Generic;

namespace MyStore.Data.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        
        public DateTimeOffset CreateTimeOffset { get; set; }
        
        public ICollection<OrderedProduct> OrderedProducts { get; set; }
    }
}
