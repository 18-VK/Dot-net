Configuring the Model
--------------------
1) Configure Using Conventions 

EF Core Conventions or the default rules that you follow while creating the entity model. The EF Core uses
these to infer and to configure the Database.   It uses the information available in the POCO Classes to 
determine and infer the schema of the database that these classes are mapped to. For example, the table name, 
Column Name, Data Type, Primary keys are inferred from the  Class name, property name & Property type by 
convention to build the database.

2) Configure Using Data Annotations

Data Annotations allow us the configure the model classes by adding metadata to the class and also the class 
properties. The Entity Framework Core (EF Core) recognizes these attributes and uses them to configure the 
models. We have seen how to use Entity Framework Core Convention to configure our models in our previous 
tutorials. The conventions have their limits in their functionalities. Data Annotations in Entity Framework 
Core allow us to further fine-tune the model. They override the conventions.

What is Data Annotation : 

Data Annotations are the attributes that we apply to the class or on the properties of the class. They provide 
additional metadata about the class or its properties. These attributes are not specific to Entity Framework 
Core. They are part of a larger .NET  /.NET Core Framework. The ASP.NET MVC or ASP.NET MVC Core Applications 
also uses these attributes to validate the model.

The Data annotation attributes falls into two groups depending functionality provided by them.

- Data Modeling Attributes
- Validation Related Attributes

# Data Modelling Attributes
Data Modeling Attributes specify the schema of the database. These attributes are present in the namespace  
System.ComponentModel.DataAnnotations.Schema.The following is the list of attributes are present in the namespace.

# Validation Related Attributes
Validation related Attributes reside in the  System.ComponentModel.DataAnnotations namespace. We use these 
attributes to enforce validation rules for the entity properties. These attributes also define the size, 
nullability & Primary key, etc

Note : for list of attribute, go through given pdf file.. 

Extra : 
What "C" and "C2" Mean

These are standard numeric format specifiers in C# used for currency formatting.
decimal price = 1234.5m;

Console.WriteLine($"{price:C}");   // ₹1,234.50  (in India locale)
Console.WriteLine($"{price:C0}");  // ₹1,235     (no decimals)
Console.WriteLine($"{price:C1}");  // ₹1,234.5   (1 decimal place)
Console.WriteLine($"{price:C2}");  // ₹1,234.50  (2 decimal places)
Console.WriteLine($"{price:C3}");  // ₹1,234.500 (3 decimal places)

3) Configure Using Fluent API

Note : Gone through the docx file given 

Navigation Data loading in EF Core
-----------------------------------
In EF Core, “loading” refers to how related (navigation) data is fetched from the database.

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public ICollection<Student> Students { get; set; } = new List<Student>();
}

When you query students — how will EF Core load their Courses? That’s where the 3 types come in.

# Types 

1. Eager loading
Load related data immediately as part of the query (via Include / ThenInclude).

var students = context.Students
                      .Include(s => s.Courses)                 // collection
                      .ThenInclude(c => c.Teacher)            // nested include
                      .ToList();

SQL: single query with JOINs (or a set of joined rows).

Use Case : 

- When you know you’ll need related data immediately.
- Example: Displaying a Student with all enrolled Courses in a dashboard.

When to Avoid :

- When related data is large and not always required → it loads unnecessary data.

2. Lazy Loading

Related data is loaded automatically when first accessed, not when the main entity is fetched.

EF Core loads the related entity on demand, using a separate query each time the navigation property is 
accessed.

> Setup (You must enable it) 

Install:

Microsoft.EntityFrameworkCore.Proxies

And enable it:

protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    optionsBuilder
        .UseSqlServer(connectionString)
        .UseLazyLoadingProxies();
}


Make navigation properties virtual:

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}

E.g : 

using var context = new SchoolContext();

var students = context.Students.ToList();

foreach (var s in students)
{
    // Lazy loading triggers separate query here
    Console.WriteLine($"{s.Name} - {s.Courses.Count}");
}

SQL Generated

- First query loads Students.
- When s.Courses accessed, another query loads Courses for that student.

Use Case : 

- When related data may not always be needed.
- Example: A UI showing students first, then expands details on-demand.

When to Avoid : 

- Can cause N+1 query problem (many small SQL queries).
- Slower for large data sets.

3. Explicit Loading

You load related data manually, after the main entity is retrieved.
You explicitly call .Entry(...).Collection(...).Load() or .Reference(...).Load().

