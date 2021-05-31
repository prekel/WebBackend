module MyStore.Client.Cart

open Feliz

open MyStore.Dto.Shop

[<ReactComponent>]
let Cart (cart: CartModel) =
    Html.div [ for i in cart.Products do
                   Html.li i.Name ]
