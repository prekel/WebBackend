using System;

namespace MyStore.Data.Entity.Support
{
    public class Question
    {
        public int SupportQuestionId { get; set; }
        public int SupportTicketId { get; set; }
        public Ticket SupportTicket { get; set; }
        public DateTimeOffset SendTimestamp { get; set; }
        public DateTimeOffset? ReadTimestamp { get; set; }
        public string Text { get; set; }
    }
}
