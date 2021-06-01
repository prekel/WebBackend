module MyStore.Web.Chat

open Fable.SignalR
open MyStore.Domain.Chat

open FSharp.Control.Tasks

module SignalRHub =

    let update (msg: SignalRHub.Action) =
        match msg with
        | SignalRHub.Action.IncrementCount i -> SignalRHub.Response.NewCount(i + 1)
        | SignalRHub.Action.DecrementCount i -> SignalRHub.Response.NewCount(i - 1)

    let invoke (msg: SignalRHub.Action) _ = task { return update msg }

    let send (msg: SignalRHub.Action) (hubContext: FableHub<SignalRHub.Action, SignalRHub.Response>) =
        update msg |> hubContext.Clients.Caller.Send

    let config =
        { Fable.SignalR.SignalR.Settings.EndpointPattern = Endpoints.Root
          Fable.SignalR.SignalR.Settings.Send = send
          Fable.SignalR.SignalR.Settings.Invoke = invoke
          Fable.SignalR.SignalR.Settings.Config = None }
