module Server

open Saturn
open Config

let endpointPipe =
    pipeline {
        plug head
        plug requestId
    }

let app =
    application {
        pipe_through endpointPipe

        error_handler (fun ex _ -> pipeline { render_html (InternalError.layout ex) })
        use_router Router.appRouter
        url "http://0.0.0.0:8085/"
        memory_cache
        use_static "static"
        use_gzip

        use_config (fun _ ->
            { Context = Sql.Sql.GetDataContext()
              connectionString = "Host=localhost;Database=postgres;Username=postgres;Password=qwerty123" })
    }

[<EntryPoint>]
let main _ =
    FSharp.Data.Sql.Common.QueryEvents.SqlQueryEvent
    |> Event.add (printfn "Executing SQL: %O")

    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0 // return an integer exit code
