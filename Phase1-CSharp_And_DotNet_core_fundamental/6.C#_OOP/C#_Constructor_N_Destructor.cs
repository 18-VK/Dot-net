Example obj = new Example();
obj.class_id = 2;
Example Obj2 = new Example("Hey");
Example Obj3 = new Example(obj);
obj = null;
//Example2 ObjEX2 = new Example2(); // error, because we can't create object of class with priavte constructor 

Console.WriteLine(Obj3.class_id);
class Example
{
    public int class_id;
    public Example()//default constructor
    {
        Console.WriteLine("Deafult Constructor");
    }
    public Example(string val) //Parameterize Constructor
    {
        Console.WriteLine("Parameterize Constructor, val : " + val);
    }
    public Example(Example g)
    {
        this.class_id = g.class_id;
    }
    static Example() //static Constructor
    {
        Console.WriteLine("Static Constructor");
    }
    ~Example()
    {
        Console.WriteLine("Destructor called");
    }
};

class Example2
{
    private Example2()
    {
         Console.WriteLine("private Constructor");
    }
}
