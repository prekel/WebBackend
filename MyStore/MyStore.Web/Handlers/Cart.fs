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
open MyStore.Web.Core

let customerStuff (db: Context) (user: ApplicationUser) =
    task {
        let! userCustomerE =
            query {
                for i in db.Customers do
                    where (i.CustomerId = user.CustomerId.Value)
                    select i
            }
            |> fun qr -> qr.SingleAsync()

        let userCustomerDto = userCustomerE.ToDto()
        let userCustomer = Customer.ToDomain userCustomerDto
        return userCustomerE, userCustomerDto, userCustomer
    }

let private cartStuff (db: Context) (user: ApplicationUser) cartId =
    task {
        let! cartE =
            query {
                for i in db.Carts.Include(fun w -> w.Products) do
                    where (cartId = i.CartId)
            }
            |> fun qr -> qr.SingleAsync()

        let cartDto = cartE.ToDto()
        let cart = Cart.ToDomain cartDto

        let! userCustomerE, userCustomerDto, userCustomer = customerStuff db user

        let isAccessed =
            match cart.OwnerCustomerId with
            | Some customerId -> customerId = userCustomer.CustomerId
            | _ -> cart.IsPublic

        let isCurrent =
            userCustomer.CurrentCartId
            |> Option.map ((=) cart.CartId)
            |> Option.defaultValue false

        return cartE, cartDto, cart, isAccessed, isCurrent, userCustomer
    }

let cartById (cartId: int) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! cartE, cartDto, cart, isAccessed, isCurrent, _ = cartStuff db user cartId

            if isAccessed then
                let productsDto =
                    cartE.Products
                    |> Seq.map (fun p -> p.ToDto())
                    |> Seq.toArray

                let model =
                    { CartModel.cart = cartDto
                      products = productsDto
                      isCurrent = isCurrent }

                return! razorOrJson "Cart/Cart" (Some model) None None next ctx
            else
                return! RequestErrors.FORBIDDEN "Forbidden" next ctx

        }

[<CLIMutable>]
type CartsQueryOption =
    { isPublic: bool option
      count: int option
      offset: int option }

let carts : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let q = ctx.BindQueryString<CartsQueryOption>()

            let q =
                { CartsQuery.isPublic = q.isPublic |> Option.defaultValue false
                  count = q.count |> Option.defaultValue 15
                  offset = q.offset |> Option.defaultValue 0 }

            let! userCustomerE =
                query {
                    for i in db.Customers do
                        where (i.CustomerId = user.CustomerId.Value)
                        select i
                }
                |> fun qr -> qr.SingleAsync()


            let! cartsE =
                if q.isPublic then
                    query {
                        for i in db.Carts do
                            where (i.IsPublic = true)
                            skip q.offset
                            take q.count
                            select i
                    }
                    |> fun a -> a.ToArrayAsync()
                else
                    query {
                        for i in db.Carts do
                            where (i.OwnerCustomerId.Value = userCustomerE.CustomerId)
                            skip q.offset
                            take q.count
                            select i
                    }
                    |> fun a -> a.ToArrayAsync()

            let cartsDto = cartsE |> Array.map (fun c -> c.ToDto())

            let model =
                { CartsModel.carts = cartsDto
                  query = q }

            return! razorOrJson "Cart/Index" (Some model) None None next ctx
        }

[<CLIMutable>]
type SetCurrentCartQueryOption = { setCurrent: bool option }

let setCurrentCart cartId : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let q =
                ctx.BindQueryString<SetCurrentCartQueryOption>()

            let! _, _, _, isAccessed, isCurrent, userCustomer = cartStuff db user cartId

            let q =
                { SetCurrentCartQuery.setCurrent =
                      q.setCurrent
                      |> Option.defaultValue (not isCurrent) }


            if isAccessed then
                let userCustomerId = %userCustomer.CustomerId

                let! customer =
                    query {
                        for i in db.Customers do
                            where (i.CustomerId = userCustomerId)
                            select i
                    }
                    |> fun qr -> qr.SingleAsync()

                if q.setCurrent then
                    customer.CurrentCartId <- cartId
                else
                    customer.CurrentCartId <- Nullable()

                let! _ = db.SaveChangesAsync()

                return! json q next ctx
            else
                return! RequestErrors.FORBIDDEN "Forbidden" next ctx
        }

let currentCart : HttpHandler =
    fun next ctx ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! userCustomerE, _, _ = customerStuff db user

            match userCustomerE.CurrentCartId |> Option.ofNullable with
            | Some cartId -> return! redirectTo false $"/Shop/Cart/%i{cartId}" next ctx
            | None ->
                let cart =
                    Shop.Cart(IsPublic = false, OwnerCustomerId = userCustomerE.CustomerId)

                let! _ = db.Carts.AddAsync(cart)
                userCustomerE.CurrentCart <- cart
                let! _ = db.SaveChangesAsync()
                return! redirectTo false $"/Shop/Cart/%i{cart.CartId}" next ctx
        }
