using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using MyStore.Data.Entity.Support;

namespace MyStore.Data.Entity
{
    public record Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Honorific { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [JsonIgnore]
        public byte[]? PasswordHash { get; set; }

        [JsonIgnore]
        public int PasswordSalt { get; set; }

        public int? CurrentCartId { get; set; }

        [JsonIgnore]
        public Cart? CurrentCart { get; set; }

        [JsonIgnore]
        public ICollection<Order>? Orders { get; set; }

        [JsonIgnore]
        public ICollection<Cart>? OwnedCarts { get; set; }

        [JsonIgnore]
        public ICollection<Ticket>? SupportTickets { get; set; }
    }
}
