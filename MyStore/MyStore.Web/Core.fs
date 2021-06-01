module MyStore.Web.Core

open System.Net.Mime
open Microsoft.AspNetCore.Http

open Giraffe
open Giraffe.Razor

let razorOrJson viewName model viewData modelState : HttpHandler =
    fun next ctx ->
        if ctx.Request.GetTypedHeaders().Accept
           |> Seq.exists (fun h -> h.ToString() = MediaTypeNames.Application.Json) then
            match model with
            | Some model -> json model next ctx
            | None -> json null next ctx
        else
            razorHtmlView viewName model viewData modelState next ctx
