# Ways to use relational database in C# Application
1. ADO.NET (Low-Level, Manual Control)

What it is:
The oldest and most direct way to interact with databases in .NET — uses SqlConnection, SqlCommand, 
SqlDataReader, etc.

When to use:
When you need maximum control, high performance, or want to execute raw SQL queries manually.

Pros

- Very fast and lightweight.
- Full control over SQL, transactions, and parameters.

Cons

- Verbose and error-prone.
- You manually handle mapping between rows and objects.

2. Dapper (Micro-ORM)

What it is:
A lightweight ORM developed by Stack Overflow. It maps query results to objects automatically but leaves SQL control to you.

When to use:
When you want performance + object mapping without heavy frameworks like EF Core.

Example:

using Dapper;
using System.Data.SqlClient;

string connectionString = "Server=.;Database=ShopDB;Trusted_Connection=True;";

using (var connection = new SqlConnection(connectionString))
{
    var products = connection.Query<Product>(
        "SELECT * FROM Products WHERE Price > @price", new { price = 100 });

    foreach (var p in products)
        Console.WriteLine($"{p.Name} - {p.Price}");
}


Pros

- Fast (near ADO.NET performance).
- Simple object mapping.
- No context or tracking overhead.

Cons

- You still write SQL manually.
- No migrations or schema management.

3. Entity Framework Core (ORM)

What it is:
A full-fledged Object Relational Mapper that lets you work with data as C# objects. You can use LINQ instead of raw SQL.

When to use:
For enterprise apps, CRUD-heavy systems, or when productivity > raw speed.

Example (Code-First):

public class ShopContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}

var context = new ShopContext();
var products = context.Products.Where(p => p.Price > 100).ToList();


Pros

- No SQL writing needed (can use LINQ).
- Handles migrations, relationships, and tracking.
- Supports lazy loading and change tracking.

Cons

- Slower than Dapper or ADO.NET for large queries.
- Requires more configuration and understanding.

4. EF Core + Raw SQL

EF Core allows mixing LINQ and SQL:

var result = context.Products
    .FromSqlRaw("SELECT * FROM Products WHERE Price > {0}", 100)
    .ToList();

Best of both worlds — ORM mapping + SQL control.

Recommended in Modern .NET Apps
------------------------------
- Small projects / APIs: Dapper
- Enterprise or complex apps: EF Core (with migrations)
- Performance-critical reporting: ADO.NET or Dapper for raw queries
- Hybrid: Use EF Core + Dapper together (common in real-world apps)

# Note : 
Should have knowledge of ADO .net, dapper also with EFCore
Can ignore ADO .net but other two's are must

# String Interpolation 
String Interpolation in C# is a concise and readable way to embed variables or expressions directly inside 
string.It was introduced in C# 6.0 and has become the standard way to format strings.

Basic Syntax

You prefix the string with a $ symbol and insert variables or expressions inside curly braces {}.

string name = "Aman";
int age = 25;

string message = $"My name is {name} and I am {age} years old.";
Console.WriteLine(message);

# Environment Variables in C#

Environment variables are system-wide or user-specific key–value pairs that store configuration settings 
outside your application code.
They’re often used for secrets, connection strings, or environment-specific settings like “Development”, 
“Staging”, or “Production”.

> Reading Environment Variables

You can access environment variables via the System.Environment class.

using System;

class Program
{
    static void Main()
    {
        string? path = Environment.GetEnvironmentVariable("PATH");
        Console.WriteLine(path);
    }
}

Returns the value of the environment variable PATH.

> Setting Environment Variables
On Windows (PowerShell)

setx MyApp_ConnectionString "Server=.;Database=ProdDB;Trusted_Connection=True;"

# launchSettings.json

What launchSettings.json is — short
-----------------------------------
It's a developer-only file that configures how the project is launched when debugging locally (Visual Studio, 
Rider, VS Code C# extension) or when using dotnet run --launch-profile.

It lives under the project Properties folder and is not used in production. It helps set environment variables, 
command-line args, which "profile" to use (IIS Express, Project, Executable), URLs, whether to launch a 
browser, etc.

It is intended only for local debugging/development workflows — it is not part of the app runtime config for 
production. Treat it as a dev convenience.

Where it is
-----------
- File path: <project-root>/Properties/launchSettings.json
- If missing, you can create it manually (create Properties folder then the file).

Basic structure and important fields (explained)
A launchSettings.json contains a top-level "profiles" object (plus optionally iisSettings). Each profile 
configures a launch scenario.

Key fields you'll encounter:
---------------------------
- commandName — type of launch:
- "Project": run the project (dotnet run behavior).
- "IISExpress": Visual Studio IIS Express for web apps.
- "Executable": run an external executable with args.
- "Docker": Docker launch (when using container tools).
- applicationUrl — for web projects: the URL(s) Kestrel will bind to (example "https://localhost:5001;http://localhost:5000").
- environmentVariables — key/value pairs set for the launched process (commonly DOTNET_ENVIRONMENT).
- launchBrowser (bool) — for web projects, whether to open a browser when starting.
- launchUrl — for web projects, the path opened in the browser.
- commandLineArgs — string of arguments passed to the app when launched through the profile.
- dotnetRunMessages — (true/false) show SDK run messages.
- inspectUri — used for certain debugger integrations (auto-populated by VS).
- iisSettings — top-level IISExpress configuration (ssl port, site name); used by Visual Studio for web projects.

Explanation:

- DOTNET_ENVIRONMENT=Development means Host.CreateDefaultBuilder will load appsettings.Development.json 
    after appsettings.json.
- App__MaxItemsPerRun maps to the configuration key App:MaxItemsPerRun (double-underscore maps to colons).
- commandLineArgs will be visible to args[] or Host if you read them.

example : 

Web project example (ASP.NET Core — dev only)

{
  "iisSettings": {
    "windowsAuthentication": false,
    "anonymousAuthentication": true,
    "iisExpress": {
      "applicationUrl": "http://localhost:49270",
      "sslPort": 44300
    }
  },
  "profiles": {
    "IIS Express": {
      "commandName": "IISExpress",
      "launchBrowser": true,
      "launchUrl": "weatherforecast",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "MyWebApp": {
      "commandName": "Project",
      "launchBrowser": true,
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ConnectionStrings__Default": "Server=.;Database=DevDb;Trusted_Connection=True;"
      }
    }
  }
}

