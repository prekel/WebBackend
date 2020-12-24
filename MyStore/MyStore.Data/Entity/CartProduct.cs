using System.Text.Json.Serialization;

namespace MyStore.Data.Entity
{
    public record CartProduct
    {
        public int CartId { get; set; }

        [JsonIgnore]
        public Cart Cart { get; set; }

        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}
