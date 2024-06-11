using System;
using Spectre.Console;

public static class ConfirmationScreen
{
    public static void Show(Movie selectedMovie, Playtime selectedPlaytime, List<(string, int, decimal)> selectedFoodAndDrinks)
    {
        AnsiConsole.Clear();
        ProgressHelper.DisplayProgressBar("Reservering Bevestigen", 5);
        AnsiConsole.Write(new FigletText("Reservering Succesvol").Centered().Color(Color.Green));
        AnsiConsole.MarkupLine("[bold]Reservering succesvol! Er is een bevestiging naar uw e-mail adres verstuurd.[/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[bold]Film:[/] {selectedMovie.Title}");
        AnsiConsole.MarkupLine($"[bold]Datum:[/] {selectedPlaytime.DateTime.ToString("dd-MM-yyyy")}");
        AnsiConsole.MarkupLine($"[bold]Zaal:[/] {selectedPlaytime.Room}");
        AnsiConsole.MarkupLine($"[bold]Tijd:[/] {selectedPlaytime.DateTime.ToString("HH:mm")}");

        AnsiConsole.MarkupLine("[bold]Geselecteerde eten en drinken:[/]");
        foreach (var item in selectedFoodAndDrinks)
        {
            AnsiConsole.MarkupLine($"[green]{item.Item1} - {item.Item2}x €{item.Item3}[/]");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[green]Druk op een toets om terug te gaan naar het hoofdmenu.[/]");
        Console.ReadKey(true);
        Program.ShowScreen(MainScreen.MainMenu);
    }
}
