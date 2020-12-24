namespace MyStore.WebApi.Controllers

open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Utils
open Microsoft.EntityFrameworkCore

[<ApiController>]
[<Route("[controller]")>]
type CartsController(logger: ILogger<CartsController>, context: Context) =
    inherit ControllerBase()

    let exists' id =
        query {
            for i in context.Carts do
                exists (i.CartId = id)
        }

    let exactlyOne' id =
        query {
            for i in context.Carts do
                where (i.CartId = id)
                exactlyOne
        }

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Cart>
        <| async { if exists' id then return this.Ok(exactlyOne' id) :> _ else return this.NotFound() :> _ }


    [<HttpGet("{id}/products")>]
    member this.GetCartProducts(id) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Product>>
        <| async {
            if exists' id then
                let products =
                    (query {
                        for i in context.Carts.Include(fun j -> j.Products) do
                            where (i.CartId = id)
                            exactlyOne
                     })
                        .Products

                return this.Ok(products) :> _
            else
                return this.NotFound() :> _
           }


    [<HttpPut("{id}/owner/{ownerId}")>]
    member this.SetOwner(id, [<FromQuery>] ownerId) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if (exists' id
                && query {
                    for i in context.Customers do
                        exists (i.CustomerId = ownerId)
                   }) then
                let cart = exactlyOne' id

                cart.OwnerCustomerId <- ownerId

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpPut("{id}")>]
    member this.Update(id, [<FromBody>] cart: Cart) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if exists' id then
                cart.CartId <- id

                context.Carts.Update(cart) |> ignore

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpPost>]
    member this.Create([<FromBody>] cart: Cart) =
        ActionResult.ofAsyncTA ActionResult<Cart>
        <| async {
            do! context.Carts.AddAsync(cart).AsTask()
                |> Async.AwaitTask
                |> Async.Ignore

            do! context.SaveChangesAsync()
                |> Async.AwaitTask
                |> Async.Ignore

            return this.Created($"carts/{cart.CartId}", cart) :> _
           }

    [<HttpPut("{id}/products/{productId}")>]
    member this.AddProduct(id, productId) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if (exists' id
                && query {
                    for j in context.Products do
                        exists (j.ProductId = productId)
                   }) then
                let cart = exactlyOne' id

                let product =
                    query {
                        for j in context.Products do
                            where (j.ProductId = id)
                            exactlyOne
                    }

                cart.Products.Add(product)

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.Ok() :> _
            else
                return this.NotFound() :> _
           }
