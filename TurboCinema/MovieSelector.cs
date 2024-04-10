using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Text;

class MovieSelector
{
    List<Movie>? movies = LoadMovies();
    static int selectedIndex = 0;
    static Style? SelectedStyle;


    public MovieSelector()
    {
        DisplayMovies();

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

                    return;
            }

            DisplayMovies();
        }
    }


    public void DisplayMovies()
    {
        Console.Clear();
        AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
        AnsiConsole.WriteLine();

        if (movies?.Count > 0)
        {
            // AnsiConsole.MarkupLine("[[bold yellow] Star Wars Movies [/]]");
            AnsiConsole.Write(new Text("[ Movies ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();


            var grid = new Table
            {
                Border = TableBorder.SimpleHeavy,
                BorderStyle = new Style(Color.Red),

            };

            grid.AddColumn(new TableColumn("[red]ID[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Title[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Release[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Director[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Duration[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Genre[/]").Centered());
            grid.AddColumn(new TableColumn("[red]Age Rating[/]").Centered());
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

            AnsiConsole.Write(new Text("[ End ]", new Style(Color.Yellow, Color.Black)).Centered());

        }
        else
        {
            AnsiConsole.Markup("[red]No movies found.[/]");
        }
    }


    public void DisplayMovieDetails(Movie selectedMovie)
    {
        // Start with a clear screen
        AnsiConsole.Clear();
        // Retain the TurboCinema header
        AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));

        // Construct the movie details as a single string.
        var details = new StringBuilder();
        details.AppendLine($"[bold]Title:[/] {selectedMovie.Title}");
        details.AppendLine($"[bold]Release:[/] {selectedMovie.Release}");
        details.AppendLine($"[bold]Director:[/] {selectedMovie.Director}");
        details.AppendLine($"[bold]Duration:[/] {selectedMovie.Duration} minutes");
        details.AppendLine($"[bold]Genre:[/] {string.Join(", ", selectedMovie.Genre)}");
        details.AppendLine($"[bold]Age Rating:[/] {selectedMovie.AgeRating}");
        details.AppendLine($"[bold]Actors:[/] {string.Join(", ", selectedMovie.Actors)}");
        details.AppendLine($"[bold]Description:[/] {selectedMovie.Description}");

        // Create a panel with the details content
        var panel = new Panel(details.ToString())
            .Expand()
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Red))
            .Padding(1, 2);

        // Render the panel below the header
        AnsiConsole.Write(panel);

    }


    public void DisplayMoviePlaytimes(Movie selectedMovie)
    {
        var playtimesJson = File.ReadAllText("Data/playtimes.json") ?? "";
        var moviePlaytimesList = JsonConvert.DeserializeObject<List<MoviePlaytimes>>(playtimesJson);

        var selectedMoviePlaytimes = moviePlaytimesList?.FirstOrDefault(mp => mp.Title.Equals(selectedMovie.Title, StringComparison.OrdinalIgnoreCase));

        if (selectedMoviePlaytimes?.Playtimes != null && selectedMoviePlaytimes.Playtimes.Any())
        {
            // Start with a clear screen
            AnsiConsole.Clear();

            // Use a table to present the playtimes
            var table = new Table()
                .Centered()
                .AddColumn(new TableColumn("Date and Time").Centered())
                .AddColumn(new TableColumn("Room").Centered());

            foreach (var playtime in selectedMoviePlaytimes.Playtimes)
            {
                // Format the DateTime nicely here
                table.AddRow(playtime.DateTime.ToString("g"), playtime.Room);
            }

            // Configure the look of the table
            table.Title($"[underline yellow]{selectedMovie} Playtimes[/]");
            table.Border(TableBorder.Rounded);

            // Render the table to the console
            AnsiConsole.Write(table);

            // Selection part
            var selectionPrompt = new SelectionPrompt<string>()
                .Title("Select a playtime:")
                .PageSize(10).HighlightStyle(Style.Parse("red"));

            foreach (var playtime in selectedMoviePlaytimes.Playtimes)
            {
                selectionPrompt.AddChoice(playtime.DateTime.ToString("g") + " - Room: " + playtime.Room);
            }

            var selectedOption = AnsiConsole.Prompt(selectionPrompt);
            AnsiConsole.WriteLine($"You selected: {selectedOption}");

            // Here, handle the selected option as needed
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
