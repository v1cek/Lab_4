using System;
using System.Collections.Generic;
using System.Linq;

// Клас "Товар"
class Product
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public double Rating { get; set; }

    public Product(string name, double price, string description, string category, double rating)
    {
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Rating = rating;
    }

    public void ShowInfo()
    {
        Console.WriteLine($"Назва: {Name}, Ціна: {Price}, Категорія: {Category}, Рейтинг: {Rating}");
        Console.WriteLine($"Опис: {Description}");
    }
}

// Клас "Користувач"
class User
{
    public string Login { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; set; } = new List<Order>();

    public User(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public void ShowPurchaseHistory()
    {
        if (PurchaseHistory.Count == 0)
        {
            Console.WriteLine("Історія покупок порожня.");
            return;
        }

        Console.WriteLine("Історія покупок:");
        foreach (var order in PurchaseHistory)
        {
            order.ShowDetails();
        }
    }
}

// Клас "Замовлення"
class Order
{
    public List<Product> Products { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; private set; }
    public string Status { get; set; }

    public Order(List<Product> products, int quantity, string status)
    {
        Products = products;
        Quantity = quantity;
        Status = status;
        TotalPrice = Products.Sum(p => p.Price) * quantity;
    }

    public void ShowDetails()
    {
        Console.WriteLine($"Замовлення: Статус={Status}, Загальна вартість={TotalPrice}");
        Console.WriteLine("Товари:");
        foreach (var product in Products)
        {
            product.ShowInfo();
        }
    }
}

// Інтерфейс "ISearchable"
interface ISearchable
{
    List<Product> SearchByPrice(double minPrice, double maxPrice);
    List<Product> SearchByCategory(string category);
    List<Product> SearchByRating(double minRating);
}

// Клас "Магазин"
class Store : ISearchable
{
    public List<Product> Products { get; set; } = new List<Product>();
    public List<User> Users { get; set; } = new List<User>();

    public void AddProduct(Product product)
    {
        Products.Add(product);
        Console.WriteLine($"Товар '{product.Name}' додано до магазину.");
    }

    public void RegisterUser(User user)
    {
        Users.Add(user);
        Console.WriteLine($"Користувача '{user.Login}' зареєстровано.");
    }

    public List<Product> SearchByPrice(double minPrice, double maxPrice)
    {
        return Products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    public List<Product> SearchByRating(double minRating)
    {
        return Products.Where(p => p.Rating >= minRating).ToList();
    }

    public void ShowAllProducts()
    {
        foreach (var product in Products)
        {
            product.ShowInfo();
        }
    }
}

// Тестуємо
class Program
{
    static void Main()
    {
        // створюємо магазин
        var store = new Store();

        // додаємо товари
        store.AddProduct(new Product("Ноутбук", 1000, "Потужний ноутбук для роботи.", "Електроніка", 4.8));
        store.AddProduct(new Product("Мікрохвильовка", 200, "Компактна мікрохвильова піч.", "Побутова техніка", 4.5));
        store.AddProduct(new Product("Крісло", 150, "Зручне офісне крісло.", "Меблі", 4.2));

        // реєструємо користувача
        var user = new User("admin", "12345");
        store.RegisterUser(user);

        // пошук товарів
        Console.WriteLine("\n--- Пошук за ціною (100-500) ---");
        var cheapProducts = store.SearchByPrice(100, 500);
        foreach (var product in cheapProducts)
        {
            product.ShowInfo();
        }

        Console.WriteLine("\n--- Пошук за категорією 'Електроніка' ---");
        var electronics = store.SearchByCategory("Електроніка");
        foreach (var product in electronics)
        {
            product.ShowInfo();
        }

        // створення замовлення
        Console.WriteLine("\n--- Створення замовлення ---");
        var order = new Order(new List<Product> { cheapProducts[0] }, 2, "У процесі");
        user.PurchaseHistory.Add(order);
        user.ShowPurchaseHistory();
    }
}
