module App

open Fable.React
open Feliz
open Feliz.Router
open Feliz.UseElmish
open Customers
open Models

type State = { CurrentUrl: string list }
type Msg = UrlChanged of string list

let init () =
    { CurrentUrl = Router.currentUrl () }, Elmish.Cmd.none

let update (UrlChanged segments) state =
    { state with CurrentUrl = segments }, Elmish.Cmd.none

let router =
    FunctionComponent.Of(fun () ->
        let state, dispatch = React.useElmish (init, update, [||])

        React.router [ router.onUrlChanged (UrlChanged >> dispatch)

                       router.children [ match state.CurrentUrl with
                                         | [] -> Html.h1 "Home"
                                         | [ "users" ] ->
                                             Customers
                                                 {| Customers =
                                                        [ { CustomerId = 0
                                                            FirstName = "123"
                                                            LastName = Some "123"
                                                            Honorific = "123"
                                                            Email = ""
                                                            CurrentCartId = Some 12 } ] |}
                                         | [ "users"; Route.Int userId ] -> Html.h1 (sprintf "User ID %d" userId)
                                         | [ "customers" ] -> CustomersPage()
                                         | _ -> Html.h1 "Not found" ] ])

[<ReactComponent>]
let HelloWorld () = React.fragment [ router () ]
