﻿module MyStore.Web.Handlers.Chat

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
open MyStore.Dto.Support

let chats : HttpHandler =
    fun next ctx ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! ticketsE =
                query {
                    for i in db.SupportTickets do
                        join j in db.Users on (i.CustomerId = j.CustomerId.Value)
                        select i
                }
                |> fun qr -> qr.ToArrayAsync()

            let ticketsDto =
                ticketsE |> Array.map (fun t -> t.ToDto())

            let model = { TicketsModel.tickets = ticketsDto }

            return! razorHtmlView "Chat/Index" (Some model) None None next ctx
        }

let newChat : HttpHandler =
    fun next ctx ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! customerE, _, _ = customerStuff db user

            let ticket =
                Support.Ticket(CustomerId = customerE.CustomerId)

            let! _ = db.SupportTickets.AddAsync ticket
            let! _ = db.SaveChangesAsync()

            return! redirectTo false $"/Support/Chat/%i{ticket.SupportTicketId}" next ctx
        }


let chatPage (ticketId: int) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let db = ctx.GetService<Context>()

            let userManager =
                ctx.GetService<UserManager<ApplicationUser>>()

            let! user = userManager.GetUserAsync(ctx.User)

            let! customerE, _, _ = customerStuff db user

            let! ticketE =
                query {
                    for i in db
                        .SupportTickets
                        .Include(fun t -> t.SupportAnswers)
                        .Include(fun t -> t.SupportQuestions) do
                        where (i.SupportTicketId = ticketId)
                        select i
                }
                |> fun qr -> qr.FirstOrDefaultAsync()

            if isNull ticketE then
                return! RequestErrors.notFound (text "Not found") next ctx
            else
                let isAccessedCustomer =
                    match user.CustomerId |> Option.ofNullable with
                    | Some userCustomerId -> userCustomerId = ticketE.CustomerId
                    | None -> false

                let isAccessedOperator =
                    match user.OperatorId |> Option.ofNullable with
                    | Some userOperatorId ->
                        match ticketE.SupportOperatorId |> Option.ofNullable with
                        | Some tickerOperatorId -> tickerOperatorId = userOperatorId
                        | None -> true
                    | None -> false

                if not (isAccessedCustomer || isAccessedOperator) then
                    return! RequestErrors.forbidden (text "Forbidden") next ctx
                else
                    let questions =
                        ticketE.SupportQuestions
                        |> Seq.map (fun q -> q.ToDto())
                        |> Seq.toArray

                    let answers =
                        ticketE.SupportAnswers
                        |> Seq.map (fun q -> q.ToDto())
                        |> Seq.toArray

                    let chatModel =
                        { ChatModel.ticket = ticketE.ToDto()
                          questions = questions
                          answers = answers }

                    return! razorHtmlView "Chat/Chat" (Some chatModel) None None next ctx
        }
