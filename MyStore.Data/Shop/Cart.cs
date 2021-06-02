using System.Collections.Generic;

using MyStore.Dto.Shop;

namespace MyStore.Data.Shop
{
    public record Cart
    {
        public int CartId { get; set; }

        public bool IsPublic { get; set; }
        public int? OwnerCustomerId { get; set; }


        public Customer? OwnerCustomer { get; set; }


        public ICollection<Product>? Products { get; set; }


        public ICollection<Customer>? CurrentCustomers { get; set; }


        public List<CartProduct>? CartProducts { get; set; }

        public CartDto ToDto() => new(CartId, IsPublic, OwnerCustomerId);
    }
}
