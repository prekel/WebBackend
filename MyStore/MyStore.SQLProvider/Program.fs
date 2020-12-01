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

    let employeesFirstName =
        query {
            for emp in ctx.Public.Customers do
                select emp.FirstName
        }
        |> Seq.head

    printfn "Hello %s!" employeesFirstName
    0
