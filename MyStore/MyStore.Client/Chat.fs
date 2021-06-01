module MyStore.Client.Chat


open Elmish
open FSharp.UMX
open Fable.Core
open Fable.Import
open Feliz
open MyStore.Domain.SimpleTypes
open Thoth.Fetch
open Thoth.Json
open Feliz.UseElmish
open Feliz.Router

open Fable.SignalR.Feliz
open Fable.SignalR


open MyStore.Dto.Support
open MyStore.Domain.Support
open MyStore.Domain.Chat

let TextDisplay (input: {| count: int; text: string |}) =
    React.fragment [ Html.div input.count
                     Html.div input.text ]

[<ReactComponent>]
let Buttons
    (input: {| count: int
               hub: Hub<SignalRHub.Action, SignalRHub.Response> |})
    =
    React.fragment [ Html.button [ prop.text "Increment"
                                   prop.onClick
                                   <| fun _ -> input.hub.sendNow (SignalRHub.Action.IncrementCount input.count) ]
                     Html.button [ prop.text "Decrement"
                                   prop.onClick
                                   <| fun _ -> input.hub.sendNow (SignalRHub.Action.DecrementCount input.count) ]
                     Html.button [ prop.text "Get Random Character"
                                   prop.onClick
                                   <| fun _ -> input.hub.sendNow (SignalRHub.Action.DecrementCount 10) ] ]

let TrueChat () =
    let count, setCount = React.useState 0
    let text, setText = React.useState ""

    let hub =
        React.useSignalR<SignalRHub.Action, SignalRHub.Response>
            (fun hub ->
                hub.withUrl(Endpoints.Root).withAutomaticReconnect()
                    .configureLogging(
                    LogLevel.Debug
                )
                    .onMessage
                <| function
                | SignalRHub.Response.NewCount i -> setCount i
                | SignalRHub.Response.TickerCount str -> setText str)

    Html.div [ prop.children [ TextDisplay {| count = count; text = text |}
                               Buttons {| count = count; hub = hub.current |} ] ]

[<ReactComponent>]
let Chat obj =
    //let conn, setConn = React.useState false

    let isServer =
        try
            JS.console.log Browser.Dom.window.location.origin
            true
        with ex -> false

    if isServer then
        TrueChat()
    else
        Html.div []
//else
//    Html.button [ prop.onClick (fun _ -> setConn true) ]
