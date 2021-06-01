module MyStore.Client.Cart

open Fable.Core
open Feliz

open MyStore.Dto.Shop
open MyStore.Domain.Shop

[<ReactComponent>]
let Cart (cart: CartModel) =
    let cart, products =
        cart.cart |> Cart.ToDomain, cart.products |> Array.map Product.ToDomain

    JS.console.log cart
    JS.console.log products

    let c = sprintf "%A" cart
    let p = sprintf "%A" products

    Html.div [ Html.p c
               Html.p p
               for i in products do
                   Html.p i.Description ]
