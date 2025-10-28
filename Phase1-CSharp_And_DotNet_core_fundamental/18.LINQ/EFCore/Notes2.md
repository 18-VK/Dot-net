Configuring the Model
--------------------
1) Configure Using Conventions 

EF Core Conventions or the default rules that you follow while creating the entity model. The EF Core uses these to 
infer and to configure the Database.   It uses the information available in the POCO Classes to determine and infer the 
schema of the database that these classes are mapped to. For example, the table name, Column Name, Data Type, Primary 
keys are inferred from the  Class name, property name & Property type by convention to build the database.

2) Configure Using Data Annotations

Data Annotations allow us the configure the model classes by adding metadata to the class and also the class 
properties. The Entity Framework Core (EF Core) recognizes these attributes and uses them to configure the models. We 
have seen how to use Entity Framework Core Convention to configure our models in our previous tutorials. The 
conventions have their limits in their functionalities. Data Annotations in Entity Framework Core allow us to further 
fine-tune the model. They override the conventions.

What is Data Annotation : 

Data Annotations are the attributes that we apply to the class or on the properties of the class. They provide 
additional metadata about the class or its properties. These attributes are not specific to Entity Framework Core. They 
are part of a larger .NET  /.NET Core Framework. The ASP.NET MVC or ASP.NET MVC Core Applications also uses these 
attributes to validate the model.

The Data annotation attributes falls into two groups depending functionality provided by them.

- Data Modeling Attributes
- Validation Related Attributes

# Data Modelling Attributes
Data Modeling Attributes specify the schema of the database. These attributes are present in the namespace  
System.ComponentModel.DataAnnotations.Schema.The following is the list of attributes are present in the namespace.

# Validation Related Attributes
Validation related Attributes reside in the  System.ComponentModel.DataAnnotations namespace. We use these attributes 
to enforce validation rules for the entity properties. These attributes also define the size, nullability & Primary 
key, etc of the

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
