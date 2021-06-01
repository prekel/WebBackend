module MyStore.Web.Handlers.Chat

open System
open System.Net.Mime
open System.Collections.Generic
open System.Diagnostics
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Identity

open FSharp.Control.Tasks
open FSharp.UMX
open Giraffe
open Giraffe.EndpointRouting
open Giraffe.Razor

open MyStore.Data
open MyStore.Data.Identity
open MyStore.Web.Models
open MyStore.Dto.Shop
open MyStore.Domain.Shop
open MyStore.Web.Core
open MyStore.Web.Database

let chatPage : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) -> task { return! razorHtmlView "Chat/Index" None None None next ctx }
