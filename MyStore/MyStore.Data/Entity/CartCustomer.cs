namespace MyStore.Data.Entity
{
    public class CartCustomer
    {
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public bool? NullIfPublic { get; set; }
        public bool? NullIfNotCurrent { get; set; }
        
    }
}
