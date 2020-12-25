module Customers

open System
open Feliz
open Feliz.UseListener
open Thoth
open Thoth.Fetch
open Thoth.Json
open Models

let getCustomerById (id: int) =
    promise {
        let url = $"{baseUrl}/Customers/{id}"

        return! Fetch.tryGet<_, Customer> (url, caseStrategy = CamelCase)
    }

let putCustomerById (id: int) customer =
    promise {
        let url = $"{baseUrl}/Customers/{id}"

        return! Fetch.tryPut<Customer, unit> (url, data = customer, caseStrategy = CamelCase)
    }

let postCustomerById customer password =
    promise {
        let url =
            $"{baseUrl}/Customers?password={password}"

        return! Fetch.tryPost<Customer, Customer> (url, data = customer, caseStrategy = CamelCase)
    }

let deleteCustomerById (id: int) =
    promise {
        let url = $"{baseUrl}/Customers/{id}"

        return! Fetch.tryDelete<_, unit> (url, caseStrategy = CamelCase)
    }

let getSkipTake start limit =
    promise {
        let url =
            $"{baseUrl}/Customers?start={start}&limit={limit}"

        return! Fetch.tryGet<_, Customer list> (url, caseStrategy = CamelCase)
    }

let CustomerForm =
    React.functionComponent<{| Customer: Customer |}> (fun props ->
        let customer, setCustomer = React.useState (props.Customer)
        let error, setError = React.useState<FetchError option> (None)
        let status, setStatus = React.useState ("Не было запроса")
        let password, setPassword = React.useState ("")

        (React.useEffect (fun () ->
            match error with
            | Some x -> setStatus (x.ToString())
            | None -> ()),
         [| error |])
        |> ignore

        React.fragment [ Html.button [ prop.text "Get"
                                       prop.onClick (fun _ ->
                                           (getCustomerById customer.CustomerId)
                                               .``then``(fun result ->
                                                   match result with
                                                   | Ok (value) ->
                                                       setError (None)
                                                       setCustomer (value)
                                                       setStatus ("Выполнен get")
                                                   | Error (error) -> setError (Some error))
                                           |> ignore) ]
                         Html.button [ prop.text "Put"
                                       prop.onClick (fun _ ->
                                           (putCustomerById customer.CustomerId customer)
                                               .``then``(fun result ->
                                                   match result with
                                                   | Ok (value) ->
                                                       setError (None)
                                                       setStatus ("Выполнен put")
                                                   | Error (error) -> setError (Some error))
                                           |> ignore) ]
                         Html.button [ prop.text "Post"
                                       prop.onClick (fun _ ->
                                           (postCustomerById customer password)
                                               .``then``(fun result ->
                                                   match result with
                                                   | Ok (value) ->
                                                       setError (None)
                                                       setCustomer (value)
                                                       setStatus ("Выполнен post")
                                                   | Error (error) -> setError (Some error))
                                           |> ignore) ]
                         Html.button [ prop.text "Delete"
                                       prop.onClick (fun _ ->
                                           (deleteCustomerById customer.CustomerId)
                                               .``then``(fun result ->
                                                   match result with
                                                   | Ok (value) ->
                                                       setError (None)
                                                       setStatus ("Выполнен delete")
                                                   | Error (error) -> setError (Some error))
                                           |> ignore) ]
                         Html.label [ prop.text status ]
                         Html.br []
                         Html.label [ prop.text "Id" ]
                         Html.input [ prop.value customer.CustomerId
                                      prop.onChange (fun (s: string) ->
                                          setCustomer
                                              ({ customer with
                                                     CustomerId = Int32.Parse(s) })) ]
                         Html.br []
                         Html.label [ prop.text "Имя" ]
                         Html.input [ prop.value customer.FirstName
                                      prop.onChange (fun (s: string) -> setCustomer ({ customer with FirstName = s })) ]
                         Html.br []
                         Html.label [ prop.text "Фамилия" ]
                         Html.input [ prop.value
                                          (match customer.LastName with
                                           | Some (x) -> x
                                           | None -> "")
                                      prop.onChange (fun (s: string) ->
                                          setCustomer ({ customer with LastName = Some s })) ]
                         Html.br []
                         Html.label [ prop.text "Обращение" ]
                         Html.input [ prop.value
                                          (match customer.Honorific with
                                           | Some (x) -> x
                                           | None -> "")
                                      prop.onChange (fun (s: string) ->
                                          setCustomer ({ customer with Honorific = Some s })) ]
                         Html.br []
                         Html.label [ prop.text "E-mail" ]
                         Html.input [ prop.value customer.Email
                                      prop.onChange (fun (s: string) -> setCustomer ({ customer with Email = s })) ]
                         Html.br []
                         Html.label [ prop.text "Номер корзины" ]
                         Html.input [ prop.value
                                          (match customer.CurrentCartId with
                                           | Some (x) -> x.ToString()
                                           | None -> "")
                                      prop.onChange (fun (s: string) ->
                                          setCustomer
                                              ({ customer with
                                                     CurrentCartId =
                                                         match s with
                                                         | "" -> None
                                                         | x -> Int32.Parse(x) |> Some })) ]
                         Html.br []
                         Html.label [ prop.text "Пароль" ]
                         Html.input [ prop.value password
                                      prop.type' "password"
                                      prop.onChange (fun (s: string) -> setPassword (s)) ]
                         Html.br [] ])

let Customers =
    React.functionComponent<{| Customers: Customer list |}> (fun props ->
        React.fragment [ Html.table [ yield Html.th [ Html.p "Id" ]
                                      yield Html.th [ Html.p "Имя" ]
                                      yield Html.th [ Html.p "Фамилия" ]
                                      yield Html.th [ Html.p "Обращение" ]
                                      yield Html.th [ Html.p "E-mail" ]
                                      yield Html.th [ Html.p "Номер корзины" ]
                                      for c in props.Customers do
                                          yield
                                              Html.tr [ Html.td [ Html.p c.CustomerId ]
                                                        Html.td [ Html.p c.FirstName ]
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
                         Customers {| Customers = customers |}
                         CustomerForm
                             {| Customer =
                                    match customers |> List.tryHead with
                                    | Some x -> x
                                    | None ->
                                        { CustomerId = 021
                                          FirstName = "state1.ToString()"
                                          LastName = Some "123"
                                          Honorific = Some "123"
                                          Email = ""
                                          CurrentCartId = Some 12 } |} ])
