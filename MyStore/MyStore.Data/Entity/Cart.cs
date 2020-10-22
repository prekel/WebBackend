using System.Collections.Generic;

namespace MyStore.Data.Entity
{
    public class Cart
    {
        public int CartId { get; set; }
        
        public ICollection<Product> Products { get; set; }
    }
}
