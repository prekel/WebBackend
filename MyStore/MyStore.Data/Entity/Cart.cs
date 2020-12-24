using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity
{
    public record Cart
    {
        public int CartId { get; set; }

        public bool IsPublic { get; set; }
        public int? OwnerCustomerId { get; set; }

        [JsonIgnore]
        public Customer? OwnerCustomer { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products { get; set; }

        [JsonIgnore]
        public ICollection<Customer> CurrentCustomers { get; set; }

        [JsonIgnore]
        public List<CartProduct> CartProducts { get; set; }
    }
}
