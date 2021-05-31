namespace MyStore.Dto.Support

open System

type AnswerDto =
    { supportAnswerId: int
      supportTicketId: int
      supportOperatorId: int
      sendTimestamp: DateTimeOffset
      text: string }

type OperatorDto =
    { supportOperatorId: int
      firstName: string
      lastName: string
      email: string
      userId: string option }

type QuestionDto =
    { supportQuestionId: int
      supportTicketId: int
      sendTimestamp: DateTimeOffset
      readTimestamp: DateTimeOffset option
      text: string }

type TicketDto =
    { supportTicketId: int
      customerId: int
      supportOperatorId: int
      orderId: int option
      createTimestamp: DateTimeOffset }
