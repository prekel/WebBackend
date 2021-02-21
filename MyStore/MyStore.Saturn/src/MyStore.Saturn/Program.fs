module MyStore.Saturn.Server

open System.Threading.Tasks
open Microsoft.AspNetCore.Http
open Saturn

open MyStore.Saturn.Config
open MyStore.Saturn.Templates

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

        use_cookies_authentication_with_config (fun options ->
            options.Events.OnRedirectToLogin <-
                fun context ->
                    context.Response.StatusCode <- StatusCodes.Status401Unauthorized
                    Task.CompletedTask

            options.Events.OnRedirectToAccessDenied <-
                fun context ->
                    context.Response.StatusCode <- StatusCodes.Status403Forbidden
                    Task.CompletedTask)

        use_config (fun _ ->
            { connectionString = "Host=;Database=postgres;Username=postgres;Password=" })
    }

[<EntryPoint>]
let main _ =
    printfn "Working directory - %s" (System.IO.Directory.GetCurrentDirectory())
    run app
    0
