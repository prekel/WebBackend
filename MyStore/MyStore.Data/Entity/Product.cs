using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity
{
    public record Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [JsonIgnore]
        public ICollection<Cart>? Carts { get; set; }

        [JsonIgnore]
        public ICollection<OrderedProduct>? OrderedProducts { get; set; }

        [JsonIgnore]
        public List<CartProduct>? CartProducts { get; set; }
    }
}