Example
using var context = new SchoolContext();

var student = context.Students.First(s => s.Id == 1);

// Load related data explicitly
context.Entry(student)
       .Collection(s => s.Courses)
       .Load();

Use Case : 

- When you need fine-grained control.
- Example: Load only related data for selected entities (not all).

When to Avoid : 

- If you’ll need related data for all entities — use eager loading instead.

Change tracking in EF Core 
---------------------------

The ChangeTracker in EF Core tracks changes made to every entity by assigning them the Entity States. It uses 
`the EntityEntry class to store the tracking information of a given entity. The Entity States represents the 
state of an entity. The Entity State can be Added, Deleted, Modified, Unchanged or Detached. For example, when 
we add a new entity to DbContext using the Add / AddRange method, the DbContext sets the state of the entity as 
Added. When we query and load the entity the state is set to Unchanged. If we make any changes to the entity 
then its state becomes Modified The SaveChanges uses these states to determine to generate the SQL query ( 
Insert,Update or Delete )

# ChangeTracker
The ChangeTracker class is responsible for keeping track of entities loaded into the Context. It does so by 
creating an EntityEntry class instance for every entity. The ChangeTracker maintains the Entity State, the 
Original Values, Current Values, etc of each entity in the EntityEntry class.

# EntityEntry 
Each entity tracked by the context gets an instance of the EntityEntry class. It stores the Change tracking 
information of the given entity and also has methods, which you can use to manipulate the Change Tracking 
Information.

Cascade Delete in EF core
-------------------------
Entity Framework Core Cascade Delete is one of the Referential actions. These actions specify what to do with 
the related rows when we delete the parent row. We can configure these actions in the database and in the EF 
core (Client).

The options available to use are Delete the related rows (Cascade / ClientCascade), Update its Foreign Key to 
NULL (SetNull / ClientSetNull) or do nothing (Restrict/ NoAction / ClientNoAction). We setup delete behavior 
between entities using the FluentAPI OnDelete method.

Note : The ClientCascade, NoAction & ClientNoAction are added in EF Core 3.0. If you were using Restrict then 
code will break. You should change it to ClientNoAction

For Example, in an SQL Server database defines the four Referential actions. They are ON DELETE CASCADE, ON 
DELETE SET NULL, ON DELETE NO ACTION & ON DELETE SET DEFAULT

When we delete a parent record, the database will take one of the following actions based on the Referential 
actions.

# Delete behaviors
The Entity Framework Core defines two sets of delete behaviors. Behaviors that maps to the database & those who 
do not. Those who do not map to the database starts with the prefix Client.

- Cascade : Delete the Child Records both in client & Database.
- ClientCascade : Delete the Child Records both in the client only.
- SetNull : Set Foreign Key as NULL in both in Client & Database.
- ClientSetNull : Set Foreign Key as NULL only in the Client
- NoAction : Default behavior on the client and No action on the database
- ClientNoAction : No action on the client and on the database
- Restrict : Same as NoAction. The migrations script will generate (Restrict or Non) instead of NoAction.

Note : Here Client is our application that is using EF core entity model..

# Setting the Delete behaviors
The EF Core defines the Delete behaviors in the DeleteBehavior enumerator. We can specify them while defining 
the relationships using the Fluent API.

The .OnDelete(DeleteBehavior.Cascade) assigns the delete Behavior to this relationship

e.g : 
modelBuilder.Entity<Employee>()
    .HasOne(e => e.Department)
    .WithMany(d => d.Employees)
    .OnDelete(DeleteBehavior.SetDefault);

# Behaviors that maps to the database
The Cascade, SetNull, NoAction & Restrict are the actions that map to database referential action.

When you use the above behaviors to configure the relationship and use EF Core Migration to create the 
database, then the following cascade behavior will be set up for you.

Action	Mapped to database

Cascade	- ON DELETE CASCADE
NoAction - ON DELETE NO ACTION
SetNull	- ON DELETE SET NULL
Restrict - 	ON DELETE RESTRICT (Not supported by SQL SERVER)

In term of EF core to SQL 

# Behaviors that do not map to the database
If you are using ClientCascade, ClientSetNull, or ClientNoAction, then Migration will not generate the 
referential actions in the database.

That is the only difference between them.

Example : 

When we delete the Department (db.Departments.Remove(dept)), depending on how we set up the DeleteBehavior, 
DbContext will do one of the following

> DeleteBehavior.Cascade/DeleteBehavior.ClientCascade
Delete the Employee Entities

> DeleteBehavior.ClientSetNull/DeleteBehavior.SetNull:
Updates the DepartmentID of Employee record to NULL

> DeleteBehavior.NoAction
Updates the DepartmentID of Employee record to NULL

> DeleteBehavior.ClientNoAction:
Does nothing.

> DeleteBehavior.Restrict:
Updates the DepartmentID of Employee record to NULL

All of the above actions happen on the Client Side. When we call SaveChanges, DBContext will send the query to 
delete the department and also delete/update the tracked employee records.

# EF Core Migrations
As we have seen it before, we will elaborate it more here.. 

We then use the add-migration command to create the migration. We can revert or remove the migration using the 
remove-migration. Once we create the migration we can push it to the database using the update-database 
command. Or we can also create the SQL script using the script-migration Once we have the script we can use it 
to update the production database. We also show you how to revert the migration from the database.

The EF Core migrations make it easier to push the changes to the database and also preserve the existing data 
in the database. It provides commands like add-migration, remove-migration to create & remove migrations. Once 
migrations are generated, we update the database using the update-database. We can also generate the migration 
script and execute it in the production server.

| **PMC Command**      | **CLI Command**                |**Remarks**       
------------------------------------------------------------------ |
| `Add-Migration`      | `dotnet ef migrations add`     | Adds a new migration. 
| `Drop-Database`      | `dotnet ef database drop`      | Drops the database.
| `Get-DbContext`      | `dotnet ef dbcontext info`     | Gets information about a `DbContext` type.
| `Remove-Migration`   | `dotnet ef migrations remove`  | Removes the last migration.
| `Scaffold-DbContext` | `dotnet ef dbcontext scaffold` | Scaffolds a `DbContext` and entity types for an 
                                                          existing database.
| `Script-Migration`   | `dotnet ef migrations script`  | Generates a SQL script from migrations.
| `Update-Database`    | `dotnet ef database update`    | Updates the database to the latest (or specified)     migration.
| `N/A`                | `dotnet ef dbcontext list`     | Lists available `DbContext` types.
| `N/A`                | `dotnet ef migrations list`    | Lists all available migrations.

Script-Migration
-----------------
Script-Migration is an EF Core command that generates a SQL script for your migrations — instead of directly 
updating the database.

It converts your EF Core migrations into the actual SQL that EF Core would run.

PMC (Package Manager Console)
cmd : 
Script-Migration [-From <MIGRATION>] [-To <MIGRATION>] [-Output <FILE_PATH>] [-Idempotent]

Example 1 : Generate SQL for all migrations
If you want SQL for all migrations (from the start):

PMC
Script-Migration

🔹 EF Core generates a SQL script for every migration from the initial one up to the latest.

Example 2: Generate SQL between two specific migrations
If you want the SQL script between two migrations, specify both names:

PMC
Script-Migration -From InitialCreate -To AddStudentsTable

🔹 EF Core will only script the difference (the SQL needed to go from InitialCreate to AddStudentsTable).

Example 3: Generate a script for a single migration
You can use -From with the previous migration and -To with the current one.

PMC
Script-Migration -From AddStudentsTable -To AddCoursesTable

Example 4: Generate Idempotent script
The --idempotent (or -Idempotent) option creates a SQL script that can be safely run on any database — it checks if migrations already exist before applying.

PMC
Script-Migration -Idempotent -Output "C:\Scripts\Deploy.sql"

✅ Use Case:
When deploying to production, where the exact DB version might vary — the script checks which migrations are missing and applies only the required ones.

Example 5: Save script to file
PMC
Script-Migration -Output "C:\Scripts\MyMigration.sql"

EF Core will create the .sql file containing all the generated commands.

# EF Core Database First. Reverse Engineering the Database (scaffolding)

The EF core only supports Code First & Database First approach. In Database First, We use the 
Scaffold-dbcontext to create the Model from an existing database. This is basically Reverse engineering the 
existing database. Once we create the entity classes databases first does not work. You will continue to work 
in the code first approach.

You will work with the new database only if you are working with a new project. In most of the scenarios, you 
may have to start with an existing database. You can use the Scaffold-dbcontext quickly create the models from the existing database.

Setup : 
Cmd : 
Install-Package Microsoft.EntityFrameworkCore.Tools
 
We use these tools inside the Visual Studio NuGet Package manager console.

Installing the above package also installs the Microsoft.EntityFrameworkCore.Design package. This package 
actually contains the command to scaffold an existing database by reverse-engineering the schema of a database.

scaffold-dbcontext
-------------------
The Scaffold-DbContext is the command is used to generate the model from the database. We need to pass the 
connection string & database provider to this command.

| **Argument / Option**   | **Remarks? Description**
--------------------------|---------------------------|
| **`-Connection`**       | ✅ **Required**. The connection string for the database you want to reverse-engineer.
| **`-Provider`**         | ✅ **Required**. Specifies the database provider, such as SQL Server, MySQL, or PostgreSQL. 
| **`-OutputDir`**        | Optional. Folder where entity class files will be placed. Path is relative to the project directory.
| **`-ContextDir`**       | Optional. Folder to place only the `DbContext` file. Path is relative to the project directory.
| **`-Context`**          | Optional. Custom name for the generated `DbContext` class. 
| **`-Schemas`**          | Optional. Restrict scaffolding to specific schemas. Useful for large databases.
| **`-Tables`**           | Optional. Restrict scaffolding to specific tables in the database.
| **`-DataAnnotations`**  | Use data annotation attributes (like `[Key]`, `[Table]`, `[ForeignKey]`) instead of only Fluent API configuration. |
| **`-UseDatabaseNames`** | Keeps the original database table and column names instead of applying EF Core naming conventions.                 |
| **`-Force`**            | Overwrites existing files in the output folders if they already exist.


Common Example
---------------

Scaffold-DbContext "Server=.;Database=SchoolDB;Trusted_Connection=True;" `
Microsoft.EntityFrameworkCore.SqlServer `
-OutputDir Models `
-ContextDir Data `
-Context SchoolDbContext `
-DataAnnotations `
-UseDatabaseNames `
-Force

