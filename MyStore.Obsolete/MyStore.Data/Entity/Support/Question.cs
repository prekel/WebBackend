using System;
using System.Text.Json.Serialization;

namespace MyStore.Data.Entity.Support
{
    public record Question
    {
        public int SupportQuestionId { get; set; }
        public int SupportTicketId { get; set; }

        [JsonIgnore]
        public Ticket SupportTicket { get; set; }

        public DateTimeOffset SendTimestamp { get; set; }
        public DateTimeOffset? ReadTimestamp { get; set; }
        public string Text { get; set; }
    }
}
