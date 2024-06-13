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
        bool inButtonSection = false;

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("Eten & Drinken").Centered().Color(Color.Red));

            // Display food items and quantities
            for (int i = 0; i < items.Count; i++)
            {
                string prefix = i == selectedIndex && !inButtonSection ? "[blue]>[/] " : "  ";
                AnsiConsole.MarkupLine($"{prefix}{items[i].Item1} (€{items[i].Item2}) - [bold]{itemQuantities[i]}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.MarkupLine("[green]Gebruik de omhoog/omlaag pijltjestoetsen om te navigeren, en de links/rechts pijltjestoetsen om de hoeveelheid aan te passen. Druk op doorgaan om door te gaan met uw selectie, of om over te slaan.[/]");
            AnsiConsole.MarkupLine("[green]Eten en drinken wordt in uw ticket verwerkt, scan uw ticket bij de Burger King in onze locatie om te verzilveren.[/]");

            // Display buttons
            AnsiConsole.WriteLine();
            
            var doorgaan = new Text("[ Doorgaan ]", inButtonSection && selectedIndex == items.Count ? new Style(Color.Yellow, Color.Grey) : new Style(Color.Yellow, Color.Black)).Centered();
            var terug = new Text("[ Terug ]", inButtonSection && selectedIndex == items.Count + 1 ? new Style(Color.Yellow, Color.Grey) : new Style(Color.Yellow, Color.Black)).Centered();
            AnsiConsole.Write(doorgaan);
            AnsiConsole.Write(terug);

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    if (inButtonSection)
                    {
                        if (selectedIndex > items.Count)
                        {
                            selectedIndex--;
                        }
                        else
                        {
                            inButtonSection = false;
                            selectedIndex = items.Count - 1;
                        }
                    }
                    else
                    {
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                    }
                    break;
                case ConsoleKey.DownArrow:
                    if (inButtonSection)
                    {
                        selectedIndex = Math.Min(items.Count + 1, selectedIndex + 1);
                    }
                    else
                    {
                        if (selectedIndex < items.Count - 1)
                        {
                            selectedIndex++;
                        }
                        else
                        {
                            inButtonSection = true;
                            selectedIndex = items.Count; // Move to first button
                        }
                    }
                    break;
                case ConsoleKey.LeftArrow:
                    if (inButtonSection)
                    {
                        if (selectedIndex == items.Count + 1)
                        {
                            selectedIndex = items.Count; // Move to "Doorgaan" button
                        }
                    }
                    else if (itemQuantities[selectedIndex] > 0)
                    {
                        itemQuantities[selectedIndex]--;
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (inButtonSection)
                    {
                        if (selectedIndex == items.Count)
                        {
                            selectedIndex = items.Count + 1; // Move to "Terug" button
                        }
                    }
                    else if (itemQuantities[selectedIndex] < 5)
                    {
                        itemQuantities[selectedIndex]++;
                    }
                    break;
                case ConsoleKey.Enter:
                    if (inButtonSection)
                    {
                        if (selectedIndex == items.Count)
                        {
                            for (int i = 0; i < items.Count; i++)
                            {
                                if (itemQuantities[i] > 0)
                                {
                                    SelectedItems.Add((items[i].Item1, itemQuantities[i], items[i].Item2));
                                }
                            }
                            AnsiConsole.Clear();
                            Program.ShowScreen(ReservationSystem.ProceedToPayment);
                            return;
                        }
                        else if (selectedIndex == items.Count + 1)
                        {
                            SelectedItems.Clear();
                            ReservationSystem.SelectedSeats.Clear();
                            Program.PreviousScreen();
                            return;
                        }
                    }
                    break;
                case ConsoleKey.Tab:
                    if (inButtonSection)
                    {
                        inButtonSection = false; // Move back to item selection
                        selectedIndex = Math.Min(items.Count - 1, selectedIndex); // Reset to last item if out of bounds
                    }
                    else
                    {
                        inButtonSection = true; // Move to button section
                        selectedIndex = items.Count; // Start with "Doorgaan" button
                    }
                    break;
            }
        }
    }
}
