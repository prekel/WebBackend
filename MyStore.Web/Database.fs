module MyStore.Web.Database

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

let cartStuff (db: Context) (user: ApplicationUser) cartId =
    task {
        let! cartE =
            query {
                for i in db.Carts.Include(fun w -> w.Products) do
                    where (cartId = i.CartId)
            }
            |> fun qr -> qr.SingleAsync()

        let cartDto = cartE.ToDto()
        let cart = Cart.ToDomain cartDto

        let! _, _, userCustomer = customerStuff db user

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
