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
    | Fetched of CartModel
    | Failed of exn

type State = { Cart: Cart; Products: Product array }

let modelToDomain model =
    (model.cart |> Cart.ToDomain, model.products |> Array.map Product.ToDomain)

let init cartModel () =
    let cart, products = modelToDomain cartModel
    { Cart = cart; Products = products }, Cmd.none

let getCartById (id: int) =
    promise {
        let url = $"%s{baseUrl ()}/Shop/Cart/%i{id}"

        let! model = Fetch.get (url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)
        return Fetched model
    }

let update msg state =
    match msg with
    | Fetch cartId -> state, Cmd.OfPromise.either getCartById %cartId id Failed
    | Fetched cartModel ->
        let cart, products = modelToDomain cartModel
        { Cart = cart; Products = products }, Cmd.navigatePath $"/Shop/Cart/%i{%cart.CartId}"
    | Failed exn ->
        JS.console.error exn
        state, Cmd.none

[<ReactComponent>]
let Cart (cart: CartModel) =
    let state, dispatch =
        React.useElmish (init cart, update, [||])

    let idRef = React.useInputRef ()

    JS.console.log state

    Html.div [ Html.input [ prop.ref idRef ]
               Html.button [ prop.onClick
                                 (fun _ ->
                                     if idRef.current.IsSome then
                                         dispatch (Fetch %(int idRef.current.Value.value))) ]
               Html.i %state.Cart.CartId
               for i in state.Products do
                   Html.p $"%i{%i.ProductId}%s{i.Description}" ]
