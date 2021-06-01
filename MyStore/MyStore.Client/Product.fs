module MyStore.Client.Product

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
    | Fetch of ProductId
    | Fetched of ProductModel * string
    | Failed of exn
    | ToCart

type State = { Product: Product; IsInCart: bool }

let init productModel () =
    { Product = productModel.product |> Product.ToDomain
      IsInCart = productModel.isInCart },
    Cmd.none

let getProductById (id: int) =
    promise {
        let url = $"/Shop/Product/%i{id}"

        let! model = Fetch.get (baseUrl () + url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)
        return Fetched(model, url)
    }

let update msg state =
    match msg with
    | Fetch productId -> state, Cmd.OfPromise.either getProductById %productId id Failed
    | Fetched (productModel, url) ->
        { Product = productModel.product |> Product.ToDomain
          IsInCart = productModel.isInCart },
        Cmd.navigatePath url
    | Failed exn ->
        JS.console.error exn
        state, Cmd.none
    | ToCart -> state, Cmd.none

[<ReactComponent>]
let Product (productModel: ProductModel) =
    let state, dispatch =
        React.useElmish (init productModel, update, [||])

    let inputState, setInputState =
        React.useState productModel.product.productId

    Html.div [ Html.input [ prop.onChange (fun a -> setInputState (int a)) ]
               Html.button [ prop.text "GetById"
                             prop.onClick (fun _ -> dispatch (Fetch %inputState)) ]
               Html.button [ prop.text (
                                 if state.IsInCart then
                                     "Remove from cart"
                                 else
                                     "Add to cart"
                             )
                             prop.onClick (fun _ -> dispatch ToCart) ]
               Html.p $"%A{state}" ]
