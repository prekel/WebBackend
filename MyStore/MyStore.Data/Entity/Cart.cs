using System.Collections.Generic;

namespace MyStore.Data.Entity
{
    public class Cart
    {
        public int CartId { get; set; }
        
        public ICollection<Product> Products { get; set; }
        
        public ICollection<Customer> Customers { get; set; }
        public List<CartCustomer> CartCustomers { get; set; }
        
        public ICollection<Customer> CurrentCustomers { get; set; }
    }
}
