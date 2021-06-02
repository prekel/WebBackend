module MyStore.Client.Chats

open FSharp.UMX
open Feliz

open MyStore.Dto.Support
open MyStore.Domain.Support

[<ReactComponent>]
let Chats (ticketsModel: TicketsModel) =
    let tickets =
        ticketsModel.tickets |> Array.map Ticket.ToDomain

    Html.div [ for i in tickets do
                   Html.a [ prop.children [ Html.p $"%A{i}" ]
                            prop.href $"/Support/Chat/%i{%i.SupportTicketId}" ] ]
