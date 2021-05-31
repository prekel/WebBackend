module MyStore.Client.Cart

open Fable.Core
open Feliz

open MyStore.Dto.Shop

[<ReactComponent>]
let Cart (cart: CartModel) =
    JS.console.log cart

    let s = sprintf "%A" cart

    Html.div [ Html.p s
               for i in cart.products do
                   Html.p i.name ]
