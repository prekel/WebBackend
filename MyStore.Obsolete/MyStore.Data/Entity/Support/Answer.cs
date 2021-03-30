using System;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity.Support
{
    public record Answer
    {
        public int SupportAnswerId { get; set; }
        public int SupportTicketId { get; set; }

        [JsonIgnore]
        public Ticket SupportTicket { get; set; }

        public int SupportOperatorId { get; set; }

        [JsonIgnore]
        public Operator SupportOperator { get; set; }

        public DateTimeOffset SendTimestamp { get; set; }
        public string Text { get; set; }
    }
}
