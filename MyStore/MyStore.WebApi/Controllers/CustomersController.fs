namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Utils
open Microsoft.EntityFrameworkCore
open System.Linq
open Microsoft.AspNetCore.Mvc.Infrastructure


[<ApiController>]
[<Route("[controller]")>]
type CustomersController(logger: ILogger<CustomersController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        ActionResult.ofAsync
        <| async {
            let nskip, ntake =
                nullableLimitStartToSkipTake (start, limit)

            return
                this.Ok
                    (query {
                        for i in context.Customers do
                            sortBy i.CustomerId
                            select i
                            skip nskip
                            take ntake
                     }) :> _
           }


    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsync
        <| async {
            if (query {
                    for i in context.Customers do
                        exists (i.CustomerId = id)
                }) then
                return
                    this.Ok
                        (query {
                            for i in context.Customers do
                                where (i.CustomerId = id)
                                exactlyOne
                         }) :> _
            else
                return this.NotFound() :> _
           }

    [<HttpDelete("{id}")>]
    member this.DeleteById(id) =
        ActionResult.ofAsync
        <| async {
            if (query {
                    for i in context.Customers do
                        exists (i.CustomerId = id)
                }) then
                context.Customers.Remove
                    (query {
                        for i in context.Customers do
                            where (i.CustomerId = id)
                            exactlyOne
                     })
                |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }


    [<HttpPut("{id}")>]
    member this.Update(id, [<FromBody>] customer: Customer) =
        ActionResult.ofAsync
        <| async {
            if (query {
                    for i in context.Customers do
                        exists (i.CustomerId = id)
                }) then
                customer.CustomerId <- id

                context.Customers.Update(customer) |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpPost>]
    member this.Add([<FromBody>] customer: Customer, [<FromQuery>] password) =
        ActionResult.ofAsync
        <| async {
            customer.PasswordSalt <- Crypto.GenerateSaltForPassword()
            customer.PasswordHash <- Crypto.ComputePasswordHash(password, customer.PasswordSalt)

            do! context.Customers.AddAsync(customer).AsTask()
                |> Async.AwaitTask
                |> Async.Ignore

            do! context.SaveChangesAsync()
                |> Async.AwaitTask
                |> Async.Ignore
 

            return this.Created($"customers/{customer.CustomerId}", customer) :> _
           }
