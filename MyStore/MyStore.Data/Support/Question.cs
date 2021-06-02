using System;

using MyStore.Dto.Support;

namespace MyStore.Data.Support
{
    public record Question
    {
        public int SupportQuestionId { get; set; }
        public int SupportTicketId { get; set; }

        public Ticket SupportTicket { get; set; }

        public DateTimeOffset SendTimestamp { get; set; }
        public DateTimeOffset? ReadTimestamp { get; set; }
        public string Text { get; set; }

        public QuestionDto ToDto() => new(SupportQuestionId, SupportTicketId, SendTimestamp, ReadTimestamp, Text);
    }
}
