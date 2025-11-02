As we have seen that we can use LINQ with in-Memory collection and with XML/JSON
There are one for use case of it, that is use for querying relational DB. 

ORM (Object–Relational Mapping)
-------------------------------
ORM (Object–Relational Mapping) is a technique that maps objects in your programming language (like C#classes) 
to tables in a relational database (like SQL Server, MySQL, PostgreSQL, etc.).

It allows you to work with database data as regular C# objects, instead of manually writing SQL queries.

| **In C# (Object world)** | **In Database (Relational world)** |
| ------------------------ | ---------------------------------- |
| Class                    | Table                              |
| Object                   | Row (record)                       |
| Property                 | Column                             |
| Object Reference         | Foreign Key relationship           |
| Collection (List<T>)     | One-to-many relation               |

An ORM maps C# classes (entities) to database tables and automates:

- SQL generation for queries and persistence
- Change tracking & identity resolution
- Relationship management and object graphs

EF Core is a modern, cross-platform ORM that integrates well with ASP.NET Core and supports LINQ queries.

Why EF Core
------------
- Strong LINQ support (compile-time checked queries)
- Works with SQL Server, PostgreSQL, MySQL, SQLite, etc. via providers
- Migrations support (schema evolution)
- Good performance for typical app scenarios and lots of optimization knobs

| ✅ Advantages                             | ❌ Disadvantages                              |
| ---------------------------------------- | -------------------------------------------- |
| No manual SQL writing (less boilerplate) | Slower for very complex queries              |
| Strongly typed (compile-time safety)     | May generate inefficient SQL                 |
| Easier to maintain and test              | Requires understanding of ORM behavior       |
| Built-in transaction & change tracking   | Harder to tune for performance-critical apps |
| Cross-database support                   | Debugging SQL sometimes harder               |

| Term                | Meaning                                                                  |
| ------------------- | ------------------------------------------------------------------------ |
| **Entity**          | A C# class mapped to a database table.                                   |
| **DbContext**       | Manages database connection & CRUD operations.                           |
| **DbSet<TEntity>**  | Represents a table in the database.                                      |
| **Migration**       | A versioned change in the database schema created from C# model changes. |
| **Change Tracking** | Keeps track of which entities were modified, added, or deleted.          |
| **Lazy Loading**    | Loads related data only when accessed.                                   |
| **Eager Loading**   | Loads related data immediately (using `.Include()`).                     |


When NOT to use ORM
-------------------
- When performance is extremely critical (real-time systems).
- When queries are highly optimized or database-specific (e.g., stored procedures, heavy reporting).
- When you need fine-grained SQL control.

In such cases, use Dapper or raw SQL with ADO.NET.

Install EF Core to use in Application
--------------------------------------
Link : https://www.tektutorialshub.com/entity-framework-core/ef-core-console-application/

The Microsoft.EntityFrameworkCore is the core library. But installing that alone is not sufficient. We also 
need to install the EF Core database provider(s). There are many different database Providers currently 
available with the EF Core. 

For Example, to use the SQL Server, we need to install the Microsoft.EntityFrameworkCore.SqlServer. For SQLite 
install the Microsoft.EntityFrameworkCore.Sqlite. When we install the database provider(s), they automatically 
install the  Microsoft.EntityFrameworkCore.

Note : we can add it through NuGet Package Manager

Modeling the database
----------------------
EF Core performs data access using a model. A model is nothing but a POCO class. In EF Core we call them entity 
class.

The EF Core maps these entity classes to a table in the database.

Now, let us create a Product entity class

- Right-click on the solutions folder and create the folder Models
- Under the Models folder right click and create the class Product
- Change the class to the public and add two properties id & name as shown below

E.g : 

namespace EFCore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
Note : 
- The class must be declared as public
- Static classes are not allowed. The EF must be able to create an instance of the class
- The Class should not have any constructor with a parameter. The parameterless constructor is allowed

# The DBContext class

The DBContext class manages the entity classes or models. It is the heart of the Entity Framework Core. This class is responsible for

- Connecting to the database
- Querying & Updating the database
- Hold the Information needed to configure the database etc.

We create our own Context class by inheriting from the DBContext.

Under the model folder, create the EFContext class and copy the following code.
Example : 
 
using Microsoft.EntityFrameworkCore;
 
namespace EFCore.Models
{
    public class EFContext : DbContext
    {
        private const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=EFCore;Trusted_Connection=True;";
 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<Product> Products { get; set; }
    }
}

# DbSet

Creating the Model (Entity Type) is not sufficient to map it to the database. We must create a DbSet property 
for each Model in the context class. EF Core includes only those types, which have a DbSet property in the 
model.

The DBSet Provides methods like Add, Attach, remove, etc on the Entity Types. The Context class maps these 
operations into a SQL query and runs it against the database using the Database Providers.

e.g : 
public DbSet<Product> Products { get; set; }

Creating the database
----------------------
Now, our model is ready. The model has one entity type Product. We have created EFContext class to manage the 
Model. We have defined the DbSet Property of the Product so that it is included in the Model. Now it is time to 
create the database.

In Entity Framework Core, we use Migrations to create the database.

# Steps to create database

1) Adding Migration
Click on Tools – > NuGet Package Manager > Package Manager Console to open the Console

