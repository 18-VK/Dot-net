# Dependency Injection (DI)
Dependency Injection (DI) is a design pattern used to achieve loose coupling between classes.
Instead of a class creating its dependencies directly, the dependencies are injected (provided) from the 
outside.

A class should not create its own dependencies; they should be provided to it.

Without Dependency Injection (Tight Coupling)
---------------------------------------------
Let’s say we have a CustomerService class that needs EmailService to send emails.

public class EmailService
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class CustomerService
{
    private EmailService _emailService;

    public CustomerService()
    {
        // tightly coupled — CustomerService decides which EmailService to use
        _emailService = new EmailService();
    }

    public void RegisterCustomer(string name)
    {
        Console.WriteLine($"Customer {name} registered!");
        _emailService.SendEmail("Welcome " + name);
    }
}

Problems:

- CustomerService depends directly on EmailService.
- You can’t easily replace EmailService (e.g., with SmsService or a mock for testing).
- Harder to maintain and test.

With Dependency Injection (Loose Coupling)
------------------------------------------
We introduce an abstraction (interface) and let something else (like .NET’s DI container) provide the 
implementation.

public interface IEmailService
{
    void SendEmail(string message);
}

public class EmailService : IEmailService
{
    public void SendEmail(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class CustomerService
{
    private readonly IEmailService _emailService;

    // Dependency is injected via constructor
    public CustomerService(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public void RegisterCustomer(string name)
    {
        Console.WriteLine($"Customer {name} registered!");
        _emailService.SendEmail("Welcome " + name);
    }
}

Now we don’t create EmailService inside CustomerService.
It’s injected (supplied) by an external mechanism — like a DI Container.

Note : Now it can be replace by SMEService class, by creating class like : 
public class EmailService : IEmailService for SMEService and passing it into constructor "CustomerService
(IEmailService emailService)"

# Why Use Dependency Injection?

| **Benefit**                | **Explanation**                                                    |
| -------------------------- | ------------------------------------------------------------------ |
| **Loose Coupling**         | Classes depend on abstractions, not concrete classes.              |
| **Easier Testing**         | You can inject mock or fake dependencies in unit tests.            |
| **Better Maintainability** | You can change implementations without touching other classes.     |
| **Reusability**            | Components are more reusable and interchangeable.                  |
| **Cleaner Code**           | Reduces boilerplate and object creation logic in business classes. |

# How DI Works in .NET Core

In .NET Core, Dependency Injection is built-in.
We configure dependencies in the Service Container (via IServiceCollection) and the framework automatically 
injects them where required.

Example — Built-in DI in .NET Core Console App
----------------------------------------------
> Step 1: Create the interfaces and classes
public interface IMessageService
{
    void Send(string message);
}

public class EmailService : IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SmsService : IMessageService
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}

public class NotificationManager
{
    private readonly IMessageService _messageService;

    public NotificationManager(IMessageService messageService)
    {
        _messageService = messageService;
    }

    public void Notify(string message)
    {
        _messageService.Send(message);
    }
}

> Step 2: Register dependencies in the DI Container
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

// Register dependencies
services.AddTransient<IMessageService, EmailService>();
services.AddTransient<NotificationManager>();

var provider = services.BuildServiceProvider();

// Resolve NotificationManager — dependencies are automatically injected
var notifier = provider.GetRequiredService<NotificationManager>();
notifier.Notify("Hello from DI!");

Output:
Email sent: Hello from DI!

> Step 3: Changing Implementation Easily

You can switch implementation without touching other code:

services.AddTransient<IMessageService, SmsService>();

Now the output becomes:

SMS sent: Hello from DI!

That’s the power of DI 💪 — your business logic (NotificationManager) didn’t change at all.
# 🧩 How It Works Internally

Simplified flow:
- You register dependencies in the DI container:

services.AddTransient<IEmailService, EmailService>();

- When you request a class (e.g., CustomerService), .NET:
    * Scans its constructor.
    * Finds what dependencies it needs.
    * Automatically creates and injects those dependencies.
- You get a fully constructed object.

# DI Lifetimes in .NET

| Lifetime      | Description                                 | Example Use                     |
| ------------- | ------------------------------------------- | ------------------------------- |
| **Transient** | New instance every time it’s requested      | Lightweight, stateless services |
| **Scoped**    | One instance per request (in web apps)      | Web request-specific data       |
| **Singleton** | Single instance for the entire app lifetime | Logging, configuration, caching |

Example:

services.AddTransient<IEmailService, EmailService>();
services.AddScoped<IRepository, Repository>();
services.AddSingleton<ILogger, Logger>();

# What is a “Type” of Dependency Injection?

There are 3 main types of Dependency Injection in C# (and in general software design):
| **Type**                     | **How Dependency is Injected**             | **When Used** |
---------------------------------------------------------------------------------------------
| **1. Constructor Injection** | Through the class **constructor**  | Most common; preferred in .NET Core
| **2. Property Injection**    | Through a **public property** of the class | When dependency is optional or changeable
| **3. Method Injection**      | Through a **method parameter**             | When dependency is used only
within a single method |

We have seen Constructor Injection 

2. Property Injection

Dependencies are provided via a public property (setter).

Used when:

- Dependency is optional, or
- May change after object creation.

e.g : 
public class NotificationManager
{
    // Property Injection
    public IEmailService? EmailService { get; set; }

    public void Notify(string msg)
    {
        EmailService?.SendEmail(msg);
    }
}

Drawbacks

- The dependency might not be set (can lead to null reference errors).
- Harder to enforce dependencies at compile-time.
- Not recommended for critical dependencies (use constructor injection instead).

3. Method Injection

Dependencies are provided as parameters to a method.

Example
public class NotificationManager
{
    // Method Injection
    public void Notify(string msg, IEmailService emailService)
    {
        emailService.SendEmail(msg);
    }
}

When to Use

- When a dependency is used only once or\
- When it’s not needed throughout the lifetime of the class.

Drawback

-Can become messy if many methods require multiple dependencies.
-Doesn’t store the dependency for reuse.