using Spectre.Console;
using Newtonsoft.Json;

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
                    Console.Clear();
                    Console.WriteLine("You selected: " + movies[selectedIndex].Title);

                    AnsiConsole.Clear();

                    Movie selectedMovie = movies[selectedIndex];

                    // Display detailed information about the selected movie
                    AnsiConsole.MarkupLine($"[underline yellow]Title:[/] {selectedMovie.Title}");
                    AnsiConsole.MarkupLine($"[underline yellow]Release:[/] {selectedMovie.Release}");
                    AnsiConsole.MarkupLine($"[underline yellow]Director:[/] {selectedMovie.Director}");
                    AnsiConsole.MarkupLine($"[underline yellow]Duration:[/] {selectedMovie.Duration}");
                    AnsiConsole.MarkupLine($"[underline yellow]Genre:[/] {string.Join(", ", selectedMovie.Genre)}");
                    AnsiConsole.MarkupLine($"[underline yellow]Age Rating:[/] {selectedMovie.AgeRating}");
                    AnsiConsole.MarkupLine($"[underline yellow]Actors:[/] {string.Join(", ", selectedMovie.Actors)}");
                    AnsiConsole.Markup($"[underline yellow]Description:[/] {selectedMovie.Description}\n");

                    Console.WriteLine();
                    Console.WriteLine();
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


    public static List<Movie> LoadMovies()
    {
        string json = File.ReadAllText("Data/Movies.json") ?? "";
        List<Movie>? movies = JsonConvert.DeserializeObject<List<Movie>>(json) ?? new List<Movie>();
        return movies;
    }

}