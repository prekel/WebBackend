module MyStore.Client.Cart

open System
open FSharp.UMX
open Fable.Core
open Feliz
open Feliz.UseDeferred
open Thoth.Fetch
open Thoth.Json
open Fetch
open Fable.Core
open Fable.Core.JsInterop
open Thoth.Json

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

[<ReactComponent>]
let Cart (cart: CartModel) =
    let modelToDomain model =
        (cart.cart |> Cart.ToDomain, cart.products |> Array.map Product.ToDomain)

    let (cart, products), setCartProducts = React.useState (modelToDomain cart)

    let loginState, setLoginState =
        React.useState Deferred.HasNotStartedYet

    let id, setId = React.useState cart.CartId

    let login () = getCartById %id

    let startLogin =
        React.useDeferredCallback (login, setLoginState)

    let y =
        match loginState with
        | Deferred.HasNotStartedYet -> Html.p "HasNotStartedYet"
        | Deferred.InProgress -> Html.i "InProgress"
        | Deferred.Failed error -> Html.div error.Message
        | Deferred.Resolved content ->
            //setCartProducts (modelToDomain content)
            Html.div $"Resolved%A{content}"

    JS.console.log cart

    JS.console.log products

    let c = sprintf "%A" cart
    let p = sprintf "%A" products

    Html.div [ y
               Html.input [ prop.onChange (fun (text: string) -> setId %(int text)) ]
               Html.button [ prop.onClick (fun t -> startLogin ()) ]
               Html.p c
               Html.p p
               for i in products do
                   Html.p i.Description ]
