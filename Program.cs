public class Item
{
    public string Name {get; set;}
    public int Price {get; set;}

    public Item(string name, int price)
    {
        Name = name;
        Price = price;
    }

    protected void AddPrice(Item item)
    {
        Price += item.Price;
    }

    protected void AddPrice(int price)
    {
        Price += price;
    }

    protected void SubtractPrice(Item item)
    {
        Price -= item.Price;
    }

    protected void SubtractPrice(int price)
    {
        Price -= price;
    }

    public override string ToString()
    {
        return $"This item's name is {Name} and its price is {Price}.";
    }
}

public class Topping : Item
{
    public Topping(string name, int price) : base(name, price)
    {
    }

    public override string ToString()
    {
        return $"This topping is {Name} and its additional cost is {Price}.";
    }
}

public class Pizza : Item
{
    public int MenuNumber {get; set;}
    public Topping[] PizzaTopping {get; set;}

    public Pizza(string name, int price) : base(name, price)
    {
        PizzaTopping = new Topping[0];
    }

    public Pizza(string name, int price, int menuNumber) : this(name, price)
    {
        MenuNumber = menuNumber;
    }

    public void AddTopping(Topping newTopping)
    {
        int newArrayLength = PizzaTopping.Length + 1;
        Topping[] newToppingArray = new Topping[newArrayLength];
        PizzaTopping.CopyTo(newToppingArray, 0);
        newToppingArray[newArrayLength - 1] = newTopping;
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

    public override string ToString()
    {
        string[] pizzaInfo = {$"This pizza is called {Name}, ",
                              $"it has the menu number #{MenuNumber} ",
                              $"and its price is {Price} DKK."};
        string finalString = "";
        foreach (string info in pizzaInfo)
        {
            finalString += info;
        }
        if (PizzaTopping.Length == 0)
        {
            finalString += $"\nIt has no extra topping.";
        }
        else
        {
            foreach (Topping topping in PizzaTopping)
            {
                finalString += $"\nIt has extra {topping.Name} as topping.";
            }
        }
        return finalString;
    }
}

public class Store
{
    public string Name {get; set;}
    public Dictionary<string, int> PizzasAndPrices {get; set;}

    public Store(string name, string[] pizzaNames, int[] pizzaPrices)
    {
        Name = name;
        Dictionary<string, int> pizzasDict = new Dictionary<string, int>();
        for (int i = 0; i < pizzaNames.Length; i++)
        {
            pizzasDict.Add(pizzaNames[i], pizzaPrices[i]);
        }
        PizzasAndPrices = pizzasDict;
    }

    public string[] GetAllPizzaNames()
    {
        int pizzaTypeAmount = PizzasAndPrices.Keys.Count;
        string[] pizzaNames = new string[pizzaTypeAmount];
        PizzasAndPrices.Keys.CopyTo(pizzaNames, 0);
        return pizzaNames;
    }

    public Pizza PickRandomPizza()
    {
        string[] pizzaNames = GetAllPizzaNames();
        int pizzaTypeAmount = PizzasAndPrices.Keys.Count;
        Random randomizer = new Random();
        int randomNumber = randomizer.Next(0, pizzaTypeAmount);
        string randomPizza = pizzaNames[randomNumber];
        int priceOfRandom = PizzasAndPrices[randomPizza];
        return new Pizza(randomPizza, priceOfRandom, randomNumber + 1);
    }

    private void TestPizzas(Pizza[] myPizzas)
    {
        foreach (Pizza pizza in myPizzas)
        {
            Console.WriteLine(pizza.ToString());
        }
    }

    public void Start()
    {
        Pizza[] myPizzas = new Pizza[3];
        for (int i = 0; i < myPizzas.Length; i++)
        {
            myPizzas[i] = PickRandomPizza();
        }
        Topping myTopping = new Topping("cheese", 10);
        myPizzas[0].AddTopping(myTopping);
        TestPizzas(myPizzas);
        myPizzas[0].RemoveTopping(myTopping);
        Console.WriteLine("\nBEFORE AND AFTER\n");
        TestPizzas(myPizzas);
        // Customer[] customers;
        // Order[] orders;
    }
}

class Program
{
    static void Main(string[] args)
    {
        string[] pizzaNames = {"Margherita", "Vesuvio", "Capricciosa", "Calzone",
                               "Quattro Stagioni", "Marinara", "Vegetariana", "Italiana",
                               "Gorgonzola", "Contadina","Napoli","Vichinga",
                               "Calzone Speciale", "Esotica", "Tonno", "Sardegna",
                               "Romana", "Sole", "Big Mamma", "La salami",
                               "Rocco", "Marco", "KoKKode", "Antonello",
                               "Pasqualino", "Felix", "Bambino"};
        int[] pizzaPrices   = {80, 92, 98, 98,
                               98, 97, 98, 93,
                               97, 92, 95, 98,
                               98, 98, 97, 97,
                               98, 98, 99, 98,
                               99, 99, 99, 99,
                               98, 95, 65};
        Store BigMamma = new Store("Big Mamma", pizzaNames, pizzaPrices);
        BigMamma.Start();
    }
}