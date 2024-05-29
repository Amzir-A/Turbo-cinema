using Spectre.Console;
using System;
using System.Collections.Generic;

public static class FoodAndDrinksScreen
{
    public static List<(string, int, decimal)> SelectedItems = new List<(string, int, decimal)>();

    public static void Show()
    {
        SelectedItems.Clear();

        var items = new List<(string, decimal)>
        {
            ("Popcorn & Frisdrank naar keuze (S)", 3.00m),
            ("Popcorn & Frisdrank naar keuze (M)", 6.00m),
            ("Popcorn & Frisdrank naar keuze (L)", 9.00m)
        };

        var itemQuantities = new int[items.Count];
        int selectedIndex = 0;

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Eten & Drinken").Centered().Color(Color.Red));

            for (int i = 0; i < items.Count; i++)
            {
                string prefix = i == selectedIndex ? "[blue]>[/] " : "  ";
                AnsiConsole.MarkupLine($"{prefix}{items[i].Item1} (€{items[i].Item2}) - [bold]{itemQuantities[i]}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[green]Gebruik de pijltjestoetsen om te navigeren en de hoeveelheid aan te passen. Druk op Enter om door te gaan.[/]");
            AnsiConsole.MarkupLine("[green]Eten en drinken wordt in uw ticket verwerkt, scan uw ticket bij de Burger King in onze locatie om te verzilveren.[/]");

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(items.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (itemQuantities[selectedIndex] > 0)
                    {
                        itemQuantities[selectedIndex]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (itemQuantities[selectedIndex] < 5)
                    {
                        itemQuantities[selectedIndex]++;
                    }
                    break;
                case ConsoleKey.Enter:
                    for (int i = 0; i < items.Count; i++)
                    {
                        if (itemQuantities[i] > 0)
                        {
                            SelectedItems.Add((items[i].Item1, itemQuantities[i], items[i].Item2));
                        }
                    }
                    AnsiConsole.Clear();
                    ReservationSystem.ProceedToPayment();
                    return;
            }
        }
    }
}
