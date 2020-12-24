namespace MyStore.WebApi

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.AspNetCore.Authorization
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.OpenApi.Models
open MyStore.Data

type Startup(configuration: IConfiguration) =
    let MyAllowSpecificOrigins = "_myAllowSpecificOrigins"

    member this.Configuration = configuration

    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        // Add framework services.
        services.AddControllers() |> ignore

        services.AddSwaggerGen(fun c -> c.SwaggerDoc("v1", OpenApiInfo(Title = "MyStore.WebApi", Version = "v1")))
        |> ignore

        services.AddDbContext<Context>() |> ignore

        services.AddCors() |> ignore

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseSwagger() |> ignore

            app.UseSwaggerUI(fun c -> c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyStore.WebApi v1"))
            |> ignore

        app
            .UseHttpsRedirection()
            .UseRouting()
            //.UseAuthorization()
            .UseCors(fun builder -> builder.AllowAnyOrigin() |> ignore)
            .UseEndpoints(fun endpoints -> endpoints.MapControllers() |> ignore)
        |> ignore
