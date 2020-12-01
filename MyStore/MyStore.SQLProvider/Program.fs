open System
open FSharp.Data.Sql

type sql =
    SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL, "Host=localhost;Database=postgres;Username=postgres;Password=qwerty123">

let ctx = sql.GetDataContext()

[<EntryPoint>]
let main argv =
    printfn "Hello world %A" ctx
    0 // return an integer exit code