- Nuget Package Manager Console
- Run the Add-Migration to create the migration.

cmd :  Add-Migration “NewDatabase”

The Add-Migration generates the instructions to generate the SQL commands to update the underlying database to 
match the model. You can see that the three files are created and added under the Migrations folder.

2) The next step is to create the database using the Migrations from the previous step.

- Open the Package Manager Console and run the command Update-Database.
- The update-database uses the migrations to generate the SQL Queries to update the database. If the database 
does not exist it will create it. It uses the connection string provided while configuring the DBContext to
connect to the database.

now check the database name given in connection string, it will have new tables and all the we defined in model

Note : Up() → creates tables/columns/constraints

Down() → removes tables/columns/constraints (used when rolling back)

CRUD Operations
---------------
Let us now add simple create/read/update & delete operations on the Product Entity model and persist the data 
to the database.

# Inserting Data
Inserting to the database is handled by the SaveChanges method of the DBContext object.  To do that, you need 
to follow these steps.

- Create a new instance of the DbContext class.
using (var db = new EFContext())
 
- Create a new instance of the domain class Product and assign values to its properties.

Product product = new Product();
product.Name = "Pen Drive";

- Next, add it to the DbContext class so that Context becomes aware of the entity.

db.Add(product); or db.products.Add(Product)
 
- Finally, call the saveChanges method of the DBContext to persist the changes to the database. 

db.SaveChanges();

e.g : 
using (var db = new EFContext())
{
    // 1️⃣ Create a new entity
    var product = new Product
    {
        Name = "Laptop",
        Price = 75000
    };

    // 2️⃣ Add to DbSet
    db.Products.Add(product);      // Or AddAsync(product) in async code

    // 3️⃣ Save to database
    db.SaveChanges();              // Or await context.SaveChangesAsync()
    
    Console.WriteLine($"Product inserted with ID: {product.Id}");
}

# Querying the Data
The queries are written against the Dbset property of the entity.  The queries are written using the LINQ to 
Entities API. There are two ways in which you can write queries in LINQ. One is method syntax & the other one 
is Query Syntax.

The following example code retrieves the Product using the Method Syntax. It uses the ToList() method of the 
DbSet. The ToList() sends the select query to the database and retrieves and convert the result into a List of 
Products as shown below. 

e.g :  
using System.Linq;
 
static void readProduct()
{
 
    using (var db = new EFContext())
    {
        // read all 
        List<Product> products = db.Products.ToList();
        foreach (Product p in products)
        {
            Console.WriteLine("{0} {1}", p.Id, p.Name);
        }
    }
    return;
}

# Update the Record
The following code shows how to update a single entity.

First, we use the find method to retrieve the single Product. The find method takes the id (primary key) of the 
product as the argument and retrieves the product from the database and maps it into the Product entity.

Next, we update the Product entity.

Finally, we call SaveChanges to update the database
 
