module Customers

open System
open Fable.Core
open Feliz
open Feliz.UseListener
open Thoth
open Thoth.Fetch
open Thoth.Json
open Models


let Customers =
    React.functionComponent<{| Customers: Customer list |}> (fun props ->
        React.fragment [ Html.table [ yield Html.th [ Html.p "Имя" ]
                                      yield Html.th [ Html.p "Фамилия" ]
                                      yield Html.th [ Html.p "Обращение" ]
                                      yield Html.th [ Html.p "E-mail" ]
                                      yield Html.th [ Html.p "Номер корзины" ]
                                      for c in props.Customers do
                                          yield
                                              Html.tr [ Html.td [ Html.p c.FirstName ]
                                                        Html.td [ Html.p
                                                                      (match c.LastName with
                                                                       | Some (x) -> x
                                                                       | None -> "") ]
                                                        Html.td [ Html.p
                                                                      (match c.Honorific with
                                                                       | Some (x) -> x
                                                                       | None -> "") ]
                                                        Html.td [ Html.p c.Email ]
                                                        Html.td [ Html.p
                                                                      (match c.CurrentCartId with
                                                                       | Some (x) -> x.ToString()
                                                                       | None -> "") ] ] ] ])

let getBookById (id: int): JS.Promise<Customer> =
    promise {
        let url =
            sprintf "http://localhost:5000/Customers/%i" id

        return! Fetch.get (url, caseStrategy = CamelCase)
    }

let getSkipTake start limit =
    promise {
        let url =
            $"http://localhost:5000/Customers?start={start}&limit={limit}"

        return! Fetch.tryGet<_, Customer list> (url, caseStrategy = CamelCase)
    }

let CustomersPage =
    React.functionComponent (fun () ->
        let id, setId = React.useState (2)

        let customers, setCustomers =
            React.useState<Customer list>
                ([ { CustomerId = 021
                     FirstName = "state1.ToString()"
                     LastName = Some "123"
                     Honorific = Some "123"
                     Email = ""
                     CurrentCartId = Some 12 } ])

        let error, setError = React.useState<FetchError option> (None)

        React.fragment [ Html.button [ prop.text "Показать"
                                       prop.onClick (fun _ ->
                                           (getSkipTake id 10)
                                               .``then``(fun result ->
                                                   match result with
                                                   | Ok (value) -> setCustomers (value)
                                                   | Error (error) -> setError (Some error))
                                           |> ignore) ]
                         Html.input [ prop.onChange (fun (e: string) -> setId (Int32.Parse(e))) ]
                         Customers {| Customers = customers |} ])
