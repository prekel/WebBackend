module MyStore.Client.Cart

open System

open Elmish
open FSharp.UMX
open Fable.Core
open Feliz
open MyStore.Domain.SimpleTypes
open Thoth.Fetch
open Thoth.Json
open Feliz.UseElmish
open Feliz.Router

open MyStore.Dto.Shop
open MyStore.Domain.Shop

type Msg =
    | Fetch of CartId
    | Fetched of CartModel * string
    | Failed of exn
    | SetCurrent of bool
    | CurrentSet of SetCurrentCartQuery

type State =
    { Cart: Cart
      Products: Product array
      IsCurrent: bool }

let init cartModel () =
    { Cart = cartModel.cart |> Cart.ToDomain
      Products = cartModel.products |> Array.map Product.ToDomain
      IsCurrent = cartModel.isCurrent },
    Cmd.none

let getCartById (id: int) =
    promise {
        let url = $"/Shop/Cart/%i{id}"

        let! model = Fetch.get (baseUrl () + url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)
        return Fetched(model, url)
    }

let setCurrentById (id: int, setCurrent: bool) =
    promise {
        let url =
            $"/Shop/Cart/%i{id}/SetCurrentCart?setCurrent=%b{setCurrent}"

        let! setCurrentCartQuery =
            Fetch.post (baseUrl () + url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)

        return CurrentSet(setCurrentCartQuery)
    }

let update msg state =
    match msg with
    | Fetch cartId -> state, Cmd.OfPromise.either getCartById %cartId id Failed
    | Fetched (cartModel, url) ->
        { Cart = cartModel.cart |> Cart.ToDomain
          Products = cartModel.products |> Array.map Product.ToDomain
          IsCurrent = cartModel.isCurrent },
        Cmd.navigatePath url
    | Failed exn ->
        JS.console.error exn
        state, Cmd.none
    | SetCurrent setCurrent -> state, Cmd.OfPromise.either setCurrentById (%state.Cart.CartId, setCurrent) id Failed
    | CurrentSet scq ->
        { state with
              IsCurrent = scq.setCurrent },
        Cmd.none

[<ReactComponent>]
let Cart (cart: CartModel) =
    let state, dispatch =
        React.useElmish (init cart, update, [||])

    let idRef = React.useInputRef ()

    JS.console.log state

    Html.div [ Html.input [ prop.ref idRef ]
               Html.button [ prop.text "GetById"
                             prop.onClick
                                 (fun _ ->
                                     if idRef.current.IsSome then
                                         dispatch (Fetch %(int idRef.current.Value.value))) ]
               Html.button [ prop.text "Toggle current"
                             prop.onClick (fun _ -> SetCurrent(not state.IsCurrent) |> dispatch) ]
               Html.p $"%A{state}"
               Html.i %state.Cart.CartId
               for i in state.Products do
                   Html.p $"%i{%i.ProductId}%s{i.Description}" ]
