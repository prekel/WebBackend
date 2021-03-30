module MyStore.Saturn.Auth.Handlers

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

let signIn =
    fun next (ctx: HttpContext) ->
        task {
            let testUser =
                {| UserId = 1L
                   Email = "misterptits@yandex.ru"
                   FullName = "Vladislav Prekel" |}

            let claims =
                [ Claim(ClaimTypes.Sid, testUser.UserId |> string)
                  Claim(ClaimTypes.Email, testUser.Email)
                  Claim(ClaimTypes.Surname, testUser.FullName)
                  Claim(ClaimTypes.Role, "Admin") ]

            let claimsIdentity =
                ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme)

            let authProperties = AuthenticationProperties()

            let! a =  ctx.WriteHtmlViewAsync Views.layout
            
            do! ctx.SignInAsync
                    (CookieAuthenticationDefaults.AuthenticationScheme, ClaimsPrincipal(claimsIdentity), authProperties)
            //                logger.LogInformation (sprintf "User signed (Email = %s, FullName = %s)" testUser.Email testUser.FullName)
            return! Successful.ok (json (testUser)) next ctx
        }
