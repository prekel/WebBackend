module MyStore.Web.Router

open System
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

open FSharp.Control.Tasks

open Giraffe
open Giraffe.EndpointRouting
open Giraffe.Razor
open MyStore.Web.Models
open MyStore.Dto.Shop

let handler1 : HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) -> ctx.WriteTextAsync "Hello World"

let handler2 (firstName: string, age: int) : HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        sprintf "Hello %s, you are %i years old." firstName age
        |> ctx.WriteTextAsync

let handler3 (a: string, b: string, c: string, d: int) : HttpHandler =
    fun (_: HttpFunc) (ctx: HttpContext) ->
        sprintf "Hello %s %s %s %i" a b c d
        |> ctx.WriteTextAsync

let indexHandler =
    razorHtmlView "Home/Index" None None None

let privacyHandler =
    razorHtmlView "Home/Privacy" None None None

let errorHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let reqId =
                if isNull Activity.Current then
                    ctx.TraceIdentifier
                else
                    Activity.Current.Id

            return! razorHtmlView "Home/Error" (Some { RequestId = reqId }) None None next ctx
        }

let cartHandler (id: int) =
    fun (next: HttpFunc) (ctx: HttpContext) ->
        task {
            let cart =
                { CartDto.cartId = id
                  isPublic = true
                  ownerCustomerId = Nullable.op_Implicit 34 }

            let model =
                { CartModel.cart = cart
                  products =
                      [| { productId = 1
                           name = "asd"
                           description = "asdd"
                           price = 12. }
                         { productId = 2
                           name = "asd2"
                           description = "asdd2"
                           price = 122. } |] }

            return! razorHtmlView "Shop/Cart" (Some model) None None next ctx
        }

let antiforgeryTokenHandler =
    text "Bad antiforgery token"
    |> RequestErrors.badRequest
    |> validateAntiforgeryToken

let mustBeLoggedIn : HttpHandler =
    requiresAuthentication (redirectTo false "/Identity/Account/Login")

let endpoints1 =
    [ subRoute "/foo" [ GET [ route "/bar" (text "Aloha!") ] ]
      GET [ route "/" (redirectTo false "/Home")
            routef "/%s/%i" handler2
            routef "/%s/%s/%s/%i" handler3
            route "/private" (mustBeLoggedIn >=> (text "Private"))
            route "/public" (text "public") ]
      GET_HEAD [ route "/foo" (text "Bar")
                 route "/x" (text "y")
                 route "/abc" (text "def") ]
      // Not specifying a http verb means it will listen to all verbs
      subRoute "/sub" [ route "/test" handler1 ]
      subRoute
          "/Home"
          [ GET [ route "/" (indexHandler >=> antiforgeryTokenHandler)
                  route "/Privacy" privacyHandler
                  route "/Error" errorHandler ] ]
      subRoute "/Cart" [ GET [ routef "/%i" cartHandler ] ] ]