🧾 What happens:

- Creates folder /Models → contains entities like Student.cs, Course.cs
- Creates folder /Data → contains SchoolDbContext.cs
- Uses [Key], [Required], etc. in model classes
- Keeps table/column names as-is from DB
- Overwrites any existing model files

⚡ Example Generated Output
🗂 Models/Student.cs

[Table("Students")]
public partial class Student
{
    [Key]
    public int Id { get; set; }

    [Column("FullName")]
    [StringLength(50)]
    public string FullName { get; set; } = null!;

    [ForeignKey("CourseId")]
    public virtual Course? Course { get; set; }
}

🗂 Data/SchoolDbContext.cs

public partial class SchoolDbContext : DbContext
{
    public SchoolDbContext(DbContextOptions<SchoolDbContext> options)
        : base(options) { }

    public virtual DbSet<Student> Students { get; set; }
    public virtual DbSet<Course> Courses { get; set; }
}

As you can see the scaffold-dbcontext

Models
------
- The models are created in the root folder. You can override it by specifying the -OutputDir as shown in our 
  example
- Creates one model class per table.
- No data annotations are applied to the models unless you specify the -DataAnnotations argument.
- You can limit the tables to use using the -Tables argument and specifying the name of the tables separated by 
  the comma.
- Similarly, you can restrict it to a certain schema using the -Schema

Context
-------
- The context class EFCoreMigrationContext is created deriving from the DBContext. The name of the context is 
  <DatabaseName>Context. You can change the name of the context class from the -Context argument
- The Context class is created in the models folder. You can override it by specifying the -ContextDir
- The context class uses the namespace of the default project.
- The OnConfiguring method is created with connection hardcoded in it.
- Fluent API used in the OnModelCreating() method of the DbContext class

What if the database changes
----------------------------
- The EF Core does not support updating the Model if the database changes. You have to delete the model and 
recreate it again

What if Model changes
----------------------
You can run add-migration to create the migrations, but you won’t be able to run update-database as the tables 
already exist.

To enable our new model to work under migrations you need to follow these steps

- Create Models by reverse-engineering the existing database as mentioned above
- Run add-migration to create the first migration
- Use the script-migration to create the script
- In the generated script look for the CREATE TABLE [__EFMigrationsHistory] query which is at the top of the   
  script and run it against your DB to create the table in the database
- Next, find out the INSERT INTO [__EFMigrationsHistory] which are at the bottom of the script and run it 
  against your DB

That’s it.