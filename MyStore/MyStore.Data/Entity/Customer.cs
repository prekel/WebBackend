using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyStore.Data.Entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Honorific { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public int PasswordSalt { get; set; }
    }
}
