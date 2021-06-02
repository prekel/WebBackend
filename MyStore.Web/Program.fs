module MyStore.Web.Program

open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting

let CreateHostBuilder args =
    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webBuilder -> webBuilder.UseStartup<Startup>() |> ignore)

[<EntryPoint>]
let main args =
    CreateHostBuilder(args).Build().Run()

    0
