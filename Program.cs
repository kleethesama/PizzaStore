public class Item
{
    public string Name {get; set;}
    public int Price {get; set;}

    public Item(string name, int price)
    {
        Name = name;
        Price = price;
    }
}

public class Topping : Item
{
    public Topping(string name, int price) : base(name, price)
    {
    }
}

// public class Pizza
// {
//     public int MenuNumber {get; set;}
//     public Topping[] topping {get; set;}
// }

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("test");
    }
}