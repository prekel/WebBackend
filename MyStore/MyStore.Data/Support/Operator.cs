using System.Collections.Generic;

using MyStore.Data.Identity;

namespace MyStore.Data.Support
{
    public record Operator
    {
        public int SupportOperatorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string? UserId { get; set; }

        public ApplicationUser? User { get; set; }


        public ICollection<Answer> SupportAnswers { get; set; }


        public ICollection<Ticket> SupportTickets { get; set; }
    }
}
