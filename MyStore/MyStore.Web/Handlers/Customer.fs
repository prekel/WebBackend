module MyStore.Web.Handlers.Customer

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

let customer : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! customerE =
                if user.CustomerId.HasValue then
                    query {
                        for i in db.Customers do
                            where (i.CustomerId = user.CustomerId.Value)
                            select i
                    }
                    |> fun qr -> qr.SingleAsync()
                else
                    task {
                        let customer =
                            Shop.Customer(FirstName = $"FirstName for %s{user.UserName}", Email = user.Email)

                        let! _ = db.Customers.AddAsync customer
                        user.Customer <- customer
                        let! _ = db.SaveChangesAsync()
                        return customer
                    }

            let customerDto = customerE.ToDto()

            return! razorHtmlView "Customer/Index" (Some customerDto) None None next ctx
        }
