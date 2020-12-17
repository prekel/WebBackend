namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data
open MyStore.WebApi.Utils

[<ApiController>]
[<Route("[controller]")>]
type CartsController(logger: ILogger<CartsController>, context: Context) =
    inherit ControllerBase()

    [<HttpGet>]
    member this.GetOffset([<FromQuery>] start: Nullable<int>, [<FromQuery>] limit: Nullable<int>) =
        let nskip, ntake =
            nullableLimitStartToSkipTake (start, limit)

        ActionResult<IEnumerable<Cart>>
            (query {
                for i in context.Carts do
                    sortBy i.CartId
                    select i
                    skip nskip
                    take ntake
             })

    [<HttpPut("owner/{id}")>]
    member this.SetOwner(id, [<FromQuery>] ownerId) =
        if (query {
                for i in context.Carts do
                    exists (i.CartId = id)
            }) then
            let cart =
                query {
                    for i in context.Carts do
                        where (i.CartId = id)
                        select i
                        exactlyOne
                }

            cart.OwnerCustomerId <- ownerId
            context.SaveChanges() |> ignore
            ActionResult<Customer>(base.NoContent())
        else
            ActionResult<Customer>(base.NotFound())
