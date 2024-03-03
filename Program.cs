﻿public class Item
{
    public string Name {get; set;}
    public decimal Price {get; set;}

    public Item(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    protected void AddPrice(decimal price)
    {
        Price += price;
    }

    public override string ToString()
    {
        return $"This item's name is {Name} and its price is {Price}.";
    }
}

public class Topping : Item
{
    public Topping(string name, decimal price) : base(name, price)
    {
    }

    public override string ToString()
    {
        return $"Topping is {Name} and its additional cost is {Price}.";
    }
}

public class Pizza : Item
{
    public int MenuNumber {get; set;}
    public Topping[] PizzaTopping {get; set;}

    public Pizza(string name, decimal price, int menuNumber) : base(name, price)
    {
        MenuNumber = menuNumber;
        PizzaTopping = Array.Empty<Topping>();
    }

    public void AddTopping(Topping newTopping)
    {
        int newArrayLength = PizzaTopping.Length + 1;
        Topping[] newToppingArray = new Topping[newArrayLength];
        PizzaTopping.CopyTo(newToppingArray, 0);
        newToppingArray[^1] = newTopping;
        PizzaTopping = newToppingArray;
        AddPrice(newTopping.Price);
    }

    public void RemoveTopping(Topping undesiredTopping)
    {
        int toppingIndexLocation = Array.IndexOf(PizzaTopping, undesiredTopping);
        if (toppingIndexLocation == -1)
        {
            throw new Exception("The undesired topping was not found on the pizza.");
        }
        else
        {
            int newArrayLength = PizzaTopping.Length - 1;
            Topping[] newToppingArray = new Topping[newArrayLength];
            for (int i = 0; i < newArrayLength; i++)
            {
                if (i >= toppingIndexLocation)
                {
                    newToppingArray[i] = PizzaTopping[i + 1];
                }
                else
                {
                    newToppingArray[i] = PizzaTopping[i];
                }
            }
            PizzaTopping = newToppingArray;
            AddPrice(-undesiredTopping.Price);
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

public class Customer
{
    public string CustomerName {get;}
    public Basket CustomerBasket {get; set;}

    public Customer(string customerName)
    {
        CustomerName = customerName;
        CustomerBasket = new Basket();
    }

    /*
    The logic here is that the system doesn't necessarily know who the customer
    is before they have placed their order. In the pizzeria itself, they don't
    ask the customer for their name either. So it's only required to know the
    customer's name when the order itself has been placed.
    */
    public Customer(string customerName, Basket customerBasket)
    {
        CustomerName = customerName;
        CustomerBasket = customerBasket;
    }

    public override string ToString()
    {
        return $"Customer {CustomerName} has a basket. " + CustomerBasket.ToString();
    }
}

public class Basket
{
    public Item[] Items {get; set;}
    public decimal TotalPrice {get; set;}
    public int ItemQuantity {get; set;}

    public Basket()
    {
        Items = Array.Empty<Item>();
        TotalPrice = 0;
        ItemQuantity = 0;
    }

    public Basket(Item[] items)
    {
        Items = items;
        TotalPrice = 0;
        ItemQuantity = Items.Length;
        CalculateTotalPrice();
    }

    private decimal CalculateTaxOfItems(decimal price)
    {
        decimal danishVAT = 0.25M; // Should this somehow be a global variabel?
        return price * danishVAT;
    }

    private void CalculateTotalPrice()
    {
        if (Items.Length == 0)
        {
            throw new Exception("Can't calculate total price since this basket is empty.");
        }
        else
        {
            decimal newTotalPrice = 0;
            foreach (Item item in Items)
            {
                newTotalPrice += item.Price;
            }
            TotalPrice = newTotalPrice;
        }
    }

    public void AddItem(Item item)
    {
        Item[] newItems = new Item[Items.Length + 1];
        Items.CopyTo(newItems, 0);
        newItems[^1] = item;
        Items = newItems;
        ItemQuantity = newItems.Length;
        CalculateTotalPrice();
    }

    public void RemoveItem(Item item)
    {
        int itemIndexLocation = Array.IndexOf(Items, item);
        if (itemIndexLocation == -1)
        {
            throw new Exception("The undesired item was not found in the basket.");
        }
        else
        {
            int newArrayLength = Items.Length - 1;
            Item[] newItemArray = new Item[newArrayLength];
            for (int i = 0; i < newArrayLength; i++)
            {
                if (i >= itemIndexLocation)
                {
                    newItemArray[i] = Items[i + 1];
                }
                else
                {
                    newItemArray[i] = Items[i];
                }
            }
            Items = newItemArray;
            ItemQuantity = newItemArray.Length;
            CalculateTotalPrice();
        }
    }

    public override string ToString()
    {
        if (ItemQuantity == 0)
        {
            return "This basket contains no items.";
        }
        else
        {
            string finalString = $"This basket contains {ItemQuantity} items:";
            foreach (Item item in Items)
            {
                finalString += "\n" + item.Name + $" - {item.Price} DKK.";
            }
            finalString += $"\n\nSubtotal: {TotalPrice} DKK.";
            finalString += $"\nVAT 25%: {CalculateTaxOfItems(TotalPrice)} DKK.";
            return finalString;
        }
    }
}

public class Order
{
    public int OrderNumber {get;}
    public string CustomerName {get;}
    public DateTime TimeOrderPlaced {get;}
    public bool IsOrderCompleted {get; set;}
    public Item[] Items {get;}
    public int ItemQuantity {get;}
    public decimal TotalPrice {get;}

    public Order(Customer customer, int orderNumber)
    {
        OrderNumber = orderNumber;
        CustomerName = customer.CustomerName;
        TimeOrderPlaced = DateTime.Now;
        IsOrderCompleted = false;
        Items = customer.CustomerBasket.Items;
        ItemQuantity = customer.CustomerBasket.ItemQuantity;
        TotalPrice = customer.CustomerBasket.TotalPrice;
    }

    public int GetMinutesSinceOrderPlaced()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan timeDifference = currentTime - TimeOrderPlaced;
        return timeDifference.Minutes;
    }

    // This method would be more fitting in an OrderManager.
    // public void CompleteOrder()
    // {
    //     IsOrderCompleted = true;
    // }

    public override string ToString()
    {
        string finalString = $"This order was placed {GetMinutesSinceOrderPlaced()} minutes ago:";
        foreach (Item item in Items)
        {
            if (item.GetType() == typeof(Pizza))
            {
                Pizza currentItem = (Pizza) item;
                finalString += $"\n#{currentItem.MenuNumber} {currentItem.Name} - {currentItem.Price} DKK.";
            }
            else
            {
                finalString += $"\n{item.Name} - {item.Price} DKK.";
            }
        }
        return finalString;
    }
}

public class Store
{
    public string Name {get; set;}
    public Dictionary<string, decimal> PizzasAndPrices {get; set;}

    public Store(string name, string[] pizzaNames, decimal[] pizzaPrices)
    {
        Name = name;
        Dictionary<string, decimal> pizzasDict = new Dictionary<string, decimal>();
        for (int i = 0; i < pizzaNames.Length; i++)
        {
            pizzasDict.Add(pizzaNames[i], pizzaPrices[i]);
        }
        PizzasAndPrices = pizzasDict;
    }

    private string[] GetAllPizzaNames()
    {
        int pizzaTypeAmount = PizzasAndPrices.Keys.Count;
        string[] pizzaNames = new string[pizzaTypeAmount];
        PizzasAndPrices.Keys.CopyTo(pizzaNames, 0);
        return pizzaNames;
    }

    private Pizza PickRandomPizza()
    {
        string[] pizzaNames = GetAllPizzaNames();
        int pizzaTypeAmount = PizzasAndPrices.Keys.Count;
        Random randomizer = new Random();
        int randomNumber = randomizer.Next(0, pizzaTypeAmount);
        string randomPizza = pizzaNames[randomNumber];
        decimal priceOfRandom = PizzasAndPrices[randomPizza];
        return new Pizza(randomPizza, priceOfRandom, randomNumber + 1);
    }

    private Pizza[] GeneratePizzas(int pizzaAmount)
    {
        Pizza[] myPizzas = new Pizza[pizzaAmount];
        for (int i = 0; i < pizzaAmount; i++)
        {
            myPizzas[i] = PickRandomPizza();
        }
        return myPizzas;
    }

    static void WriteItemsInfo(Item[] items)
    {
        foreach (Item item in items)
        {
            Console.WriteLine(item);
        }
    }

    public void Start()
    {
        Pizza[] myPizzas = GeneratePizzas(3);
        string[] toppingNames = {"cheese", "pepperoni", "pineapple"};
        foreach (string toppingName in toppingNames)
        {
            myPizzas[0].AddTopping(new Topping(toppingName, 10));
        }
        Basket myBasket = new Basket(myPizzas);
        myBasket.AddItem(new Item("Cola", 20));
        myBasket.AddItem(new Item("Fanta", 25));
        // Console.WriteLine(myBasket);
        Customer customerJens = new Customer("Jens");
        customerJens.CustomerBasket = myBasket;
        Console.WriteLine(customerJens);
        // Order myOrder = new Order(myBasket, "Jens", 0);
        // Console.WriteLine(myOrder);
        // Customer[] customers;
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
        decimal[] decimalPizzaPrices = new decimal[pizzaPrices.Length];
        for (int i = 0; i < pizzaPrices.Length; i++)
        {
            decimalPizzaPrices[i] = (decimal) pizzaPrices[i];
        }
        Store BigMamma = new Store("Big Mamma", pizzaNames, decimalPizzaPrices);
        BigMamma.Start();
    }
}