module MyStore.Web.Handlers.Operator

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

let operator : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! operatorE =
                if user.OperatorId.HasValue then
                    query {
                        for i in db.SupportOperators do
                            where (i.SupportOperatorId = user.OperatorId.Value)
                            select i
                    }
                    |> fun qr -> qr.SingleAsync()
                else
                    task {
                        let operator =
                            Support.Operator(
                                FirstName = $"FirstName for %s{user.UserName}",
                                LastName = $"LastName for %s{user.UserName}",
                                Email = user.Email
                            )

                        let! _ = db.SupportOperators.AddAsync operator
                        user.Operator <- operator
                        let! _ = db.SaveChangesAsync()
                        return operator
                    }

            let operatorDto = operatorE.ToDto()

            return! razorHtmlView "Operator/Index" (Some operatorDto) None None next ctx
        }
