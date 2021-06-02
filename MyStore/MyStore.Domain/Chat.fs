module MyStore.Domain.Chat

open MyStore.Domain.SimpleTypes
open MyStore.Domain.Support

module SignalRHub =
    type Role =
        | Operator
        | Customer

    type ActionType =
        | JoinRoom
        | Message of string
        | LeaveRoom

    type Action =
        { Role: Role
          TicketId: TicketId
          Action: ActionType }

    type Response =
        | Joined of Ticket
        | NewAnswer of Answer
        | NewQuestion of Question
        | LeaveDone of TicketId
        | Forbidden of TicketId
        | NotFound of TicketId

module Endpoints =
    [<Literal>]
    let Root = "/Support/Chat/Ws"
