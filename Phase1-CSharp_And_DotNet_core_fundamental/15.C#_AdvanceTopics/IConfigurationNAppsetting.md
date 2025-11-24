# WHAT IConfiguration IS

- IConfiguration is the unified configuration abstraction in Microsoft.Extensions.Configuration.
- It aggregates values from multiple providers (JSON files, environment variables, command line, secrets stores,
etc.) and exposes a hierarchical key/value API and binding helpers for POCOs.
- In non-ASP apps you typically use the Generic Host (Host.CreateDefaultBuilder) which wires configuration for 
you, or build a ConfigurationBuilder manually when you need config before host creation.

OPTION 1 — Recommended: use Generic Host (production style)
------------------------------------------------------------

Host.CreateDefaultBuilder wires config sources in this order (later sources override earlier):
- appsettings.json
- appsettings.{ENVIRONMENT}.json
- Environment variables
- Command-line args
Above is the sequence of priority 

This is the simplest production-ready approach.

# Example Program.cs (Generic Host + DI, non-ASP):

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args) // loads appsettings.json, env-specific, env vars, command line
            .ConfigureServices((context, services) =>
            {
                // Bind strongly-typed options
                services.Configure<AppOptions>(context.Configuration.GetSection("App"));

                // Register a hosted background service (or some worker class)
                services.AddHostedService<Worker>();

                // Register any other services that need IConfiguration injected
                services.AddSingleton<IMyService, MyService>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.AddConsole(); // or use Serilog via UseSerilog(...)
            })
            .Build();

        await host.RunAsync();
    }
}

public class AppOptions
{
    public string Name { get; set; } = "";
    public int MaxItemsPerRun { get; set; } = 100;
}

# How to read config inside services:

public class MyService : IMyService
{
    private readonly IConfiguration _config;
    private readonly ILogger<MyService> _logger;

    public MyService(IConfiguration config, ILogger<MyService> logger)
    {
        _config = config;
        _logger = logger;
    }

    public void DoSomething()
    {
        // ad-hoc read
        var mode = _config["FeatureFlags:FastMode"];            // returns string "true"/"false" or null
        var conn = _config.GetConnectionString("Default");     // extension method
        _logger.LogInformation("FastMode={FastMode} Conn={Conn}", mode, conn);
    }
}

OPTION 2 — Manual ConfigurationBuilder (small tools or early bootstrap)
------------------------------------------------------------

Use this when you need configuration before building the Host—for example to bootstrap Serilog or to decide which host to create.

using var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")}.json", optional: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var conn = config.GetConnectionString("Default");
var fastMode = config.GetValue<bool>("FeatureFlags:FastMode");
Console.WriteLine($"Conn={conn}, FastMode={fastMode}");


Notes:

- reloadOnChange: true allows the config to be reloaded when the JSON file changes (useful with IOptionsMonitor<T>).

- GetValue<T>("path") converts values to typed values and supports default values: config.GetValue<int>
("App:MaxItemsPerRun", 100).

Get section directly:

var app = config.GetSection("App").Get<AppOptions>();
// or
var hosts = config.GetSection("Hosts").Get<List<string>>();

Connection strings common pattern:

config.GetConnectionString("Default") reads ConnectionStrings:Default.

# Note 
Yes — appsettings.{Environment}.json overrides appsettings.json.

