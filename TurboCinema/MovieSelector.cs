using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Text;

class MovieSelector
{
    public List<Movie>? movies = LoadMovies();

    public void DisplayMovies()
    {

        if (movies?.Count > 0)
        {
            // AnsiConsole.MarkupLine("[[bold yellow] Star Wars Movies [/]]");
            AnsiConsole.Write(new Text("[ Movies ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();

            var rule = new Rule();
            rule.Style = Style.Parse("red dim");
            AnsiConsole.Write(rule);

            var grid = new Grid();
            grid.AddColumns(8);
            grid.AddRow(new Text[]{
                new Text("ID", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Title", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Release", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Director", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Duration", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Genre", new Style(Color.Red, Color.Black)).Centered(),
                new Text("Age Rating", new Style(Color.Red, Color.Black)).Centered(),
                // new Text("Description", new Style(Color.Red, Color.Black)).Centered()
            });

            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                grid.AddRow(new Text[]{
                    new Text((i + 1).ToString(), new Style(Color.White, Color.Black)).Centered(),
                    new Text(movie.Title, new Style(Color.White, Color.Black)).Centered(),
                    new Text(movie.Release, new Style(Color.White, Color.Black)).Centered(),
                    new Text(movie.Director, new Style(Color.White, Color.Black)).Centered(),
                    new Text(movie.Duration, new Style(Color.White, Color.Black)).Centered(),
                    new Text(string.Join(", ", movie.Genre), new Style(Color.White, Color.Black)).Centered(),
                    new Text(movie.AgeRating, new Style(Color.White, Color.Black)).Centered(),
                    // new Text(movie.Description, new Style(Color.White, Color.Black)).Centered()
                });
            }

            AnsiConsole.Write(grid.Centered());

            var rule2 = new Rule();
            rule2.Style = Style.Parse("red dim");
            AnsiConsole.Write(rule2);
            AnsiConsole.WriteLine();

            AnsiConsole.Write(new Text("[ End ]", new Style(Color.Yellow, Color.Black)).Centered());

        }
        else
        {
            AnsiConsole.Markup("[red]No movies found.[/]");
        }
    }
    public void SelectMovie()
    {
        var movieTitles = movies?.Select(m => m.Title).ToList() ?? new List<string>();

        var selectedTitle = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select a [green]movie[/]:")
                .PageSize(10) // Number of items to show before scrolling
                .MoreChoicesText("[grey](Scroll up or down to see more movies)[/]")
                .AddChoices(movieTitles));

        var selectedMovie = movies?.FirstOrDefault(m => m.Title == selectedTitle);

        if (selectedMovie != null)
        {
            DisplayMovieDetails(selectedMovie); // Show the movie details

            // Now we wait for the user to press ENTER
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine("Press ENTER to continue...");
            Console.ReadLine(); // Waits for the Enter key to be pressed
            DisplayMoviePlaytimes(selectedTitle); // Show the playtimes
        }
        else
        {
            AnsiConsole.Markup("[red]Movie not found.[/]");
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
            .BorderStyle(new Style(Color.Yellow))
            .Padding(1, 2);

        // Render the panel below the header
        AnsiConsole.Render(panel);
        
        // Wait for user input to continue
        

    }

    

    public void DisplayMoviePlaytimes(string selectedTitle)
    {
        var playtimesJson = File.ReadAllText("Data/playtimes.json") ?? "";
        var moviePlaytimesList = JsonConvert.DeserializeObject<List<MoviePlaytimes>>(playtimesJson);

        var selectedMoviePlaytimes = moviePlaytimesList?.FirstOrDefault(mp => mp.Title.Equals(selectedTitle, StringComparison.OrdinalIgnoreCase));

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
                // Format the DateTime nicely here (you might need to adjust it to your needs)
                table.AddRow(playtime.DateTime.ToString("g"), playtime.Room);
            }

            // Configure the look of the table here with colors, borders, etc.
            table.Title($"[underline yellow]{selectedTitle} Playtimes[/]");
            table.Border(TableBorder.Rounded);

            // Finally, render the table to the console
            AnsiConsole.Render(table);
        }
        else
        {
            AnsiConsole.Markup("[red]Geen speeltijden gevonden voor deze film.[/]");
        }

        // Wait for user input to continue
        AnsiConsole.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }



    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }
}
