module MyStore.Web.Handlers.Product

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
open MyStore.Web.Core
open MyStore.Web.Database

let productById (id: int) : HttpHandler =
    fun next ctx ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()


            let! product =
                query {
                    for i in db.Products do
                        where (i.ProductId = id)
                        select i
                }
                |> fun qr -> qr.SingleAsync()

            let productDto = product.ToDto()

            let! user = userManager.GetUserAsync(ctx.User)

            let! productModel =
                if isNotNull user then
                    task {
                        let! userCustomerE, _, _ = customerStuff db user

                        let! carts =
                            query {
                                for i in db.Customers do
                                    join j in db.Carts.Include
                                                  (fun cart -> cart.Products)
                                                  on
                                                  (i.CurrentCartId.Value = j.CartId)

                                    where (i.CustomerId = userCustomerE.CustomerId)
                                    select j
                            }
                            |> fun qr -> qr.ToArrayAsync()

                        let cart = carts |> Array.tryHead

                        let isInCart =
                            match cart with
                            | Some cart ->
                                cart.Products
                                |> Seq.exists (fun p -> p.ProductId = id)
                            | None -> false

                        return
                            { ProductModel.product = productDto
                              isInCart = isInCart
                              isLoggedIn = true }
                    }
                else
                    { ProductModel.product = productDto
                      isInCart = false
                      isLoggedIn = false }
                    |> Task.FromResult

            return! razorOrJson "Product/Product" (Some productModel) None None next ctx
        }

[<CLIMutable>]
type ProductsQueryOption =
    { count: int option
      offset: int option }

let products : HttpHandler =
    fun next ctx ->
        task {
            let db = ctx.GetService<Context>()

            let q =
                ctx.BindQueryString<ProductsQueryOption>()

            let q =
                { ProductsQuery.count = q.count |> Option.defaultValue 10
                  offset = q.offset |> Option.defaultValue 0 }

            let! products =
                query {
                    for i in db.Products do
                        skip q.offset
                        take q.count
                        select i
                }
                |> fun qr -> qr.ToArrayAsync()

            let productsDto =
                products |> Array.map (fun p -> p.ToDto())

            let productsModel =
                { ProductsModel.products = productsDto
                  query = q }

            return! razorOrJson "Product/Index" (Some productsModel) None None next ctx
        }

let toggleToCart (id: int) : HttpHandler =
    fun next ctx -> task { return! json id next ctx }
