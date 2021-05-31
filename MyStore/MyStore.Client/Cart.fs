module MyStore.Client.Cart

open Fable.Core
open Feliz

open MyStore.Dto.Shop

[<ReactComponent>]
let Cart (props: {| cart: CartModel |}) =
    JS.console.log props
    JS.console.log props.cart

    let s = sprintf "%A" props

    Html.div [ Html.p s
               for i in props.cart.products do
                   Html.p i.name ]
