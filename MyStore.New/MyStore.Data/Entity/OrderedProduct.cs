using System.Text.Json.Serialization;

namespace MyStore.Data.Entity
{
    public record OrderedProduct
    {
        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public decimal OrderedPrice { get; set; }
    }
}
