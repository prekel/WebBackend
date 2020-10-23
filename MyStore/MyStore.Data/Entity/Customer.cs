using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using MyStore.Data.Entity.Support;

namespace MyStore.Data.Entity
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Honorific { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public byte[] PasswordHash { get; set; }
        public int PasswordSalt { get; set; }

        public int? CurrentCartId { get; set; }
        public Cart? CurrentCart { get; set; }

        public ICollection<Order> Orders { get; set; }

        public ICollection<Cart> OwnedCarts { get; set; }

        public ICollection<Ticket> SupportTickets { get; set; }
    }
}
