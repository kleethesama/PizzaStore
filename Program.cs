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

public class Pizza : Item
{
    public int MenuNumber {get; set;}
    public Topping[] topping {get; set;}

    public Pizza(string name, int price) : base(name, price)
    {
        topping = new Topping[1];
    }

    public Pizza(string name, int price, int menuNumber) : this(name, price)
    {
        MenuNumber = menuNumber;
    }

    public void AddTopping(Topping topping)
    {
        Topping[] newToppingArray = new Topping[this.topping.Length + 1];
        this.topping.CopyTo(newToppingArray, 0);
        newToppingArray[newToppingArray.Length - 1] = topping;
        this.topping = newToppingArray;
    }

    public void CalculatePrice()
    {
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("test");
    }
}