using System.Collections.Generic;
using System.Dynamic;

namespace MyStore.Data.Entity
{
    public class Cart
    {
        public int CartId { get; set; }

        public bool IsPublic { get; set; }
        public int? OwnerCustomerId { get; set; }
        public Customer? OwnerCustomer { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Customer> CurrentCustomers { get; set; }
        
        public List<CartProduct> CartProducts { get; set; }
    }
}
