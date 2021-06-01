module MyStore.Web.Handlers.Cart

open System
open System.Net.Mime
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
open Microsoft.AspNetCore.Identity

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open Giraffe.EndpointRouting
open Giraffe.Razor

open MyStore.Data
open MyStore.Data.Identity
open MyStore.Web.Models
open MyStore.Dto.Shop
open MyStore.Domain.Shop

let razorOrJson viewName model viewData modelState : HttpHandler =
    fun next ctx ->
        if ctx.Request.GetTypedHeaders().Accept
           |> Seq.exists (fun h -> h.ToString() = MediaTypeNames.Application.Json) then
            match model with
            | Some model -> json model next ctx
            | None -> json null next ctx
        else
            razorHtmlView viewName model viewData modelState next ctx

let cartHandler (id: int) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let cartE =
                query {
                    for i in db.Carts.Include(fun w -> w.Products) do
                        where (id = i.CartId)
                        head
                }

            let cartDto = cartE.ToDto()
            let cart = Cart.ToDomain cartDto

            let isAccessed =
                match cart.OwnerCustomerId, cart.IsPublic with
                | Some customerId, false ->
                    let customerId = %customerId

                    let ownerUserId =
                        query {
                            for i in db.Customers do
                                where (i.CustomerId = customerId)
                                select i.UserId
                                head
                        }

                    user.Id = ownerUserId
                | _, isPublic -> isPublic

            if isAccessed then
                let productsDto =
                    cartE.Products
                    |> Seq.map (fun p -> p.ToDto())
                    |> Seq.toArray

                let model =
                    { CartModel.cart = cartDto
                      products = productsDto }

                return! razorOrJson "Shop/Cart" (Some model) None None next ctx
            else
                return! RequestErrors.FORBIDDEN "Forbidden" next ctx

        }
