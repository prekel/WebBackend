namespace MyStore.WebApi.Controllers

open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.WebApi.Repository
open MyStore.WebApi.Utils

[<ApiController>]
[<Route("[controller]")>]
type CartsController(logger: ILogger<CartsController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet("{id}")>]
    member this.GetById(id) =
        ActionResult.ofAsyncTA ActionResult<Cart>
        <| async {
            if Carts.exists context id
            then return this.Ok(Carts.exactlyOne context id) :> _
            else return this.NotFound() :> _
           }


    [<HttpGet("{id}/products")>]
    member this.GetCartProducts(id) =
        ActionResult.ofAsyncTA ActionResult<IEnumerable<Product>>
        <| async {
            match Carts.exists context id with
            | true ->
                return
                    this.Ok
                        ((Carts.exactlyOneIncludeProducts context id)
                            .Products) :> _
            | false -> return this.NotFound() :> _
           }


    [<HttpPut("{id}/owner/{ownerId}")>]
    member this.SetOwner(id, [<FromQuery>] ownerId) =
        ActionResult.ofAsyncTA ActionResult<unit>
        <| async {
            if (Carts.exists context id
                && Customers.exists context ownerId) then
                let cart = Carts.exactlyOne context id

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
            if Carts.exists context id then
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
            if (Carts.exists context id
                && Products.exists context productId) then
                let cart = Carts.exactlyOne context id

                let product = Products.exactlyOne context productId

                cart.Products.Add(product)

                do! context.SaveChangesAsync()
                    |> Async.AwaitTask
                    |> Async.Ignore

                return this.Ok() :> _
            else
                return this.NotFound() :> _
           }
