module MyStore.Domain.Chat

module SignalRHub =
    type Action =
        | IncrementCount of int
        | DecrementCount of int

    type Response =
        | NewCount of int
        | TickerCount of string

module Endpoints =
    [<Literal>]
    let Root = "/Support/Chat/Ws"
