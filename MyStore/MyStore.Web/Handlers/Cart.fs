module MyStore.Web.Handlers.Cart

open System
open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

open FSharp.Control.Tasks

open System.Net.Mime

open Giraffe
open Giraffe.EndpointRouting
open Giraffe.Razor
open MyStore.Data
open MyStore.Web.Models
open MyStore.Dto.Shop

let razorOrJson viewName model viewData modelState : HttpHandler =
    fun next ctx ->
        if ctx.Request.GetTypedHeaders().Accept
           |> Seq.map string
           |> Seq.exists (fun h -> h = MediaTypeNames.Application.Json) then
            json (model |> Option.defaultValue Unchecked.defaultof<_>) next ctx
        else
            razorHtmlView viewName model viewData modelState next ctx

let cartHandler (id: int) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let cartE =
                query {
                    for i in db.Carts.Include(fun w -> w.Products) do
                        where (id = i.CartId)
                        head
                }

            let cart = cartE.ToDto()

            let products =
                cartE.Products
                |> Seq.map (fun p -> p.ToDto())
                |> Seq.toArray

            let model =
                { CartModel.cart = cart
                  products = products }

            return! razorOrJson "Shop/Cart" (Some model) None None next ctx
        }
