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
    public Topping[] PizzaTopping {get; set;}

    public Pizza(string name, int price) : base(name, price)
    {
        PizzaTopping = new Topping[1];
    }

    public Pizza(string name, int price, int menuNumber) : this(name, price)
    {
        MenuNumber = menuNumber;
    }

    private void AddPrice(Topping newTopping)
    {
        Price += newTopping.Price;
    }

    private void SubtractPrice(Topping newTopping)
    {
        Price -= newTopping.Price;
    }

    public void AddTopping(Topping newTopping)
    {
        int newArrayLength = PizzaTopping.Length + 1;
        Topping[] newToppingArray = new Topping[newArrayLength];
        PizzaTopping.CopyTo(newToppingArray, 0);
        newToppingArray[newArrayLength - 2] = newTopping;
        PizzaTopping = newToppingArray;
        AddPrice(newTopping);
    }

    public void RemoveTopping(Topping undesiredTopping)
    {
        int undesiredToppingIndex = Array.IndexOf(PizzaTopping, undesiredTopping);
        if (undesiredToppingIndex == -1)
        {
            // TODO: Make this into a new type of exception.
            Console.WriteLine("Could not find the PizzaTopping to be removed!");
        }
        else
        {
            int newArrayLength = PizzaTopping.Length - 1;
            Topping[] newToppingArray = new Topping[newArrayLength];
            for (int i = 0; i < newArrayLength; i++)
            {
                if (i == undesiredToppingIndex)
                {
                    i--;
                    continue;
                }
                newToppingArray[i] = PizzaTopping[i];
            }
            PizzaTopping = newToppingArray;
            SubtractPrice(undesiredTopping);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("test");
    }
}