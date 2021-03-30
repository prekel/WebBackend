namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Utils
open MyStore.WebApi.Repository

[<ApiController>]
[<Route("[controller]")>]
type CustomersController(logger: ILogger<CustomersController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Customer>>
        <| async {
            return
                this.Ok
                    (nullableLimitStartToSkipTake (start, limit)
                     |> Customers.skipTake context) :> _
           }


    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Customer>
        <| async {
            if (Customers.exists context id)
            then return this.Ok(Customers.exactlyOne context id) :> _
            else return this.NotFound() :> _
           }

    [<HttpDelete("{id}")>]
    member this.DeleteById(id) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if Customers.exists context id then
                context.Customers.Remove(Customers.exactlyOne context id)
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
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if Customers.exists context id then
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
        ActionResult.ofAsyncTA ActionResult<Customer>
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
