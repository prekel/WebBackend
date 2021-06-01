module MyStore.Client.Products

open System

open Elmish
open FSharp.UMX
open Fable.Core
open Feliz
open Thoth.Fetch
open Thoth.Json
open Feliz.UseElmish
open Feliz.Router

open MyStore.Dto.Shop
open MyStore.Domain.Shop

type Msg =
    | SetPage of int
    | Fetched of ProductsModel * string
    | Failed of exn

type State = { Products: Product array; Page: int }

let init productsModel () =
    { Products =
          productsModel.products
          |> Array.map Product.ToDomain
      Page = productsModel.query.offset |> offsetToPage },
    Cmd.none

let getProducts page =
    promise {
        let url =
            $"/Shop/Product?count=%i{itemsPerPage}&offset=%i{pageToOffset page}"

        let! model = Fetch.get (baseUrl () + url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)
        return Fetched(model, url)
    }

let update msg state =
    match msg with
    | SetPage page -> { state with Page = page }, Cmd.OfPromise.either getProducts page id Failed
    | Fetched (productsModel, url) ->
        { state with
              Products =
                  productsModel.products
                  |> Array.map Product.ToDomain },
        Cmd.navigatePath url
    | Failed exn ->
        JS.console.error exn
        state, Cmd.none

[<ReactComponent>]
let Products (productsModel: ProductsModel) =
    let state, dispatch =
        React.useElmish (init productsModel, update, [||])

    Html.div [ Html.button [ prop.text "+"
                             prop.onClick (fun _ -> SetPage(state.Page + 1) |> dispatch) ]
               Html.p $"Page: %i{state.Page}"
               Html.button [ prop.text "-"
                             prop.onClick (fun _ -> SetPage(state.Page - 1) |> dispatch) ]
               for i in state.Products do
                   Html.a [ prop.children [ Html.p $"%A{i}" ]
                            prop.href $"/Shop/Product/%i{%i.ProductId}" ] ]
