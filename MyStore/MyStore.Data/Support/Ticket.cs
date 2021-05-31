using System;
using System.Collections.Generic;

using MyStore.Data.Shop;

namespace MyStore.Data.Support
{
    public record Ticket
    {
        public int SupportTicketId { get; set; }
        public int CustomerId { get; set; }


        public Customer Customer { get; set; }

        public int SupportOperatorId { get; set; }


        public Operator SupportOperator { get; set; }

        public int? OrderId { get; set; }


        public Order? Order { get; set; }

        public DateTimeOffset CreateTimestamp { get; set; }


        public ICollection<Answer> SupportAnswers { get; set; }


        public ICollection<Question> SupportQuestions { get; set; }
    }
}
