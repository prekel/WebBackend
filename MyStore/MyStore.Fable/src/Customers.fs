module Customers

open Browser.Types
open Fable.Core
open Fable.Core.JS
open Fable.React
open Feliz
open Feliz.Router
open Feliz.UseElmish
open Feliz.UseListener
open Thoth.Fetch
open Thoth.Json
open Models

let Customers =
    React.functionComponent<{| Customers: Customer list |}> (fun props ->
        React.fragment [ Html.table [ Html.tr [ Html.td [ Html.p
                                                              (props.Customers
                                                               |> List.head
                                                               |> (fun o -> o.FirstName)) ]
                                                Html.td [ Html.p "12" ] ]
                                      Html.tr [ Html.td [ Html.p "21" ]
                                                Html.td [ Html.p "22" ] ] ] ])

let getBookById (id: int): JS.Promise<Customer> =
    promise {
        let url =
            sprintf "http://localhost:5000/Customers/%i" id

        return! Fetch.get (url, caseStrategy = CamelCase)
    }

let CustomersPage =
    React.functionComponent (fun () ->
        let id, setId = React.useState (2)

        let customers, setCustomers =
            React.useState<Customer list>
                ([ { CustomerId = 021
                     FirstName = "state1.ToString()"
                     LastName = Some "123"
                     Honorific = "123"
                     Email = ""
                     CurrentCartId = Some 12 } ])

        let error, setError =
            React.useState<Browser.Types.ErrorEvent option> (None)

        (React.useEffect (fun () ->
            (getBookById id)
                .``then``(fun result -> setCustomers ([ result ]), setError)
            |> ignore),
         [||])
        |> ignore

        React.fragment [ Html.button [ prop.onClick (fun _ -> setId (id + 1)) ]
                         Customers {| Customers = [ customers |> List.head ] |} ])
