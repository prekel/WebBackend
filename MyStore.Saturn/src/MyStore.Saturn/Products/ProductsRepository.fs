module MyStore.Saturn.Products.Database

open System.Threading.Tasks
open FSharp.Control.Tasks.ContextInsensitive
open Npgsql

open MyStore.Saturn.Database

let getAll connectionString (limit: int): Task<Result<ProductDto seq, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        return!
            query
                connection
                """SELECT "ProductId", "Name", "Description", "Price", "Rating" FROM "Products" ORDER BY "ProductId" LIMIT @Limit"""
                (Some <| dict [ "Limit" => limit ])
    }

let getById connectionString (id: int): Task<Result<ProductDto option, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        return!
            querySingle
                connection
                """SELECT "ProductId", "Name", "Description", "Price", "Rating" FROM "Products" WHERE "ProductId"=@ProductId """
                (Some <| dict [ "ProductId" => id ])
    }

let update connectionString (v: ProductDto): Task<Result<int, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        return!
            execute
                connection
                """UPDATE "Products" SET "ProductId" = @ProductId, "Name" = @Name, "Description" = @Description, "Price" = @Price, "Rating" = @Rating WHERE "ProductId"=@ProductId"""
                v
    }

let insert connectionString v: Task<Result<int, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        return!
            execute
                connection
                """INSERT INTO "Products"("ProductId", "Name", "Description", "Price", "Rating") VALUES (@ProductId, @Name, @Description, @Price, @Rating)"""
                v
    }

let delete connectionString id: Task<Result<int, exn>> =
    task {
        use connection = new NpgsqlConnection(connectionString)

        return!
            execute connection """DELETE FROM "Products" WHERE "ProductId"=@ProductId""" (dict [ "ProductId" => id ])
    }
