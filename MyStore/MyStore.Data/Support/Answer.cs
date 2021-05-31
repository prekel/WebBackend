using System;

namespace MyStore.Data.Support
{
    public record Answer
    {
        public int SupportAnswerId { get; set; }
        public int SupportTicketId { get; set; }


        public Ticket SupportTicket { get; set; }

        public int SupportOperatorId { get; set; }


        public Operator SupportOperator { get; set; }

        public DateTimeOffset SendTimestamp { get; set; }
        public string Text { get; set; }

        public Dto.Support.AnswerDto ToDto() =>
            new(SupportAnswerId, SupportTicketId, SupportOperatorId, SendTimestamp, Text);
    }
}
