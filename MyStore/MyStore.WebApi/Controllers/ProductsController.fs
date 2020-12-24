namespace MyStore.WebApi.Controllers

open System
open System.Collections.Generic
open Microsoft.AspNetCore.Mvc
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open MyStore.Data
open MyStore.Data.Entity
open MyStore.Data.Entity
open MyStore.WebApi.Utils
open Microsoft.EntityFrameworkCore
open System.Linq
open Microsoft.AspNetCore.Mvc.Infrastructure


[<ApiController>]
[<Route("[controller]")>]
type ProductsController(logger: ILogger<CustomersController>, context: Context) =
    inherit ControllerBase()

    let get dbSet (pred: 'T -> int -> bool) id funNotFound funFound =
        ActionResult.ofAsyncT1 ActionResult<Product>
        <| async {
            if (query {
                    for i in dbSet do
                        exists (pred i id)
                }) then
                return
                    funFound
                        (query {
                            for i in dbSet do
                                where (pred i id)
                                exactlyOne
                         }) :> _
            else
                return funNotFound () :> _
           }

    [<HttpGet("{id}")>]
    member this.GetById1(id) =
        get context.Products (fun (i: Product) id -> i.ProductId = id) id this.NotFound this.Ok

//    [<HttpGet("{id}")>]
//    member this.GetById(id) =
//        ActionResult.ofAsyncT1 ActionResult<Product>
//        <| async {
//            if (query {
//                    for i in context.Products do
//                        exists (i.ProductId = id)
//                }) then
//                return
//                    this.Ok
//                        (query {
//                            for i in context.Products do
//                                where (i.ProductId = id)
//                                exactlyOne
//                         }) :> _
//            else
//                return this.NotFound() :> _
//           }
