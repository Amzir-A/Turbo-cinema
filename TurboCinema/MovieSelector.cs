using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Dynamic;

class MovieSelector
{
    List<Movie>? movies = LoadMovies();
    static int selectedIndex = 0;
    static Style? SelectedStyle;
    private Playtime selectedPlaytime;


    public MovieSelector()
    {
        DisplayMovies();
        SelectMovie();
    }
    public Movie GetSelectedMovie()
    {
        return movies[selectedIndex];
    }

    public Playtime GetSelectedPlaytime()
    {
        return selectedPlaytime;
    }
    private void SelectMovie()
    {
        var sortCriteria = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Hoe wilt u de films sorteren?")
            .AddChoices(new[] { "Genre", "Actor", "Release Date", "Duration", "Doorgaan zonder sorteren" }));

        DisplaySortedMovies(sortCriteria);
        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = Math.Max(0, selectedIndex - 1);
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = Math.Min(movies.Count - 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    AnsiConsole.Clear();

                    Movie selectedMovie = movies[selectedIndex];
                    DisplayMovieDetails(selectedMovie);

                    AnsiConsole.WriteLine("\nDruk op een toets om door te gaan...");
                    Console.ReadKey();

                    DisplayMoviePlaytimes(selectedMovie);

                    return; // Verlaat de lus na het tonen van de details en speeltijden.
            }

            DisplayMovies(); // Update de weergave na elke actie.
        }
    }

    public void DisplaySortedMovies(string sortBy)
    {
        List<Movie> sortedMovies = new List<Movie>();

        switch (sortBy.ToLower())
        {
            case "genre":
                sortedMovies = movies.OrderBy(m => m.Genre.FirstOrDefault()).ToList();
                break;
            case "actor":
                sortedMovies = movies.Where(m => m.Actors.Any()).OrderBy(m => m.Actors.FirstOrDefault()).ToList();
                break;
            case "release":
                sortedMovies = movies.OrderBy(m => DateTime.Parse(m.Release)).ToList();
                break;
            case "duration":
                sortedMovies = movies.OrderBy(m => int.Parse(m.Duration)).ToList();
                break;
            case "Doorgaan zonder sorteren":
                sortedMovies = movies.ToList();
                break;
            default:
                sortedMovies = movies.ToList();
                break;
        }
        movies = sortedMovies;
        DisplayMovies();
    }


    public void DisplayMovies()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
        AnsiConsole.WriteLine();

        if (movies?.Count > 0)
        {
            // AnsiConsole.MarkupLine("[[bold yellow] Star Wars Movies [/]]");
            AnsiConsole.Write(new Text("[ Films ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();


            var grid = new Table
            {
                Border = TableBorder.SimpleHeavy,
                BorderStyle = new Style(Color.Red),

            };

            grid.AddColumn(new TableColumn("[red]ID[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Titel[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Release[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Regie[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Lengte[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Genre[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Leeftijd[/]").Centered());
            // grid.AddColumn(new TableColumn("[red]Description[/]").Centered());

            for (int i = 0; i < movies.Count; i++)
            {

                if (i == selectedIndex)
                {
                    SelectedStyle = new Style(Color.White, Color.Black).Background(Color.Grey39);
                }
                else
                {
                    SelectedStyle = new Style(Color.White, Color.Black);
                }

                var movie = movies[i];
                grid.AddRow(new Text[]{
                    new Text((i + 1).ToString(), SelectedStyle).Centered(),
                    new Text(movie.Title, SelectedStyle).Centered(),
                    new Text(movie.Release, SelectedStyle).Centered(),
                    new Text(movie.Director, SelectedStyle).Centered(),
                    new Text(movie.Duration, SelectedStyle).Centered(),
                    new Text(string.Join(", ", movie.Genre), SelectedStyle).Centered(),
                    new Text(movie.AgeRating, SelectedStyle).Centered(),
                    // new Text(movie.Description, SelectedStyle).Centered()
                });
            }

            AnsiConsole.Write(grid);

            AnsiConsole.WriteLine();

            AnsiConsole.Write(new Text("[ Eind ]", new Style(Color.Yellow, Color.Black)).Centered());

        }
        else
        {
            AnsiConsole.Markup("[red]Geen films gevonden.[/]");
        }
    }


    public void DisplayMovieDetails(Movie selectedMovie)
    {
        // Start with a clear screen
        AnsiConsole.Clear();
        // behouden the TurboCinema header
        AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));

        var details = new StringBuilder();
        details.AppendLine($"[bold]Titel:[/] {selectedMovie.Title}");
        details.AppendLine($"[bold]Release:[/] {selectedMovie.Release}");
        details.AppendLine($"[bold]Regie:[/] {selectedMovie.Director}");
        details.AppendLine($"[bold]Lengte:[/] {selectedMovie.Duration} minutes");
        details.AppendLine($"[bold]Genre:[/] {string.Join(", ", selectedMovie.Genre)}");
        details.AppendLine($"[bold]Leeftijd:[/] {selectedMovie.AgeRating}");
        details.AppendLine($"[bold]Cast:[/] {string.Join(", ", selectedMovie.Actors)}");
        details.AppendLine($"[bold]Beschrijving:[/] {selectedMovie.Description}");

        //maak een panel with the details content
        var panel = new Panel(details.ToString())
            .Expand()
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Red))
            .Padding(1, 2);

        // Render de panel onder the header
        AnsiConsole.Write(panel);

    }


    public void DisplayMoviePlaytimes(Movie selectedMovie)
    {
        var playtimesJson = File.ReadAllText("Data/playtimes.json") ?? "";
        var moviePlaytimesList = JsonConvert.DeserializeObject<List<MoviePlaytimes>>(playtimesJson);

        var selectedMoviePlaytimes = moviePlaytimesList?.FirstOrDefault(mp => mp.Title.Equals(selectedMovie.Title, StringComparison.OrdinalIgnoreCase));

        if (selectedMoviePlaytimes?.Playtimes != null && selectedMoviePlaytimes.Playtimes.Any())
        {
            AnsiConsole.Clear();

            var table = new Table()
                .Centered()
                .AddColumn(new TableColumn("Datum en tijd").Centered())
                .AddColumn(new TableColumn("Zaal").Centered());

            foreach (var playtime in selectedMoviePlaytimes.Playtimes)
            {
                table.AddRow(playtime.DateTime.ToString("g"), playtime.Room);
            }

            table.Title($"[underline yellow]{selectedMovie.Title} Speeltijden[/]");
            table.Border(TableBorder.Rounded);
            AnsiConsole.Write(table);

            var selectionPrompt = new SelectionPrompt<string>()
                .Title("Selecteer een tijdstip:")
                .PageSize(10).HighlightStyle(Style.Parse("red"));

            foreach (var playtime in selectedMoviePlaytimes.Playtimes)
            {
                selectionPrompt.AddChoice(playtime.DateTime.ToString("g") + " - Zaal: " + playtime.Room);
            }

            var selectedOption = AnsiConsole.Prompt(selectionPrompt);
            selectedPlaytime = selectedMoviePlaytimes.Playtimes.FirstOrDefault(p => p.DateTime.ToString("g") + " - Zaal: " + p.Room == selectedOption);
            AnsiConsole.WriteLine($"Uw keuze: {selectedOption}");
        }
        else
        {
            AnsiConsole.Markup("[red]Geen speeltijden gevonden voor deze film.[/]");
        }

        // Wait for user input to continue
        AnsiConsole.WriteLine("\nDruk op een toets om door te gaan...");
        Console.ReadKey();
    }



    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }
}
