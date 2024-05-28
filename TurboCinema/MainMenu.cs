using Spectre.Console;
using System;

public static class MainScreen
{
    public static void MainMenu()
    {
        bool runApp = true;
        while (runApp)
        {
            CE.Clear();
            AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
            AnsiConsole.Write(new Rule("Welkom bij TurboCinema!").Centered().RuleStyle("red dim"));
            CE.WL();

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hoofdmenu")
                    .PageSize(10)
                    .HighlightStyle(Style.Parse("red"))
                    .AddChoices("Films/Reserveren", "Inloggen/Registeren", "Zaal Configureren", "Afsluiten"));

            switch (keuze)
            {
                case "Films/Reserveren":
                    Program.ShowScreen(DisplayAndHandleMovies);
                    break;
                case "Inloggen/Registeren":
                    Program.ShowScreen(LoginScreen.LoginMenu);
                    break;
                case "Zaal Configureren":
                    Program.ShowScreen(AdminMenu);
                    break;
                case "Afsluiten":
                    runApp = false;
                    Environment.Exit(0);
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Ongeldige keuze[/]");
                    break;
            }

            Console.ReadLine();
        }
    }

    public static void DisplayAndHandleMovies()
    {
        MovieSelector.SelectMovie();
        AnsiConsole.Clear();

        bool startOver = CE.Confirm("Opnieuw beginnen met een nieuwe film?");
        if (!startOver)
        {
            Environment.Exit(0);
        }
    }

    public static void AdminMenu()
    {
        AnsiConsole.Write(new FigletText("Admin Menu").Centered().Color(Color.Blue));
        var hallName = AnsiConsole.Ask<string>("Voer de naam van de zaal in:");
        var numRows = AnsiConsole.Ask<int>("Voer het aantal rijen in:");
        var numSeatsPerRow = AnsiConsole.Ask<int>("Voer het aantal stoelen per rij in:");

        string moviesFilePath = "path_to_MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.SetHallSize(hallName, numRows, numSeatsPerRow);

        AnsiConsole.MarkupLine($"[green]De configuratie van {hallName} is succesvol bijgewerkt naar {numRows} rijen en {numSeatsPerRow} stoelen per rij.[/]");
        CE.PressAnyKey();
        Program.PreviousScreen();
    }
}
