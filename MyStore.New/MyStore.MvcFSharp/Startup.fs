namespace MyStore.MvcFSharp

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
open MyStore.MvcFSharp.Models

type Startup(configuration: IConfiguration) =
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

    let antiforgeryTokenHandler =
        text "Bad antiforgery token"
        |> RequestErrors.badRequest
        |> validateAntiforgeryToken

    let endpoints1 =
        [ subRoute "/foo" [ GET [ route "/bar" (text "Aloha!") ] ]
          GET [ route "/" (text "Hello World")
                routef "/%s/%i" handler2
                routef "/%s/%s/%s/%i" handler3 ]
          GET_HEAD [ route "/foo" (text "Bar")
                     route "/x" (text "y")
                     route "/abc" (text "def") ]
          // Not specifying a http verb means it will listen to all verbs
          subRoute "/sub" [ route "/test" handler1 ]
          subRoute
              "/Home"
              [ GET [ route "/" (indexHandler >=> antiforgeryTokenHandler)
                      route "/Privacy" privacyHandler
                      route "/Error" errorHandler ] ] ]


    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()
        |> ignore

        services.AddRazorPages() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseExceptionHandler("/Home/Error") |> ignore
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseStaticFiles() |> ignore

        app.UseRouting() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapGiraffeEndpoints(endpoints1)

                //endpoints.MapControllerRoute(name = "default", pattern = "{controller=Home}/{action=Index}/{id?}")
                //|> ignore

                //endpoints.MapRazorPages() |> ignore
                )
        |> ignore
