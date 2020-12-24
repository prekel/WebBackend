namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data.Entity
open MyStore.WebApi.Repository
open MyStore.WebApi.Utils

[<ApiController>]
[<Route("[controller]")>]
type OrdersController(logger: ILogger<OrdersController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Order>
        <| async {
            match Orders.exists context id with
            | true -> return this.Ok(Orders.exactlyOne context id) :> _
            | false -> return this.NotFound() :> _
           }


    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Order>>
        <| async {
            return
                this.Ok
                    (nullableLimitStartToSkipTake (start, limit)
                     |> Orders.skipTake context) :> _
           }

    [<HttpPost>]
    member this.Create([<FromBody>] order) =
        ActionResult.ofAsyncTA ActionResult<Order>
        <| async {
            do! context.Orders.AddAsync(order).AsTask()
                |> Async.AwaitTask
                |> Async.Ignore

            do! context.SaveChangesAsync()
                |> Async.AwaitTask
                |> Async.Ignore


            return this.Created($"orders/{order.OrderId}", order) :> _
           }

    [<HttpPut("{id}")>]
    member this.Update(id, [<FromBody>] order: Order) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if Orders.exists context id then
                order.OrderId <- id

                context.Orders.Update(order) |> ignore

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
            if Orders.exists context id then
                context.Orders.Remove(Orders.exactlyOne context id)
                |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpGet("{id}/orderedProducts")>]
    member this.GetOrderedProducts(id) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<OrderedProduct>>
        <| async {
            match Orders.exists context id with
            | true ->
                return
                    this.Ok
                        ((Orders.exactlyOneIncludeOrderedProducts context id)
                            .OrderedProducts) :> _
            | false -> return this.NotFound() :> _
           }


    [<HttpPost("{id}/orderedProducts/{productId}")>]
    member this.AddProduct(id, productId) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if (Orders.exists context id
                && Products.exists context productId) then

                let order = Orders.exactlyOne context id
                let product = Products.exactlyOne context productId

                let orderedProduct =
                    OrderedProduct(ProductId = product.ProductId, OrderId = order.OrderId, OrderedPrice = product.Price)

                do! context
                        .OrderedProducts
                        .AddAsync(orderedProduct)
                        .AsTask()
                    |> Async.AwaitTask
                    |> Async.Ignore

                order.OrderedProducts.Add(orderedProduct)

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.Created($"{id}/orderedProducts/{productId}", orderedProduct) :> _
            else
                return this.NotFound() :> _
           }

    [<HttpGet("{id}/orderedProducts/{productId}")>]
    member this.GetProduct(id, productId) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            match OrderedProducts.exists context id productId with
            | true -> return this.Ok(OrderedProducts.exactlyOne context id productId) :> _
            | false -> return this.NotFound() :> _
           }
