
using Spectre.Console;
using System;

public static class MainScreen
{
    public static void MainMenu()
    {
        ReservationSystem.SelectedSeats.Clear();
        bool runApp = true;
        while (runApp)
        {
            CE.Clear();
            AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
            AnsiConsole.Write(new Rule("Welkom bij TurboCinema!").Centered().RuleStyle("red dim"));
            AnsiConsole.Write(new Rule("Gebruik de pijltjes toetsen om te navigeren").Centered().RuleStyle("red dim"));
            AnsiConsole.Write(new Rule("Klik op enter om een optie te kiezen").Centered().RuleStyle("red dim"));
            CE.WL();

            var choices = new List<string> { "Films/Reserveren", "Inloggen/Registeren", "Afsluiten" };

            if (LoginScreen.IsAdmin)
            {
                choices.Insert(2, "Admin");
            }

            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hoofdmenu")
                    .PageSize(10)
                    .HighlightStyle(Style.Parse("red"))
                    .AddChoices(choices));


            switch (keuze)
            {
                case "Films/Reserveren":
                    Program.ShowScreen(DisplayAndHandleMovies);
                    break;
                case "Inloggen/Registeren":
                    Program.ShowScreen(LoginScreen.LoginMenu);
                    break;
                case "Admin":
                    if (LoginScreen.IsAdmin)
                    {
                        Program.ShowScreen(AdminMenu);
                    }
                    break;
                case "Afsluiten":
                    runApp = false;
                    Environment.Exit(0);
                    break;
                default:
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

        string moviesFilePath = "Data/MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.SetHallSize(hallName, numRows, numSeatsPerRow);

        AnsiConsole.MarkupLine($"[green]De configuratie van {hallName} is succesvol bijgewerkt naar {numRows} rijen en {numSeatsPerRow} stoelen per rij.[/]");
        CE.PressAnyKey();
    }
}