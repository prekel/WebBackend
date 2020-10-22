using System;

namespace MyStore.Data.Entity.Support
{
    public class Answer
    {
        public int SupportAnswerId { get; set; }
        public int SupportTicketId { get; set; }
        public int SupportOperatorId { get; set; }
        public DateTimeOffset SendTimestamp { get; set; }
        public string Text { get; set; }
    }
}
