open System
open System.Text
open FSharp.Data.Sql

[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + "/libraries"

[<Literal>]
let connStr =
    "Host=localhost;Database=postgres;Username=postgres;Password=qwerty123"

type HR = SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL, connStr, ResolutionPath=resolutionPath>


[<EntryPoint>]
let main argv =
    Console.OutputEncoding <- Encoding.UTF8
    let ctx = HR.GetDataContext()

    FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent
    |> Event.add (printfn "Executing SQL: %O")

    let employeesFirstName =
        query {
            for customer in ctx.Public.Customers do
                where (customer.CustomerId = 34)
                select $"{customer.FirstName} {customer.LastName}"
        }

    let cnt =
        query {
            for customer in ctx.Public.Customers do
                where (customer.CustomerId = 34)
                count
        }

    let annas =
        query {
            for customer in ctx.Public.Customers do
                where (customer.FirstName = "Анна")
                select customer
        }

    annas
    |> Seq.toList
    |> List.map (fun customer -> printf "%s\n" $"{customer.FirstName} {customer.LastName}")
    |> ignore

    0
