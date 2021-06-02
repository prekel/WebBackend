namespace MyStore.Domain.Support

open System

open FSharp.UMX

open MyStore.Domain.SimpleTypes
open MyStore.Dto.Support

type Operator =
    { SupportOperatorId: OperatorId
      FirstName: string
      LastName: string
      Email: Email }
    static member ToDomain(dto: OperatorDto) =
        { SupportOperatorId = %dto.supportOperatorId
          FirstName = dto.firstName
          LastName = dto.lastName
          Email = %dto.email }

    member this.FromDomain() =
        { OperatorDto.supportOperatorId = %this.SupportOperatorId
          firstName = this.FirstName
          lastName = this.LastName
          email = %this.Email }

type Answer =
    { SupportAnswerId: AnswerId
      SupportTicketId: TicketId
      SupportOperatorId: OperatorId
      SendTimestamp: DateTimeOffset
      Text: string }
    static member ToDomain(dto: AnswerDto) =
        { SupportAnswerId = %dto.supportAnswerId
          SupportTicketId = %dto.supportTicketId
          SupportOperatorId = %dto.supportOperatorId
          SendTimestamp = dto.sendTimestamp
          Text = dto.text }

    member this.FromDomain() =
        { AnswerDto.supportAnswerId = %this.SupportAnswerId
          supportTicketId = %this.SupportTicketId
          supportOperatorId = %this.SupportOperatorId
          sendTimestamp = this.SendTimestamp
          text = this.Text }

type Question =
    { SupportQuestionId: QuestionId
      SupportTicketId: TicketId
      SendTimestamp: DateTimeOffset
      ReadTimestamp: DateTimeOffset option
      Text: string }

    static member ToDomain(dto: QuestionDto) =
        { SupportQuestionId = %dto.supportQuestionId
          SupportTicketId = %dto.supportTicketId
          SendTimestamp = %dto.sendTimestamp
          ReadTimestamp =
              dto.readTimestamp
              |> Option.ofNullable
              |> Option.map (~%)
          Text = dto.text }

    member this.FromDomain() =
        { QuestionDto.supportQuestionId = %this.SupportQuestionId
          supportTicketId = %this.SupportTicketId
          sendTimestamp = %this.SendTimestamp
          readTimestamp =
              this.ReadTimestamp
              |> Option.map (~%)
              |> Option.toNullable
          text = this.Text }

type Ticket =
    { SupportTicketId: TicketId
      CustomerId: CustomerId
      SupportOperatorId: OperatorId
      OrderId: OrderId option
      CreateTimestamp: DateTimeOffset }

    static member ToDomain(dto: TicketDto) =
        { SupportTicketId = %dto.supportTicketId
          CustomerId = %dto.customerId
          SupportOperatorId = %dto.supportOperatorId
          OrderId =
              dto.orderId
              |> Option.ofNullable
              |> Option.map (~%)
          CreateTimestamp = dto.createTimestamp }

    member this.FromDomain() =
        { TicketDto.supportTicketId = %this.SupportTicketId
          customerId = %this.CustomerId
          supportOperatorId = %this.SupportOperatorId
          orderId =
              this.OrderId
              |> Option.map (~%)
              |> Option.toNullable
          createTimestamp = this.CreateTimestamp }
