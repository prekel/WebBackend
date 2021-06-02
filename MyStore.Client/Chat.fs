module MyStore.Client.Chat

open Fable.Core
open Fable.Import
open Feliz
open MyStore.Domain.SimpleTypes

open Fable.SignalR.Feliz
open Fable.SignalR


open MyStore.Dto.Support
open MyStore.Domain.Support
open MyStore.Domain.Chat
open MyStore.Domain.Chat.SignalRHub

type Status =
    | Join
    | Leave
    | Undefined
    | ForbiddenStatus
    | NotFoundStatus

type State =
    { Ticket: Ticket
      Questions: Question list
      Answers: Answer list
      Status: Status }

type QuestionOrAnswer =
    | Question of Question
    | Answer of Answer

//[<ReactComponent>]
let ChatDisplay (st: {| chat: State |}) =
    Html.div [ Html.p $"%A{st.chat}"
               Html.p $"Ticket: %A{st.chat.Ticket}"
               Html.p "Answers:"
               for i in st.chat.Answers do
                   Html.p $"%A{i}"
               Html.p "Questions: "
               for i in st.chat.Questions do
                   Html.p $"%A{i}" ]
//    let messages =
//        st.chat.Answers
//        |> List.map Answer
//        |> List.append (st.chat.Questions |> List.map Question)
//        |> List.sortBy
//            (function
//            | Question q -> q.SendTimestamp
//            | Answer a -> a.SendTimestamp)
//
//    Html.div [ Html.p $"Ticket: %A{st.chat.Ticket}"
//               for i in messages do
//                   Html.p $"%A{i}" ]

//[<ReactComponent>]
let Buttons
    (input: {| ticketId: TicketId
               text: string
               hub: Hub<Action, Response> |})
    =
    React.fragment [ Html.button [ prop.text "Join as operator"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Operator
                                             TicketId = input.ticketId
                                             Action = JoinRoom } ]
                     Html.button [ prop.text "Join as customer"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Customer
                                             TicketId = input.ticketId
                                             Action = JoinRoom } ]
                     Html.button [ prop.text "Send as operator"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Operator
                                             TicketId = input.ticketId
                                             Action = Message input.text } ]
                     Html.button [ prop.text "Send as customer"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Customer
                                             TicketId = input.ticketId
                                             Action = Message input.text } ]
                     Html.button [ prop.text "Leave as operator"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Operator
                                             TicketId = input.ticketId
                                             Action = LeaveRoom } ]
                     Html.button [ prop.text "Leave as customer"
                                   prop.onClick
                                   <| fun _ ->
                                       input.hub.sendNow
                                           { Action.Role = Customer
                                             TicketId = input.ticketId
                                             Action = LeaveRoom } ] ]

[<ReactComponent>]
let TrueChat (st: {| chat: State |}) =
    let state, setState = React.useState st.chat

    let text, setText = React.useState ""

    let hub =
        React.useSignalR<Action, Response>
            (fun hub ->
                hub.withUrl(Endpoints.Root).withAutomaticReconnect()
                    .configureLogging(
                    LogLevel.Debug
                )
                    .onMessage
                <| function
                | Joined newTicket ->
                    setState
                        { state with
                              Ticket = newTicket
                              Status = Join }
                | NewAnswer answer ->
                    setState
                        { state with
                              Answers = answer :: state.Answers }
                | NewQuestion question ->
                    setState
                        { state with
                              Questions = question :: state.Questions }
                | LeaveDone _ -> setState { state with Status = Leave }
                | Forbidden _ -> setState { state with Status = ForbiddenStatus }
                | NotFound _ -> setState { state with Status = NotFoundStatus })

    Html.div [ Html.input [ prop.onChange setText ]
               ChatDisplay {| chat = state |}
               Buttons
                   {| ticketId = state.Ticket.SupportTicketId
                      text = text
                      hub = hub.current |} ]

[<ReactComponent>]
let Chat (chatModel: ChatModel) =
    let state =
        { Ticket = chatModel.ticket |> Ticket.ToDomain
          Questions =
              chatModel.questions
              |> Array.map Question.ToDomain
              |> Array.toList
          Answers =
              chatModel.answers
              |> Array.map Answer.ToDomain
              |> Array.toList
          Status = Undefined }

    let isServer =
        try
            JS.console.log Browser.Dom.window.location.origin
            false
        with ex -> true

    if isServer then
        ChatDisplay {| chat = state |}
    else
        TrueChat {| chat = state |}
