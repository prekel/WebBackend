module MyStore.Saturn.Router

open System.Threading
open Microsoft.AspNetCore.Http
open Saturn
open Giraffe.Core
open Giraffe.ResponseWriters
open FSharp.Control.Tasks.ContextInsensitive
open Saturn.Auth
open Saturn.ChallengeType
open System.Security.Claims
open Microsoft.AspNetCore.Authentication.Cookies
open Microsoft.AspNetCore.Authentication
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Logging
open Giraffe

open MyStore.Saturn.Templates

let browser =
    pipeline {
        plug acceptHtml
        plug putSecureBrowserHeaders
        plug fetchSession
        set_header "x-pipeline-type" "Browser"
    }

let defaultView =
    router {
        get "/" (htmlView Index.layout)
        get "/index.html" (redirectTo false "/")
        get "/default.html" (redirectTo false "/")
    }

let aut =
    pipeline { plug (requireAuthentication Cookies) }

let browserRouter =
    router {
        not_found_handler (htmlView NotFound.layout) //Use the default 404 webpage
        pipe_through browser //Use the default browser pipeline
        pipe_through aut

        forward "/products" Products.Controller.resource
        forward "" defaultView //Use the default view
    }

//Other scopes may use different pipelines and error handlers

let api =
    pipeline {
        //plug acceptJson
        set_header "x-pipeline-type" "Api"
    }


let someScopeOrController =
    router {
        get "/signin" MyStore.Saturn.Auth.Handlers.signIn

        get "/long/%s" (fun (next: HttpFunc) (ctx: HttpContext) ->
            task { return! text "Successfully logged in" next ctx })

        getf "/short/%s/%s" (fun (i, j) func ctx ->
            task {
                Controller.getConfig ctx |> printfn "%A"

                let! r = json (sprintf "%s short" i) func ctx
                return r
            })

        not_found_handler (text "Not Found")
    }

let apiRouter =
    router {
        not_found_handler (text "Api 404")
        pipe_through api

        forward "/someApi" someScopeOrController
    }

let appRouter =
    router {
        forward "/api" apiRouter
        forward "" browserRouter
    }
