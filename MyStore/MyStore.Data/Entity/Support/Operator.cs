using System.Collections.Generic;

namespace MyStore.Data.Entity.Support
{
    public class Operator
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
