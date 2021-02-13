namespace Products

open Database
open Npgsql
open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive

module Database =
    let getAll connectionString: Task<Result<Product seq, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! query connection """SELECT "ProductId", "Name", "Description", "Price" FROM "Products" LIMIT 50""" None
        }

    let getById connectionString (id: int): Task<Result<Product option, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                querySingle
                    connection
                    """SELECT "ProductId", "Name", "Description", "Price" FROM "Products" WHERE "ProductId"=@ProductId """ (Some <| dict [ "ProductId" => id ])
        }

    let update connectionString v: Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    """UPDATE "Products" SET "ProductId" = @ProductId, "Name" = @Name, "Description" = @Description, "Price" = @Price WHERE "ProductId"=@ProductId"""
                    v
        }

    let insert connectionString v: Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)

            return!
                execute
                    connection
                    """INSERT INTO "Products"("ProductId", "Name", "Description", "Price") VALUES (@ProductId, @Name, @Description, @Price)"""
                    v
        }

    let delete connectionString id: Task<Result<int, exn>> =
        task {
            use connection = new NpgsqlConnection(connectionString)
            return! execute connection """DELETE FROM "Products" WHERE "ProductId"=@ProductId""" (dict [ "id" => id ])
        }
