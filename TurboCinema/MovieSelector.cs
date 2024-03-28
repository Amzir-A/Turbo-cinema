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
    public int SelectMovieByTitle()
    {
        var movieTitles = movies?.Select(m => m.Title).ToList() ?? new List<string>();

        var selectedTitle = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer een [green]film[/]:")
                .PageSize(10) // Number of items to show before scrolling
                .MoreChoicesText("[grey](Scroll omhoog of omlaag om meer films te zien)[/]")
                .AddChoices(movieTitles));

        // Find the selected movie by title (assuming titles are unique)
        var selectedMovie = movies?.FirstOrDefault(m => m.Title == selectedTitle);

        // Return the movie ID or some identifier
        return selectedMovie != null ? movies.IndexOf(selectedMovie) + 1 : -1; // Adding 1 to match your 1-based IDs
    }


    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }

}