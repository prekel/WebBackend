namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Repository
open MyStore.WebApi.Utils

[<ApiController>]
[<Route("[controller]")>]
type ProductsController(logger: ILogger<ProductsController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Product>
        <| async {
            match Products.exists context id with
            | true -> return this.Ok(Products.exactlyOne context id) :> _
            | false -> return this.NotFound() :> _
           }


    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Product>>
        <| async {
            return
                this.Ok
                    (nullableLimitStartToSkipTake (start, limit)
                     |> Products.skipTake context) :> _
           }

    [<HttpPost>]
    member this.Create([<FromBody>] product) =
        ActionResult.ofAsyncTA ActionResult<Product>
        <| async {
            do! context.Products.AddAsync(product).AsTask()
                |> Async.AwaitTask
                |> Async.Ignore

            do! context.SaveChangesAsync()
                |> Async.AwaitTask
                |> Async.Ignore


            return this.Created($"products/{product.ProductId}", product) :> _
           }

    [<HttpPut("{id}")>]
    member this.Update(id, [<FromBody>] product: Product) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if Products.exists context id then
                product.ProductId <- id

                context.Products.Update(product) |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpDelete("{id}")>]
    member this.DeleteById(id) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if Products.exists context id then
                context.Products.Remove(Products.exactlyOne context id)
                |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }
