namespace MyStore.Web

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

open Giraffe
open Giraffe.EndpointRouting
open JavaScriptEngineSwitcher.ChakraCore
open JavaScriptEngineSwitcher.Extensions.MsDependencyInjection
open React.AspNet
open Fable.SignalR

open MyStore.Web.Router
open MyStore.Data
open MyStore.Data.Identity
open MyStore.Domain.Chat
open MyStore.Web.Chat

type Startup(configuration: IConfiguration) =
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddGiraffe() |> ignore

        services.AddSignalR(SignalRHub.config) |> ignore

        services.AddDbContext<Context>
            (fun options ->
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                |> ignore)
        |> ignore

        services.AddDatabaseDeveloperPageExceptionFilter()
        |> ignore

        services
            .AddDefaultIdentity<ApplicationUser>(fun options -> options.SignIn.RequireConfirmedAccount <- true)
            //.AddEntityFrameworkStores<Context>() TODO:
            .AddEntityFrameworkStores<Context>()
            //.AddEntityFrameworkStores<Context>() TODO:
        |> ignore

        services.AddAuthentication() |> ignore

        // Add framework services. TODO?
        services
            .AddControllersWithViews()
            .AddRazorRuntimeCompilation()
        |> ignore

        services
            .AddJsEngineSwitcher(fun options -> options.DefaultEngineName <- ChakraCoreJsEngine.EngineName)
            .AddChakraCore()
        |> ignore

        services.AddReact() |> ignore

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
        |> ignore

        services.BuildServiceProvider() |> ignore

        services.AddRazorPages() |> ignore // TODO?

    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        app.UseReact
            (fun config ->
                config
                    .SetReuseJavaScriptEngines(true)
                    .SetLoadBabel(false)
                    .SetLoadReact(false)
                    .SetReactAppBuildPath("~/dist")
                |> ignore)
        |> ignore

        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
            app.UseMigrationsEndPoint() |> ignore
        else
            app.UseExceptionHandler("/Home/Error") |> ignore
            app.UseHsts() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseStaticFiles() |> ignore


        app.UseRouting() |> ignore

        app.UseAuthentication() |> ignore

        app.UseAuthorization() |> ignore

        app.UseSignalR(SignalRHub.config) |> ignore

        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapGiraffeEndpoints(endpoints1)
                //endpoints.MapHub(Endpoints.Root) |> ignore
                //endpoints.MapControllerRoute(name = "default", pattern = "{controller=Home}/{action=Index}/{id?}")
                //|> ignore

                endpoints.MapRazorPages() |> ignore)
        |> ignore
