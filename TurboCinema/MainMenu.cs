using Spectre.Console;
using System;

public static class MainScreen
{
    public static void MainMenu()
    {
        string moviesFilePath = "Data/MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.RemovePastPlaytimes();

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

    var adminActions = new List<string> { "Configureer Zaal", "Genereer Speeltijden", "Nieuwe Film Toevoegen", "Terug naar Hoofdmenu" };
    var actionChoice = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Kies een actie")
            .PageSize(10)
            .HighlightStyle(Style.Parse("blue"))
            .AddChoices(adminActions));

    switch (actionChoice)
    {
        case "Zaal aanpassen":
            ZaalAanpassen();
            break;
        case "Genereer Speeltijden":
            GeneratePlaytimes();
            break;
        case "Nieuwe Film Toevoegen":
            AddNewMovie();
            break;
        case "Terug naar Hoofdmenu":
            return;
    }
}

    public static void AddNewMovie()
    {
        string title = AnsiConsole.Ask<string>("Voer de titel van de film in:");
        string release = AnsiConsole.Ask<string>("Voer de releasedatum van de film in (YYYY-MM-DD):");
        string director = AnsiConsole.Ask<string>("Voer de regisseur van de film in:");
        List<string> actors = AnsiConsole.Ask<string>("Voer de acteurs in (gescheiden door komma's):").Split(',').Select(a => a.Trim()).ToList();
        string duration = AnsiConsole.Ask<string>("Voer de duur van de film in (in minuten):");
        List<string> genre = AnsiConsole.Ask<string>("Voer de genres in (gescheiden door komma's):").Split(',').Select(g => g.Trim()).ToList();
        string ageRating = AnsiConsole.Ask<string>("Voer de leeftijdsclassificatie in:");
        string description = AnsiConsole.Ask<string>("Voer de beschrijving van de film in:");

        // Maak een nieuw Movie-object aan
        Movie newMovie = new Movie(title, release, director, actors, duration, genre, ageRating, description)
        {
            Playtimes = new List<Playtime>()
        };

        string moviesFilePath = "Data/MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.AddMovie(newMovie);

        AnsiConsole.MarkupLine($"[green]De film '{title}' is succesvol toegevoegd.[/]");
        CE.PressAnyKey();
    }

    public static void ZaalAanpassen()
    {
        var hallName = AnsiConsole.Ask<string>("Voer de naam van de zaal in:");
        var numRows = AnsiConsole.Ask<int>("Voer het aantal rijen in:");
        var numSeatsPerRow = AnsiConsole.Ask<int>("Voer het aantal stoelen per rij in:");

        string moviesFilePath = "Data/MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.SetHallSize(hallName, numRows, numSeatsPerRow);

        AnsiConsole.MarkupLine($"[green]De configuratie van {hallName} is succesvol bijgewerkt naar {numRows} rijen en {numSeatsPerRow} stoelen per rij.[/]");
        CE.PressAnyKey();
    }

    public static void GeneratePlaytimes()
    {
        string moviesFilePath = "Data/MoviesAndPlaytimes.json";
        Admin admin = new Admin(moviesFilePath);
        admin.GeneratePlaytimes(admin._movies);
        admin.SaveMovies(); // Opslaan van de nieuwe speeltijden in het bestand

        AnsiConsole.MarkupLine("[green]Speeltijden succesvol gegenereerd voor alle films.[/]");
        CE.PressAnyKey();
    }


}