namespace Products

open FSharp.Data.Sql
open Npgsql
open Sql
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Database

module Database =
    let getAll2 (ctx: Sql.dataContext) =
        task {
            let b =
                query {
                    for i in ctx.Public.Products do
                        select
                            { ProductId = i.ProductId
                              Name = i.Name
                              Description = i.Description
                              Price = i.Price }

                        take 10
                }

            return! b |> Seq.executeQueryAsync |> Async.StartAsTask
        }

    let getAll (ctx: Sql.dataContext): Task<Result<Product seq, exn>> =
        task {
            let! ret = getAll2 ctx
            return Ok ret
        //
//            use connection = new NpgsqlConnection(connectionString)
//
//            return!
//                query1 connection """SELECT "ProductId", "Name", "Description", "Price" FROM "Products" LIMIT 50""" None
        }

    let getById (ctx: Sql.dataContext) (id: int): Task<Result<Product option, exn>> =
        task {
            let b =
                query {
                    for i in ctx.Public.Products do
                        where (i.ProductId = id)

                        select
                            { ProductId = i.ProductId
                              Name = i.Name
                              Description = i.Description
                              Price = i.Price }

                }

            let! g = b |> Seq.executeQueryAsync |> Async.StartAsTask
            return g |> Seq.tryHead |> Ok

        }

    //            use connection = new NpgsqlConnection(connectionString)
//
//            return!
//                querySingle
//                    connection
//                    """SELECT "ProductId", "Name", "Description", "Price" FROM "Products" WHERE "ProductId"=@ProductId """
//                    (Some <| dict [ "ProductId" => id ])

    let update (ctx: Sql.dataContext) v: Task<Result<int, exn>> =
        task {
            try
                let b =
                    query {
                        for i in ctx.Public.Products do
                            where (i.ProductId = v.ProductId)
                    }

                let! head = b |> Seq.headAsync |> Async.StartAsTask
                head.Name <- v.Name
                head.Description <- v.Description
                head.Price <- v.Price
                do! ctx.SubmitUpdatesAsync() |> Async.StartAsTask

                return Ok 1
            with ex -> return Error ex
        } //            use connection = new NpgsqlConnection(connectionString)
    //
    //            return!
    //                execute
    //                    connection
    //                    """UPDATE "Products" SET "ProductId" = @ProductId, "Name" = @Name, "Description" = @Description, "Price" = @Price WHERE "ProductId"=@ProductId"""
    //                    v

    let insert (ctx: Sql.dataContext) v: Task<Result<int, exn>> =
        task {
            try
                ctx.Public.Products.``Create(Description, Name, Price)`` (v.Description, v.Name, v.Price)
                |> ignore

                do! ctx.SubmitUpdatesAsync() |> Async.StartAsTask
                return Ok 1
            with ex -> return Error ex
        }

    let delete (ctx: Sql.dataContext) id: Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection """DELETE FROM "Products" WHERE "ProductId"=@ProductId""" (dict [ "ProductId" => id ])
        }
//        task {
//            try
//                let g =
//                    query {
//                        for i in ctx.Public.Products do
//                            where (i.ProductId = id)
//                    }
//                do! g |> Seq.``delete all items from single table`` |> Async.Ignore |> Async.StartAsTask
//
////                let! h = g |> Seq.headAsync |> Async.StartAsTask
////                h.Delete()
////                do! ctx.SubmitUpdatesAsync() |> Async.StartAsTask
//                return Ok 1
//            with ex -> return Error ex
//        }
