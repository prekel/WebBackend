using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity.Support
{
    public record Ticket
    {
        public int SupportTicketId { get; set; }
        public int CustomerId { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }

        public int SupportOperatorId { get; set; }

        [JsonIgnore]
        public Operator SupportOperator { get; set; }

        public int? OrderId { get; set; }

        [JsonIgnore]
        public Order? Order { get; set; }

        public DateTimeOffset CreateTimestamp { get; set; }

        [JsonIgnore]
        public ICollection<Answer> SupportAnswers { get; set; }

        [JsonIgnore]
        public ICollection<Question> SupportQuestions { get; set; }
    }
}
