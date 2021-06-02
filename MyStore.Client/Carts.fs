module MyStore.Client.Carts

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
    | SetPublic of bool
    | Fetched of CartsModel * string
    | Failed of exn

type State =
    { Carts: Cart array
      Page: int
      IsPublic: bool }

let init cartsModel () =
    { Carts = cartsModel.carts |> Array.map Cart.ToDomain
      Page = cartsModel.query.offset |> offsetToPage
      IsPublic = cartsModel.query.isPublic },
    Cmd.none

let getCarts (isPublic, page) =
    promise {
        let url =
            $"/Shop/Cart?isPublic=%b{isPublic}&count=%i{itemsPerPage}&offset=%i{pageToOffset page}"

        let! model = Fetch.get (baseUrl () + url, caseStrategy = CamelCase, extra = extra, headers = acceptJson)
        return Fetched(model, url)
    }

let update msg state =
    match msg with
    | SetPage page -> { state with Page = page }, Cmd.OfPromise.either getCarts (state.IsPublic, page) id Failed
    | SetPublic isPublic ->
        { state with IsPublic = isPublic }, Cmd.OfPromise.either getCarts (isPublic, state.Page) id Failed
    | Fetched (cartsModel, url) ->
        { state with
              Carts = cartsModel.carts |> Array.map Cart.ToDomain },
        Cmd.navigatePath url
    | Failed exn ->
        JS.console.error exn
        state, Cmd.none

[<ReactComponent>]
let Carts (carts: CartsModel) =
    let state, dispatch =
        React.useElmish (init carts, update, [||])

    Html.div [ Html.a [ prop.children [ Html.p "Current cart" ]
                        prop.href "/Shop/Cart/Current" ]
               Html.input [ prop.isChecked state.IsPublic
                            prop.type' "checkbox"
                            prop.onCheckedChange (fun a -> SetPublic a |> dispatch) ]
               Html.button [ prop.text "+"
                             prop.onClick (fun _ -> SetPage(state.Page + 1) |> dispatch) ]
               Html.p $"Page: %i{state.Page}"
               Html.button [ prop.text "-"
                             prop.onClick (fun _ -> SetPage(state.Page - 1) |> dispatch) ]
               for i in state.Carts do
                   Html.a [ prop.children [ Html.p $"%A{i}" ]
                            prop.href $"/Shop/Cart/%i{%i.CartId}" ] ]
