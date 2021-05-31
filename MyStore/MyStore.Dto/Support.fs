module MyStore.Dto.Support

open System
open System.ComponentModel.DataAnnotations

type AnswerDto =
    { [<Required>]
      SupportAnswerId: int
      [<Required>]
      SupportTicketId: int
      [<Required>]
      SupportOperatorId: int
      [<Required>]
      SendTimestamp: DateTimeOffset
      [<Required>]
      Text: string }

type OperatorDto =
    { [<Required>]
      SupportOperatorId: int
      [<Required>]
      FirstName: string
      [<Required>]
      LastName: string
      [<Required>]
      Email: string
      UserId: string }

type QuestionDto =
    { [<Required>]
      SupportQuestionId: int
      [<Required>]
      SupportTicketId: int
      [<Required>]
      SendTimestamp: DateTimeOffset
      ReadTimestamp: Nullable<DateTimeOffset>
      [<Required>]
      Text: string }

type TicketDto =
    { [<Required>]
      SupportTicketId: int
      [<Required>]
      CustomerId: int
      [<Required>]
      SupportOperatorId: int
      [<Required>]
      OrderId: Nullable<int>
      [<Required>]
      CreateTimestamp: DateTimeOffset }
