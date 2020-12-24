namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open System.Linq
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data
open MyStore.WebApi.Utils
open Microsoft.EntityFrameworkCore

[<ApiController>]
[<Route("[controller]")>]
type CartsController(logger: ILogger<CartsController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncT1 ActionResult<Cart>
        <| async {
            if (query {
                    for i in context.Carts do
                        exists (i.CartId = id)
                }) then
                return
                    this.Ok
                        (query {
                            for i in context.Carts do
                                where (i.CartId = id)
                                exactlyOne
                         }) :> _
            else
                return this.NotFound() :> _
           }


    [<HttpGet("{id}/products")>]
    member this.GetCartProducts(id) =
        ActionResult.ofAsyncT1 ActionResult<IEnumerable<Product>>
        <| async {
            if (query {
                    for i in context.Carts do
                        exists (i.CartId = id)
                }) then
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
        ActionResult.ofAsyncT1 ActionResult<unit>
        <| async {
            if (query {
                    for i in context.Carts do
                        exists (i.CartId = id)
                }
                && query {
                    for i in context.Customers do
                        exists (i.CustomerId = ownerId)
                   }) then
                let cart =
                    query {
                        for i in context.Carts do
                            where (i.CartId = id)
                            select i
                            exactlyOne
                    }

                cart.OwnerCustomerId <- ownerId

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.NoContent() :> _
            else
                return this.NotFound() :> _
           }

    [<HttpPost>]
    member this.Create([<FromBody>] cart: Cart) =
        ActionResult.ofAsyncT1 ActionResult<Cart>
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
        ActionResult.ofAsyncT1 ActionResult<unit>
        <| async {
            if (query {
                    for i in context.Carts do
                        exists (i.CartId = id)
                }
                && query {
                    for j in context.Products do
                        exists (j.ProductId = productId)
                   }) then
                let cart =
                    query {
                        for i in context.Carts do
                            where (i.CartId = id)
                            exactlyOne
                    }

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
