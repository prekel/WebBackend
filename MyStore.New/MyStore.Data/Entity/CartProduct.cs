using System.Text.Json.Serialization;

namespace MyStore.Data.Entity
{
    public record CartProduct
    {
        public int CartId { get; set; }


        public Cart Cart { get; set; }

        public int ProductId { get; set; }


        public Product Product { get; set; }
    }
}
