using Spectre.Console;
using Newtonsoft.Json;

class MovieSelector
{
    List<Movie>? movies = LoadMovies();

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

        // Find the selected movie by title (assuming titles are unique)
        var selectedMovie = movies?.FirstOrDefault(m => m.Title == selectedTitle);

        if (selectedMovie != null)
        {
            // Clear the console for detailed movie information
            AnsiConsole.Clear();

            // Display detailed information about the selected movie
            AnsiConsole.MarkupLine($"[underline yellow]Title:[/] {selectedMovie.Title}");
            AnsiConsole.MarkupLine($"[underline yellow]Release:[/] {selectedMovie.Release}");
            AnsiConsole.MarkupLine($"[underline yellow]Director:[/] {selectedMovie.Director}");
            AnsiConsole.MarkupLine($"[underline yellow]Duration:[/] {selectedMovie.Duration}");
            AnsiConsole.MarkupLine($"[underline yellow]Genre:[/] {string.Join(", ", selectedMovie.Genre)}");
            AnsiConsole.MarkupLine($"[underline yellow]Age Rating:[/] {selectedMovie.AgeRating}");
            AnsiConsole.MarkupLine($"[underline yellow]Actors:[/] {string.Join(", ", selectedMovie.Actors)}");
            AnsiConsole.Markup($"[underline yellow]Description:[/] {selectedMovie.Description}\n");

            // Wait for user input to continue
            AnsiConsole.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            
            // Optionally clear the console again if you plan to move to another screen
            AnsiConsole.Clear();
        }
        else
        {
            AnsiConsole.Markup("[red]Movie not found.[/]");
        }
    }


    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }

}