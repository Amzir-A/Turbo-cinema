using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

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
                    selectedIndex = Math.Min(movies.Count - 1 + 1, selectedIndex + 1);
                    break;
                case ConsoleKey.Enter:
                    AnsiConsole.Clear();

                    if (selectedIndex == movies.Count)
                    {
                        Program.PreviousScreen();
                    }

                    Movie selectedMovie = movies[selectedIndex];


                    Program.ShowScreen(DisplayMovieDetails, [selectedMovie]);
                    // DisplayMovieDetails(selectedMovie);

                    Program.ShowScreen(DisplayMoviePlaytimes, [selectedMovie]);

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
                var genre = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Selecteer een genre:")
                    .PageSize(13)
                    .AddChoices(new[] { "Action", "Adventure","Biography", "Comedy", "Superhero", "Supernatural", "Drama", "Horror","Musical", "Mystery", "Romance", "Science fiction","Thriller"})
                );
                sortedMovies = movies.Where(m => m.Genre.Contains(genre)).ToList();
                break;
            case "actor":
                sortedMovies = movies.Where(m => m.Actors.Any()).OrderBy(m => m.Actors.FirstOrDefault()).ToList();
                break;
            case "release date":
                sortedMovies = movies.OrderByDescending(m =>
                {
                    DateTime releaseDate;
                    var dateFormats = new[] {"d-MM-yyyy", "dd-MM-yyyy"};
                    if (DateTime.TryParseExact(m.Release, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate))
                    {
                        return releaseDate;
                    }
                    return DateTime.MinValue;
                }).ToList();
                break;
            case "duration":
                sortedMovies = movies.OrderByDescending(m =>
                {
                    if (!int.TryParse(m.Duration.Split(' ')[0], out int duration))
                    {
                        duration = int.MaxValue;
                    }
                    return duration;
                }).ToList();
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

            if (selectedIndex == movies.Count)
            {
                AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Grey)).Centered());
            }
            else
            {
                AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
            }

        }
        else
        {
            AnsiConsole.Markup("[red]Geen films gevonden.[/]");
        }
    }


    public void DisplayMovieDetails(Movie selectedMovie)
    {

        int choice = 0;
        Style style_y = new Style(Color.Yellow, Color.Black);
        Style style_x = new Style(Color.Yellow, Color.Grey);


        while (true)
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


            AnsiConsole.Write(new Text($"[ Doorgaan ]",  style_x).Centered());
            AnsiConsole.Write(new Text($"[ Terug ]\n", style_y).Centered());


            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    choice--;
                    choice = Math.Min(choice, 1);
                    Style temp = style_x;
                    style_x = style_y;
                    style_y = temp;
                    break;
                case ConsoleKey.DownArrow:
                    choice++;
                    choice = Math.Max(choice, 0);
                    Style temp2 = style_x;
                    style_x = style_y;
                    style_y = temp2;
                    break;
                case ConsoleKey.Enter:
                    if (choice == 1)
                    {
                        Program.PreviousScreen();
                    }
                    else
                    {
                        return;
                    }
                    break;
            }
        }
    }


    public void DisplayMoviePlaytimes(Movie selectedMovie)
    {
        var playtimesJson = File.ReadAllText("Data/playtimes.json") ?? "";
        var moviePlaytimesList = JsonConvert.DeserializeObject<List<MoviePlaytimes>>(playtimesJson);

        var selectedMoviePlaytimes = moviePlaytimesList?.FirstOrDefault(mp => mp.Title.Equals(selectedMovie.Title, StringComparison.OrdinalIgnoreCase));

        int choice = 0;
        Style style_y = new Style(Color.Yellow, Color.Black);
        Style style_x = new Style(Color.Yellow, Color.Grey);


        while (true)
        {
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

                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();

                AnsiConsole.Write(new Text("Selecteer een tijdstip:", new Style(Color.Yellow, Color.Black, Decoration.Bold)).Centered());

                for (int i = 0; i < selectedMoviePlaytimes.Playtimes.Count; i++)
                {
                    if (i == choice)
                    {
                        AnsiConsole.Write(new Text(selectedMoviePlaytimes.Playtimes[i].DateTime.ToString("g") + " - Zaal: " + selectedMoviePlaytimes.Playtimes[i].Room, new Style(Color.Red, Color.Black)).Centered());
                    }
                    else
                    {
                        AnsiConsole.Write(new Text(selectedMoviePlaytimes.Playtimes[i].DateTime.ToString("g") + " - Zaal: " + selectedMoviePlaytimes.Playtimes[i].Room, new Style(Color.White, Color.Black)).Centered());
                    }
                }

                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();

                if (choice == selectedMoviePlaytimes.Playtimes.Count)
                {
                    AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Grey)).Centered());
                }
                else
                {
                    AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
                }

            }
            else
            {
                AnsiConsole.Markup("[red]Geen speeltijden gevonden voor deze film.[/]");
            }


            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    choice--;
                    choice = Math.Max(choice, 0);
                    Style temp = style_x;
                    style_x = style_y;
                    style_y = temp;
                    break;
                case ConsoleKey.DownArrow:
                    choice++;
                    choice = Math.Min(choice, selectedMoviePlaytimes.Playtimes.Count);
                    Style temp2 = style_x;
                    style_x = style_y;
                    style_y = temp2;
                    break;
                case ConsoleKey.Enter:
                    if (choice == selectedMoviePlaytimes.Playtimes.Count)
                    {
                        Program.PreviousScreen();
                    }
                    else
                    {
                        selectedPlaytime = selectedMoviePlaytimes.Playtimes[choice];
                        return;
                    }
                    break;
            }
        }
    }



    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }
}
