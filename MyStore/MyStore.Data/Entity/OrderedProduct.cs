namespace MyStore.Data.Entity
{
    public class OrderedProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public decimal OrderedPrice { get; set; }
    }
}
