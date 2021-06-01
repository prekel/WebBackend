module MyStore.Client.Cart

open System
open Elmish
open FSharp.UMX
open Fable.Core
open Feliz
open Feliz.UseDeferred
open MyStore.Domain.SimpleTypes
open Thoth.Fetch
open Thoth.Json
open Fetch
open Fable.Core
open Fable.Core.JsInterop
open Thoth.Json
open Feliz.UseElmish

open MyStore.Dto.Shop
open MyStore.Domain.Shop

module Nullable =
    let IntDecoder =
        Decode.object (fun get -> get.Optional.Raw Decode.int |> Option.toNullable)

    let IntEncoder (nullable: Nullable<int>) =
        Encode.option Encode.int (nullable |> Option.ofNullable)

let extra : ExtraCoders =
    Extra.empty
    |> Extra.withCustom Nullable.IntEncoder Nullable.IntDecoder

let getCartById (id: int) : Async<CartModel> =
    promise {
        let headers =
            [ Fetch.Types.Accept "application/json" ]

        let url = $"https://localhost:5001/Cart/%i{id}"
        return! Fetch.get (url, caseStrategy = CamelCase, extra = extra, headers = headers)
    }
    |> Async.AwaitPromise

type Msg =
    | Increment
    | Fetch of CartId
    | Fetched of CartModel

type State = { Cart: Cart; Products: Product array }

let modelToDomain model =
    (model.cart |> Cart.ToDomain, model.products |> Array.map Product.ToDomain)

let init cartModel () =
    let cart, products = modelToDomain cartModel
    { Cart = cart; Products = products }, Cmd.none

let fetch1 id =
    async {
        let! model = getCartById id
        return Fetched model
    }

let update msg state =
    match msg with
    | Increment -> state, Cmd.none
    | Fetch id -> state, Cmd.OfAsync.result (fetch1 %id)
    | Fetched cartModel ->
        let cart, products = modelToDomain cartModel
        { Cart = cart; Products = products }, Cmd.none

[<ReactComponent>]
let Cart (cart: CartModel) =
    let state, dispatch =
        React.useElmish (init cart, update, [||])

    //
//    let (cart, products), setCartProducts = React.useState (modelToDomain cart)
//
//    let loginState, setLoginState =
//        React.useState Deferred.HasNotStartedYet
//
    let id, setId = React.useState 0
    //
//    let login () = getCartById %id
//
//    let startLogin =
//        React.useDeferredCallback (login, setLoginState)
//
//    let y =
//        match loginState with
//        | Deferred.HasNotStartedYet -> Html.p "HasNotStartedYet"
//        | Deferred.InProgress -> Html.i "InProgress"
//        | Deferred.Failed error -> Html.div error.Message
//        | Deferred.Resolved content ->
//            //setCartProducts (modelToDomain content)
//            Html.div $"Resolved%A{content}"

    JS.console.log state

    Html.div [ Html.input [ prop.onChange (fun (text: string) -> setId (int text)) ]
               Html.button [ prop.onClick (fun t -> dispatch (Fetch %id)) ]
               Html.i %state.Cart.CartId
               for i in state.Products do
                   Html.p $"%i{%i.ProductId}%s{i.Description}" ]