static void updateProduct()
{
    using (var db = new EFContext())
    {
        Product product = db.Products.Find(1); // or can use where
        product.Name = "Better Pen Drive";
        db.SaveChanges();
    }
    return;
}
 
# Delete the Record
The following code demonstrates how to delete the record from the database.

Deleting is done using the Remove method of the DbSet. We need to pass the entity to delete as the argument to 
the remove method as shown below
 
static void deleteProduct()
{
    using (var db = new EFContext())
    {
 
        Product product = db.Products.Find(1);
        if (product !=null) 
        {   
            db.Products.Remove(product);
            db.SaveChanges();
        }
    }
    return;
}

Note : to delete first we have to get the entity then call remove.. 


# Primary Key Convention

Entity Framework Core does not allow you to create the tables without the primary key. It follows the
convention to identify the candidate for the Primary Key. It searches any property with the name ID or 
<className>ID and uses it as Primary Key.

If the model contains both id & <className>ID columns, then id is chosen as Primary key

If you do not specify the primary key, then the add-migration will throw an error. For Example, try to remove 
the id field from the customerAddress table and check. You will get the following error

# Foreign Key Convention
In relational databases, data is divided between related tables. The relationship between these tables defined 
using the foreign keys. if you are looking for an employee working in a particular department, then you need to 
specify the relationship between employees and department table.

- One to many Relationship
An Employee belongs to a Department. A Department can have many employees. This is a One to many relationships. 
This relationship is captured by adding the reference to the Department table in Employee model as shown below

Let us create models to handle

public class Employee
{
  public uint EmployeeID { get; set; }
  public string EmployeeName { get; set; }
 
  public Department Department { get; set; }
}
public class Department
{
  public int DepartmentID { get; set; }
  public string DepartmentName { get; set; }
}
 
The one to many relations can also be specified in another way. For Example in the following model we just 
removed the department property from Employee table and moved it as the collection of employees in the department table

 
public class Employee
{
  public uint EmployeeID { get; set; }
  public string EmployeeName { get; set; }
}
 
public class Department
{
  public int DepartmentID { get; set; }
  public string DepartmentName { get; set; }
 
  public ICollection<Employee> Employeees { get; set; }
}
 
Note : This will result in the same database structure.

What is the Foreign Key?
------------------------
In Entity Framework (EF) Core, when you have a navigation property like:

public Department Department { get; set; }

and you don’t explicitly define a foreign key, EF automatically adds a shadow foreign key named by convention:

DepartmentDepartmentID

That’s because it combines:
Navigation property name (Department)
Primary key (DepartmentID from Department class)

✅ Therefore:

The foreign key in the Employee table will be:

DepartmentDepartmentID

💡 But Best Practice:

You should explicitly define it for clarity and control, like this:

public class Employee
{
    public uint EmployeeID { get; set; }
    public string EmployeeName { get; set; }

    public int DepartmentID { get; set; }          // Explicit foreign key
    public Department Department { get; set; }     // Navigation property
}


Now, EF knows exactly that:

Employee.DepartmentID → references → Department.DepartmentID

- Many-to-Many in EF Core

Many-to-Many occurs when one entity can relate to multiple others, and vice versa.
Classic example: Student ↔ Course (a student can enroll in many courses, a course can have many students).
EF Core 5+ supports implicit join tables automatically without creating a separate entity class (simpler).

e.g : 
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
Inserting data into a Many-to-Many relationship : 

using (var context = new SchoolContext())
{
    var student = new Student { Name = "Aman" };
    var course1 = new Course { Title = "C# Basics" };
    var course2 = new Course { Title = "EF Core" };

    // Establish relationship
    student.Courses.Add(course1);
    student.Courses.Add(course2);

    context.Students.Add(student);
    context.SaveChanges();
}

Explanation  : 

What happens under the hood 
When you call context.Students.Add(student);, EF Core inspects the whole object graph (the student and its related entities).

Here’s the sequence:

- Student (Aman) → EF marks as Added.
- Course (C# Basics & EF Core) → EF marks them as Added (because they’re new objects not tracked by the context).
- Relationship (many-to-many) → EF creates entries in the join table automatically.
- SaveChanges() runs 3 SQL operations:

INSERT INTO Students (Name) VALUES ('Aman');
INSERT INTO Courses (Title) VALUES ('C# Basics');
INSERT INTO Courses (Title) VALUES ('EF Core');
INSERT INTO StudentCourse (StudentsId, CoursesId) VALUES (1, 1);
INSERT INTO StudentCourse (StudentsId, CoursesId) VALUES (1, 2);

✅ So yes — EF Core will also insert new records into the Courses table (and the join table), because those course1 and course2 entities are new.


EF Core automatically populates the join table.

# Implicit Join Table (EF Core 5+)

EF Core automatically creates a hidden join table for you.
Use this when you have a plain many-to-many relationship with no extra data on the relationship.

For above many to many relationship example : 

- EF Core creates three tables: Students, Courses, and implicit join table CourseStudent.
- Join table has two FKs: StudentId + CourseId.

If you want to control the table name or column names, you can set it in OnModelCreating:

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Student>()
        .HasMany(s => s.Courses)
        .WithMany(c => c.Students)
        .UsingEntity(j => j.ToTable("Enrollments")); // configure join table name
}
Note : We'll breakdown above function, In specific topic called 'Configuration', for now, we can simple say, it 
is defining many-to-many relationship and renaming automatically created join table as "Enrollments"
       
Pros
- Very concise — less code.
- EF Core handles join table lifecycle and most queries are simple.
- Good for pure many-to-many with no additional columns.

Cons
- Cannot store extra fields (e.g., EnrolledOn, Grade) on the relationship.
- Harder to reference the join row as a first-class entity for auditing or business logic.

# Explicit join table (explicit join entity class)

Use this when the relationship has payload (extra properties) or you want to treat the join as an entity (audit 
fields, soft delete, status, etc.).

C# classes (explicit join entity Enrollment)
public class Student
{
    public int StudentId { get; set; }
    public string Name { get; set; } = "";

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
}

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; } = "";

    public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
}

public class Enrollment
{
    // composite key: StudentId + CourseId (recommended)
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    // payload
    public DateTime EnrolledOn { get; set; }
    public string? Grade { get; set; }
}

Fluent API (configure composite key + relationships)

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Enrollment>()
        .HasKey(e => new { e.StudentId, e.CourseId }); // composite PK

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Student)
        .WithMany(s => s.Enrollments)
        .HasForeignKey(e => e.StudentId);

    modelBuilder.Entity<Enrollment>()
        .HasOne(e => e.Course)
        .WithMany(c => c.Enrollments)
        .HasForeignKey(e => e.CourseId);
}

Pros
- You can store metadata (timestamps, grades, status).
- The join is a first-class entity: easier to query, validate, and audit.
- You can add indexes, constraints and business logic directly on the join.

Cons
- More boilerplate code.
- Slightly more verbose queries (you may need to include the join entity in LINQ).

Note : implicit and explicit join tables apply only to many-to-many relationships.

Because for one-to-one or one-to-many relationship, A simple FK column is enough.

Indexes
--------
Indexes for Foreign key is automatically created by the EF Core

Can create Explicitly : 

1) Using Fluent API (Recommended Way)

public class Employee
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = "";

    public string Email { get; set; } = "";
}

Now define index in OnModelCreating() inside your DbContext:

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Create a regular index
    modelBuilder.Entity<Employee>()
        .HasIndex(e => e.Email);

    // Create a unique index
    modelBuilder.Entity<Employee>()
        .HasIndex(e => e.EmployeeCode)
        .IsUnique();
}

Composite index : 

If you frequently query multiple columns together:

modelBuilder.Entity<Employee>()
    .HasIndex(e => new { e.EmployeeCode, e.Email });

2) Using Data Annotation (since EF Core 5.0+)

You can also use [Index] attribute.

[Index(nameof(Email))]
[Index(nameof(EmployeeCode), IsUnique = true)]
public class Employee
{
    public int Id { get; set; }
    public string EmployeeCode { get; set; } = "";
    public string Email { get; set; } = "";
}