Here’s the exact configuration precedence order used in .NET (for both ASP.NET Core and non-ASP.NET Core apps 
that use Host.CreateDefaultBuilder

Configuration Precedence (from lowest → highest priority)

When you use:

Host.CreateDefaultBuilder(args)

or manually add configuration in the same order as Microsoft does internally, the order is:

| **Order** | **Source**                         | **Description**  |
| --------- | ---------------------------------- |------------------|
| 1️⃣       | `appsettings.json`                 | Base/default settings for all environments.                       |
| 2️⃣       | `appsettings.{Environment}.json`   | Overrides keys in `appsettings.json` when environment matches.    |
| 3️⃣       | User Secrets (in Development only) | Developer-only secrets stored outside source control.             |
| 4️⃣       | Environment Variables              | Overrides JSON values for deployment (e.g., `App__Name=ProdApp`). |
| 5️⃣       | Command-line arguments             | Highest priority; overrides everything else.                      |

Environment variable set before run:

set DOTNET_ENVIRONMENT=Production


# AppSettings.json
Checkout this to get understanding of basics 
https://youtu.be/tQdNlju2UXo

Practical guide you can use right away to run your .NET app with environment-specific appsettings (Development, 
QA, Production). It covers file naming, folder layout, how the host picks files, how to set the environment 
locally, in CI/CD, Docker and systemd, and safe secret handling. All examples assume a non-ASP .NET Core / .NET 
5+ app using the Generic Host.

(Note : Will not cover CI/CD, docker and systemd, will only go thorugh how to do it locally)

Overview — the idea
-------------------
- Keep a base file appsettings.json with defaults.
- Add environment-specific overrides appsettings.Development.json, appsettings.QA.json, appsettings.Production. 
json.
- Set the environment name (DOTNET_ENVIRONMENT or ASPNETCORE_ENVIRONMENT). The host loads appsettings.json 
first, then the environment file which overrides keys.
- Use environment variables or a secret store for secrets (never commit secrets into JSON).

File layout (project root)

- appsettings.json
- appsettings.Development.json
- appsettings.QA.json
- appsettings.Production.json
- Program.cs, Worker.cs, etc.

Example contents

appsettings.json (defaults)

{
  "App": {
    "Name": "MyApp",
    "MaxItems": 50
  },
  "ConnectionStrings": {
    "Default": "Server=(local);Database=MyAppDev;Trusted_Connection=True;"
  }
}


appsettings.Development.json

{
  "App": {
    "MaxItems": 20
  },
  "ConnectionStrings": {
    "Default": "Server=(localdb)\\mssqllocaldb;Database=MyAppDev;Trusted_Connection=True;"
  }
}


appsettings.QA.json

{
  "App": {
    "MaxItems": 200
  },
  "ConnectionStrings": {
    "Default": "Server=qa-db:1433;Database=MyAppQa;User Id=qauser;Password=__FROM_ENV__;"
  }
}


appsettings.Production.json

{
  "App": {
    "MaxItems": 1000
  },
  "ConnectionStrings": {
    "Default": "Server=prod-db:1433;Database=MyApp;User Id=produser;Password=__FROM_ENV__;"
  }
}

Key points :
-----------
- JSON keys merge; values in appsettings.{ENV}.json override appsettings.json for matching keys.
- Never store real passwords in JSON in production — use environment variables or a secret provider. In sample 
above put placeholders and set actual secrets elsewhere.

How .NET chooses which environment file to load
-----------------------------------------------
- Host.CreateDefaultBuilder reads:
- appsettings.json
- appsettings.{Environment}.json (Environment comes from DOTNET_ENVIRONMENT or ASPNETCORE_ENVIRONMENT)
- Environment variables
- Command-line args

So appsettings.Production.json overrides appsettings.json but environment variables override both.

Example : Generic host, can use configurationbuilder manually also 

Program.cs — recommended pattern (Generic Host)

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args) // loads appsettings.json + appsettings.{env}.json + env vars + cmdline
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<AppOptions>(ctx.Configuration.GetSection("App"));
        services.AddHostedService<Worker>();
    })
    .ConfigureLogging((ctx, logging) =>
    {
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();

How to set the environment locally
-----------------------------------
- PowerShell (Windows)

($env:DOTNET_ENVIRONMENT="Development") // Without braces
dotnet run

- Bash (Linux/macOS)

export DOTNET_ENVIRONMENT=QA
dotnet run

You can also pass command-line override for a single run:

dotnet run -- --DOTNET_ENVIRONMENT=QA

(note: that’s passing an argument to your app; environment variable is preferred)

How to check the active environment in code
-------------------------------------------
var env = host.Services.GetRequiredService<IHostEnvironment>();
Console.WriteLine("Environment: " + env.EnvironmentName);

# Example flow for developer → QA → Prod

- Developer uses appsettings.Development.json + user-secrets for local dev. Set DOTNET_ENVIRONMENT=Development 
  in IDE launchSettings or env.
- CI builds artifact including appsettings.json and appsettings.QA.json. Pipeline deploys to QA and sets 
  DOTNET_ENVIRONMENT=QA; secrets injected via pipeline variables.
- For Production, pipeline deploys artifact (may be same artifacts) and sets DOTNET_ENVIRONMENT=Production and 
  supplies secrets via secure store. appsettings.Production.json can be present with non-secret overrides (or 
  you can avoid having any production file and rely solely on env vars/secret provider).

(Note : you can set environment variable to project, go to project projecties, select debug and link on open 
debug launch profile UI)

checkout this link, to use environment variable : https://youtu.be/m0yjMV7LA1U

