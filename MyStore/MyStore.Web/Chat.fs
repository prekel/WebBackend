module MyStore.Web.Chat

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
open MyStore.Domain.Support
open MyStore.Web.Models
open MyStore.Dto.Shop
open MyStore.Domain.Shop
open MyStore.Web.Core
open MyStore.Web.Database
open MyStore.Dto.Support
open Fable.SignalR
open FSharp.Control.Tasks.Affine

open Microsoft.AspNetCore.Identity
open MyStore.Data
open MyStore.Data.Identity
open MyStore.Domain.Chat
open MyStore.Domain.Chat.SignalRHub

module SignalRHub =

    let update (msg: Action) (hubContext: FableHub<Action, Response>) =
        task {
            let ctx = hubContext.Context

            let db =
                hubContext.Services.GetService(typeof<Context>) :?> Context

            let userManager =
                hubContext.Services.GetService(typeof<UserManager<ApplicationUser>>) :?> UserManager<ApplicationUser>

            let! user = userManager.GetUserAsync(ctx.User)

            let ticketId = %msg.TicketId

            let! ticketE =
                query {
                    for i in db
                        .SupportTickets
                        .Include(fun t -> t.Customer)
                        .Include(fun t -> t.SupportOperator) do
                        where (i.SupportTicketId = ticketId)
                        select i
                }
                |> fun qr -> qr.FirstOrDefaultAsync()

            if isNull ticketE then
                return Response.NotFound msg.TicketId
            else
                let isAccessed =
                    match msg.Role with
                    | Customer ->
                        match user.CustomerId |> Option.ofNullable with
                        | Some customerId -> customerId = ticketE.CustomerId
                        | None -> false
                    | Operator ->
                        match user.OperatorId |> Option.ofNullable with
                        | Some userOperatorId ->
                            match ticketE.SupportOperatorId |> Option.ofNullable with
                            | Some tickerOperatorId -> tickerOperatorId = userOperatorId
                            | None -> true
                        | None -> false

                if not isAccessed then
                    return Response.Forbidden msg.TicketId
                else
                    let ticketDto = ticketE.ToDto()
                    let ticket = Ticket.ToDomain ticketDto

                    return!
                        match msg.Action with
                        | ActionType.JoinRoom ->
                            task {
                                do! hubContext.Groups.AddToGroupAsync(ctx.ConnectionId, string ticketId)

                                return Response.Joined ticket
                            }
                        | ActionType.Message text ->
                            task {
                                return!
                                    match msg.Role with
                                    | Customer ->
                                        task {
                                            let questionE =
                                                Support.Question(SupportTicketId = ticketId, Text = text)

                                            let! _ = db.SupportQuestions.AddAsync questionE
                                            let! _ = db.SaveChangesAsync()

                                            let questionDto = questionE.ToDto()
                                            let question = Question.ToDomain questionDto

                                            do!
                                                hubContext
                                                    .Clients
                                                    .Group(string ticketId)
                                                    .Send(Response.NewQuestion question)

                                            return Response.NewQuestion question
                                        }
                                    | Operator ->
                                        task {
                                            ticketE.SupportOperatorId <- user.OperatorId

                                            let answerE =
                                                Support.Answer(
                                                    SupportTicketId = ticketId,
                                                    SupportOperatorId = ticketE.SupportOperatorId.Value,
                                                    Text = text
                                                )

                                            let! _ = db.SupportAnswers.AddAsync answerE
                                            let! _ = db.SaveChangesAsync()

                                            let answerDto = answerE.ToDto()
                                            let answer = Answer.ToDomain answerDto

                                            do!
                                                hubContext
                                                    .Clients
                                                    .Group(string ticketId)
                                                    .Send(Response.NewAnswer answer)

                                            return Response.NewAnswer answer
                                        }
                            }
                        | ActionType.LeaveRoom ->
                            task {
                                do! hubContext.Groups.RemoveFromGroupAsync(ctx.ConnectionId, string ticketId)

                                return Response.LeaveDone msg.TicketId
                            }
        }

    let invoke (msg: Action) (hubContext: FableHub) =
        task { return! update msg (failwith "Invoke not used") }

    let send (msg: Action) (hubContext: FableHub<Action, Response>) =
        task {
            let! response = update msg hubContext

            return!
                match response with
                | NewAnswer _
                | NewQuestion _ -> Task.CompletedTask
                | _ -> hubContext.Clients.Caller.Send response
        }
        :> Task

    let config =
        SignalR.ConfigBuilder(Endpoints.Root, send, invoke)
            .AfterUseRouting(fun app -> app.UseAuthorization())
            .EndpointConfig(fun builder -> builder.RequireAuthorization())
            .Build()

