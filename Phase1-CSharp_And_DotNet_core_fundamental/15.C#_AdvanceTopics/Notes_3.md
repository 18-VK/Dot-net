# Target-typed new
Target-typed new expressions (introduced in C# 9) make object creation cleaner by letting the compiler infer 
the type from the context — you don’t have to repeat it.

1. Basic Example

Before C# 9:
Person person = new Person("Aman", 25);

With target-typed new:
Person person = new("Aman", 25);

Here, the compiler knows the type is Person from the left-hand side.

2. How It Works

The compiler looks at the target type (variable declaration, method parameter, return type, etc.)
If it can infer the type, you can just write new(...).

Example:

List<int> numbers = new();  // equivalent to new List<int>()
Dictionary<string, int> map = new() { ["A"] = 1, ["B"] = 2 };

# Local function

Local functions in C# are functions declared inside another method.
They help organize code by keeping helper logic close to where it’s used and hidden from the rest of the class.

Example : 

void Calculate()
{
    int Add(int x, int y)   // local function
    {
        return x + y;
    }

    int result = Add(5, 3);
    Console.WriteLine(result); // 8
}
Add is only accessible inside Calculate() — not anywhere else.

Why Use Local Functions
-----------------------
- To avoid cluttering a class with private helper methods.
- To make code easier to read and maintain.
- To access local variables of the enclosing method.

Access to Outer Variables
--------------------------
Local functions can capture variables from the outer method:

void DisplayTotal()
{
    int total = 100;

    void Show()
    {
        Console.WriteLine($"Total = {total}");
    }

    Show(); // prints: Total = 100
}

With Return Types
-----------------
You can return values as usual:

int GetSquare(int num)
{
    int Square(int n) => n * n;
    return Square(num);
}

When to Use
------------
- Use local functions when:
- The helper method is only meaningful inside one method.
- You want to hide implementation details.
- You need access to local variables easily.

# Global usings
Global usings (introduced in C# 10 / .NET 6) let you declare namespaces once and make them available across the 
entire project, instead of repeating using statements in every file.

> The Problem (Before C# 10)

You had to repeat the same using directives in many files:

// In every file
using System;
using System.Collections.Generic;
using System.Linq;

This creates clutter — especially in large projects.

> The Solution: Global Usings

You can mark a using directive as global, so it applies project-wide:

global using System;
global using System.Collections.Generic;
global using System.Linq;


These can go in:

- A dedicated file (commonly named GlobalUsings.cs)
- Or at the top of Program.cs

# Top-level statements

Top-level statements (introduced in C# 9) allow you to write C# programs without explicitly defining a Program 
class or Main() method.
This feature simplifies small apps, scripts, and console programs.

> Before C# 9
You had to wrap everything in a Program class and Main method:

using System;
namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}

> With Top-Level Statements

Now you can write:

using System;
Console.WriteLine("Hello, World!");

No need for class Program or Main() — the compiler automatically generates them behind the scenes.

> How It Works

When you use top-level statements, the compiler:

- Creates a hidden Program class.
- Wraps your statements inside a generated Main method.
- Only one file in the project can contain top-level statements.

> You Can Still Use Methods

You can define local functions and variables inside top-level statements:

using System;

void Greet(string name) => Console.WriteLine($"Hello, {name}!");
string user = "Aman";
Greet(user);

> Mixing with Other Classes

You can still declare other types below the top-level code:

Console.WriteLine(new Helper().GetMessage());

class Helper
{
    public string GetMessage() => "Top-level works!";
}

# Required properties

Required properties were introduced in C# 11 to ensure that important object properties must be initialized — 
either during object creation or in a constructor.
They help prevent accidentally leaving key fields unset.

> The Problem (Before C# 11)

Without required, it’s easy to forget to initialize properties:

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
}

var user = new User(); // ❌ Forgot to set Name and Email — no compile error

This can lead to null reference bugs later.

> Using required

With required, the compiler forces initialization:

public class User
{
    public required string Name { get; set; }
    public required string Email { get; set; }
}

Now you must set both Name and Email when creating the object.

> Correct Usage

✅ This works:

var user = new User
{
    Name = "Aman",
    Email = "aman@example.com"
};

❌ This fails to compile:

var user = new User(); // Error: required members not initialized

> You Can Also Set Them in a Constructor

You can satisfy the required rule by setting them inside a constructor:

public class User
{
    public required string Name { get; set; }
    public required string Email { get; set; }

    public User(string name, string email)
    {
        Name = name;
        Email = email;
    }
}

Then this is valid:

var user = new User("Aman", "aman@example.com");

> Works with Init-Only Properties

Often combined with init to make immutable-like objects:

public class Product
{
    public required string Name { get; init; }
    public required decimal Price { get; init; }
}

var p = new Product { Name = "Laptop", Price = 599.99m }; // ✅

> Inheritance Rules

If a base class has a required property, derived classes must also ensure it’s initialized.
The required modifier is inherited by derived types.

class Base
{
    public required string Id { get; set; }
}

class Derived : Base
{
    public string Description { get; set; } = "";
}
var d = new Derived(); // ❌ must set Id

# Default interface 

Default interface methods (introduced in C# 8) let you provide a default implementation for a method directly 
inside an interface.

Before this feature, interfaces could only declare methods, not define their behavior — all implementing 
classes had to write their own version.

> Before C# 8

Interfaces could only define method signatures:

public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine(message);
}

Every implementing class had to define Log() — even if the logic was identical.

> After C# 8 — Default Implementation

Now you can give the interface method a default body:

public interface ILogger
{
    void Log(string message)
    {
        Console.WriteLine($"[Default] {message}");
    }
}

If a class does not override the method, the default version runs automatically. 

> Why It’s Useful

Backward compatibility:
You can add new methods to an existing interface without breaking existing implementations.

Shared logic:
Avoids repeating the same implementation across multiple classes.

# Records 
Records (introduced in C# 9) are special reference types designed to make immutable, value-based objects simple 
to create.

They’re ideal for data models, DTOs, and objects that represent data rather than behavior.

> The Problem (Before C# 9)

Defining simple data-holding classes was repetitive:

public class Person
{
    public string FirstName { get; }
    public string LastName { get; }

    public Person(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public override bool Equals(object obj) =>
        obj is Person p && p.FirstName == FirstName && p.LastName == LastName;

    public override int GetHashCode() => HashCode.Combine(FirstName, LastName);

    public override string ToString() => $"{FirstName} {LastName}";
}


Way too much code for something simple.

> The Solution — Records

With records, all that boilerplate is auto-generated:

public record Person(string FirstName, string LastName);

That’s it — you get:

- Auto ToString()
- Auto Equals() and GetHashCode() (value-based)
- Auto constructor and property deconstruction

> Example Usage
var p1 = new Person("Aman", "Kumar");
var p2 = new Person("Aman", "Kumar");

Console.WriteLine(p1 == p2);   // True (value equality)
Console.WriteLine(p1);         // Person { FirstName = Aman, LastName = Kumar }

> Value-Based Equality

Unlike classes, which use reference equality by default, records use value equality.

var c1 = new Person("Aman", "Kumar");
var c2 = new Person("Aman", "Kumar");
Console.WriteLine(c1.Equals(c2)); // True ✅

For normal classes, that would be False.

> Immutable by Default

The positional record creates init-only properties:

public record Person(string FirstName, string LastName);

var p = new Person("Aman", "Kumar");
// p.FirstName = "John"; ❌ Error: init-only

If you need mutable records:

public record Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

> “With” Expression (Cloning)

You can create a copy of a record with modifications using the with keyword:

var p1 = new Person("Aman", "Kumar");
var p2 = p1 with { LastName = "Singh" };

Console.WriteLine(p2); // Person { FirstName = Aman, LastName = Singh }

The with expression keeps immutability but enables easy updates.

> Deconstruction

You can easily deconstruct record values:

var p = new Person("Aman", "Kumar");
var (first, last) = p;
Console.WriteLine($"{first} {last}");

> Records as Reference Types

Records are reference types, but they behave like value types in equality checks.
C# 10 added record struct for value-type records:

public record struct Point(int X, int Y);

> Inheritance and Sealing

Records support inheritance:

public record Person(string Name);
public record Student(string Name, string School) : Person(Name);

You can also seal a record to prevent inheritance:

public sealed record Employee(string Name, int Id);

# Partial class
In C#, a partial class allows the definition of a single class to be split into multiple files. At compile 
time, all the parts are combined into one complete class. This feature is especially useful in large projects 
where separating auto-generated code from developer-written code makes the program easier to manage and 
maintain.

Partial classes are commonly used in scenarios such as Windows Forms, ASP.NET applications and code generators 
where some parts of the class are system-generated and others are written by the programmer.

Syntax
public partial class ClassName{

// code

}

Key Points : 

- Declared using the partial keyword.
- A class can be split across multiple files.
- All parts must use the same name and namespace.
- At compile time, the compiler merges all parts into a single class.
- Useful for separating auto-generated and user-defined code.
- Supports defining fields, methods, properties, events and nested classes.

Example :

In Geeks1.cs and Geeks2.cs, a partial class is created using the partial keyword and each file contains different functionality of Geeks class as shown below.

Geeks1.cs

public partial class Geeks {
    private string Author_name;
    private int Total_articles;

    public Geeks(string a, int t)
    {
        this.Author_name = a;
        this.Total_articles = t;
    }
}
Geeks2.cs

public partial class Geeks {
    public void Display()
    {
        Console.WriteLine("Author's name is : " + Author_name);
        Console.WriteLine("Total number of articles is : " + Total_articles);
    }
}
