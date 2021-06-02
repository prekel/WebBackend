namespace MyStore.Dto.Support

open System

#if FABLE_COMPILER
open Fable.System.ComponentModel.Annotations
#else
open System.ComponentModel.DataAnnotations
#endif

type AnswerDto =
    { [<Required>]
      supportAnswerId: int
      [<Required>]
      supportTicketId: int
      [<Required>]
      supportOperatorId: int
      [<Required>]
      sendTimestamp: DateTimeOffset
      [<Required>]
      text: string }

type OperatorDto =
    { [<Required>]
      supportOperatorId: int
      [<Required>]
      firstName: string
      [<Required>]
      lastName: string
      [<Required>]
      email: string }

type QuestionDto =
    { [<Required>]
      supportQuestionId: int
      [<Required>]
      supportTicketId: int
      [<Required>]
      sendTimestamp: DateTimeOffset
      readTimestamp: Nullable<DateTimeOffset>
      [<Required>]
      text: string }

type TicketDto =
    { [<Required>]
      supportTicketId: int
      [<Required>]
      customerId: int
      supportOperatorId: Nullable<int>
      orderId: Nullable<int>
      [<Required>]
      createTimestamp: DateTimeOffset }

type TicketsModel = { tickets: TicketDto array }
