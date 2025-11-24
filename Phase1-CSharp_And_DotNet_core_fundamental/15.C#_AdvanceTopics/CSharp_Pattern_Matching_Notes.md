# 🧩 C# Pattern Matching — Notes

Pattern Matching in C# allows testing an object's type, structure, or value pattern directly — improving
readability and reducing casting or long `if-else` chains.

## 🔹 1. Basic `is` Pattern

object obj = "hello";

if (obj is string s)
{
    Console.WriteLine(s.ToUpper());
}

if (obj is int _)  // discard
{
    Console.WriteLine("It's an int");
}

---

## 🔹 2. `switch` Expression Pattern

object o = 123;

string result = o switch
{
    null => "nothing",
    int i => $"integer {i}",
    string s when s.Length > 5 => "long string",
    string s => $"short string ({s})",
    _ => "unknown"
};

✅ Cleaner than long `if-else` or `switch` statements.

---

## 🔹 3. Property Pattern

public class Person { public string Name { get; set; } = ""; public int Age { get; set; } }

Person p = new() { Name = "Asha", Age = 30 };

string category = p switch
{
    { Age: < 13 } => "child",
    { Age: >= 13 and < 20 } => "teen",
    { Age: >= 20 } => "adult",
    _ => "unknown"
};

---

## 🔹 4. Relational & Logical Patterns

int score = 85;

string grade = score switch
{
    >= 90 => "A",
    >= 80 and < 90 => "B",
    >= 70 and < 80 => "C",
    < 70 => "F",
    _ => "invalid"
};

// Logical
object x = null;
if (x is not null)
    Console.WriteLine("Not null");
---

## 🔹 5. Tuple Pattern

(int x, int y) point = (0, 5);

string pos = point switch
{
    (0, 0) => "origin",
    (_, 0) => "on X-axis",
    (0, _) => "on Y-axis",
    (var a, var b) when a > 0 && b > 0 => "Quadrant 1",
    _ => "other"
};
---

## 🔹 6. Positional Pattern

public record Point(int X, int Y);

Point pt = new(3, 4);

string desc = pt switch
{
    (0, 0) => "origin",
    (var x, 0) => $"X-axis at {x}",
    (0, var y) => $"Y-axis at {y}",
    (var x, var y) => $"point ({x},{y})"
};
---

## 🔹 7. Type + Property Pattern

object o = new Person { Name = "Ravi", Age = 17 };

string status = o switch
{
    Person { Age: < 18 } p => $"{p.Name} is a minor",
    Person { Age: >= 18 } p => $"{p.Name} is an adult",
    _ => "unknown"
};
---

## 🔹 8. `when` Guards

public decimal CalculateDiscount(Customer c) =>
    c switch
    {
        { IsVip: true } when c.TotalPurchases > 10000m => 0.20m,
        { IsVip: true } => 0.10m,
        _ => 0m
    };
---

## 🔹 9. Before vs After Example

**Before:**
if (o == null) return "null";
if (o is Order ord && ord.Total > 100) return "big order";
if (o is Customer c) return c.Name;
return "other";

**After:**
return o switch
{
    null => "null",
    Order { Total: > 100 } => "big order",
    Customer c => c.Name,
    _ => "other"
};
---

## 🔹 10. Real Use Case Example

public record Order(int Id, decimal Amount, OrderStatus Status);

public IEnumerable<Order> FilterOrders(IEnumerable<Order> orders) =>
    orders.Where(o => o switch
    {
        { Status: OrderStatus.Cancelled } => false,
        { Status: OrderStatus.Shipped, Amount: >= 100 } => true,
        { Status: OrderStatus.Processing, Amount: > 500 } => true,
        _ => false
    });
---

## 🔹 11. Best Practices

- ✅ Use pattern matching for **concise, readable logic**.
- ✅ Combine **relational** and **property patterns**.
- ⚠️ Avoid overly complex nested patterns.
- 🚀 Great for **type checks**, **data filtering**, and **switch expressions**.

---

## 🔹 12. Quick Summary Table

| Pattern Type | Example | Description |
|---------------|----------|--------------|
| **Type pattern** | `if (x is int i)` | Match type and assign |
| **Property pattern** | `{ Age: >= 18 }` | Match object properties |
| **Relational pattern** | `>= 90` | Compare numeric values |
| **Logical pattern** | `and`, `or`, `not` | Combine or negate patterns |
| **Tuple pattern** | `(x, y) =>` | Match tuples |
| **Positional pattern** | `(var x, var y)` | Match deconstructed record |
| **Guard (`when`)** | `when condition` | Add extra conditions |
| **Discard (`_`)** | `_ => "default"` | Catch-all pattern |

---

## 🧠 TL;DR

Pattern Matching makes your C# code:
- ✅ **Shorter**
- ✅ **Safer (no casting)**
- ✅ **More readable**
- ✅ **More functional**
