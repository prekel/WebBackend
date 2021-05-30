using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity.Support
{
    public record Operator
    {
        public int SupportOperatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }


        public byte[] PasswordHash { get; set; }


        public int PasswordSalt { get; set; }


        public ICollection<Answer> SupportAnswers { get; set; }


        public ICollection<Ticket> SupportTickets { get; set; }
    }
}
