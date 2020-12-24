namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Utils

[<ApiController>]
[<Route("[controller]")>]
type ProductsController(logger: ILogger<ProductsController>, context: Context) =
    inherit ControllerBase()

    let exists' id =
        query {
            for i in context.Products do
                exists (i.ProductId = id)
        }

    let exactlyOne' id =
        query {
            for i in context.Products do
                where (i.ProductId = id)
                exactlyOne
        }

    let skipTake (nskip, ntake) =
        query {
            for i in context.Products do
                sortBy i.ProductId
                select i
                skip nskip
                take ntake
        }

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Product>
        <| async {
            match exists' id with
            | true -> return this.Ok(exactlyOne' id) :> _
            | false -> return this.NotFound() :> _
           }


    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Customer>>
        <| async {
            return
                this.Ok
                    (nullableLimitStartToSkipTake (start, limit)
                     |> skipTake) :> _
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
            if exists' id then
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
            if exists' id then
                context.Products.Remove(exactlyOne' id) |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }
