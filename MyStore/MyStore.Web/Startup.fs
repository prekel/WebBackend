namespace MyStore.Web

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.EntityFrameworkCore

open Giraffe.EndpointRouting
open JavaScriptEngineSwitcher.ChakraCore
open JavaScriptEngineSwitcher.Extensions.MsDependencyInjection
open React.AspNet

open MyStore.Web.Router
open MyStore.Data
open MyStore.Data.Identity

type Startup(configuration: IConfiguration) =
    // This method gets called by the runtime. Use this method to add services to the container.
    member this.ConfigureServices(services: IServiceCollection) =
        services.AddDbContext<Context>
            (fun options ->
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                |> ignore)
        |> ignore

        services.AddDatabaseDeveloperPageExceptionFilter()
        |> ignore

        services
            .AddDefaultIdentity<ApplicationUser>(fun options -> options.SignIn.RequireConfirmedAccount <- true)
            .AddEntityFrameworkStores<Context>()
        |> ignore


        services
            .AddAuthentication()
        |> ignore

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

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        // Initialise ReactJS.NET. Must be before static files.
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
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseStaticFiles() |> ignore

        app.UseRouting() |> ignore

        app.UseAuthentication() |> ignore

        app.UseAuthorization() |> ignore

        app.UseEndpoints
            (fun endpoints ->
                endpoints.MapGiraffeEndpoints(endpoints1)

                //endpoints.MapControllerRoute(name = "default", pattern = "{controller=Home}/{action=Index}/{id?}")
                //|> ignore

                endpoints.MapRazorPages() |> ignore
                )
        |> ignore
