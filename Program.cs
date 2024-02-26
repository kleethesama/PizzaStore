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

    private void AddPrice(Item item)
    {
        Price += item.Price;
    }

    private void SubtractPrice(Item item)
    {
        Price -= item.Price;
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

    public void Start()
    {
        Pizza[] myPizzas = new Pizza[3];
        for (int i = 0; i < myPizzas.Length; i++)
        {
            myPizzas[i] = PickRandomPizza();
        }
        Console.WriteLine($"This pizza is called {myPizzas[0].Name}, it has the menu number #{myPizzas[0].MenuNumber} and its price is {myPizzas[0].Price} DKK.");
        Topping myTopping = new Topping("cheese", 10);
        Console.WriteLine($"The pizza needs extra {myTopping.Name} and it'll cost an additional {myTopping.Price} DKK.");
        myPizzas[0].AddTopping(myTopping);
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