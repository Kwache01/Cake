using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню заказа тортов");
            Console.WriteLine("=================\n");
            Console.WriteLine("1. Выбрать торт");
            Console.WriteLine("2. История заказов");
            Console.WriteLine("Esc. Выход");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    CakeOrder();
                    break;
                case ConsoleKey.D2:
                    ShowOrderHistory();
                    break;
                case ConsoleKey.Escape:
                    Environment.Exit(0);
                    break;
            }
        }
    }

    static void CakeOrder()
    {
        Console.Clear();
        Console.WriteLine("Выберите торт:");
        Console.WriteLine("==============\n");

        List<CakeOptionGroup> cakeOptionGroups = new List<CakeOptionGroup>
        {
            new CakeOptionGroup("Форма", new List<CakeOption>
            {
                new CakeOption("Круглая", 100),
                new CakeOption("Прямоугольная", 150)
            }),
            new CakeOptionGroup("Размер", new List<CakeOption>
            {
                new CakeOption("Маленький", 200),
                new CakeOption("Средний", 300),
                new CakeOption("Большой", 400)
            }),
            new CakeOptionGroup("Вкус", new List<CakeOption>
            {
                new CakeOption("Шоколадный", 150),
                new CakeOption("Ванильный", 100),
                new CakeOption("Фруктовый", 120)
            }),
            new CakeOptionGroup("Количество коржей", new List<CakeOption>  
            {
                new CakeOption("Один", 0),
                new CakeOption("Два", 50),
                new CakeOption("Три", 100)
            }),
            new CakeOptionGroup("Глазурь", new List<CakeOption>
            {
                new CakeOption("Шоколадная", 50),
                new CakeOption("Ванильная", 50),
                new CakeOption("Фруктовая", 50)
            }),
            new CakeOptionGroup("Декор", new List<CakeOption>
            {
                new CakeOption("Цветы", 30),
                new CakeOption("Фигурки", 40),
                new CakeOption("Шоколадные стружки", 20)
            })
        };

        int totalPrice = 0;
        List<string> selectedOptions = new List<string>();
        int currentGroupIndex = 0;
        int currentOptionIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите торт:");
            Console.WriteLine("==============\n");

            CakeOptionGroup currentGroup = cakeOptionGroups[currentGroupIndex];
            Console.WriteLine($"{currentGroup.Description}:");
            Console.WriteLine("=====================\n");

            for (int i = 0; i < currentGroup.Options.Count; i++)
            {
                CakeOption option = currentGroup.Options[i];
                if (i == currentOptionIndex)
                {
                    Console.WriteLine($"> {option.Description} ({option.Price} рублей)");
                }
                else
                {
                    Console.WriteLine($"  {option.Description} ({option.Price} рублей)");
                }
            }

            Console.WriteLine("Esc. Выход");

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            Console.WriteLine();

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return;
            }

            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (currentOptionIndex > 0)
                {
                    currentOptionIndex--;
                }
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (currentOptionIndex < currentGroup.Options.Count - 1)
                {
                    currentOptionIndex++;
                }
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                CakeOption selectedOption = currentGroup.Options[currentOptionIndex];
                selectedOptions.Add(selectedOption.Description);
                totalPrice += selectedOption.Price;

                currentGroupIndex++;
                currentOptionIndex = 0;

                if (currentGroupIndex >= cakeOptionGroups.Count)
                {
                    break;
                }
            }
        }

        Console.Clear();
        Console.WriteLine("Итоговый заказ:");
        Console.WriteLine("===============");
        foreach (string option in selectedOptions)
        {
            Console.WriteLine(option);
        }
        Console.WriteLine($"Суммарная цена: {totalPrice} рублей");

        SaveOrderToHistory(selectedOptions, totalPrice);

        Console.WriteLine("\nНажмите любую клавишу для возврата в меню.");
        Console.ReadKey();
    }

    static void ShowOrderHistory()
    {
        Console.Clear();
        Console.WriteLine("История заказов:");
        Console.WriteLine("================\n");

        if (File.Exists("order_history.txt"))
        {
            string[] orders = File.ReadAllLines("order_history.txt");
            foreach (string order in orders)
            {
                Console.WriteLine(order);
            }
        }
        else
        {
            Console.WriteLine("История заказов пуста.");
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата в меню.");
        Console.ReadKey();
    }

    static void SaveOrderToHistory(List<string> options, int totalPrice)
    {
        string order = string.Join(", ", options);
        string orderWithPrice = $"{order} - {totalPrice} рублей";

        using (StreamWriter writer = new StreamWriter("order_history.txt", true))
        {
            writer.WriteLine(orderWithPrice);
        }
    }
}

class CakeOptionGroup
{
    public string Description { get; }
    public List<CakeOption> Options { get; }

    public CakeOptionGroup(string description, List<CakeOption> options)
    {
        Description = description;
        Options = options;
    }
}

class CakeOption
{
    public string Description { get; }
    public int Price { get; }

    public CakeOption(string description, int price)
    {
        Description = description;
        Price = price;
    }
}
