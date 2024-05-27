using Spectre.Console;

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


            // Toon het hoofdmenu en laat de gebruiker een keuze maken.
            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hoofdmenu")
                    .PageSize(10).HighlightStyle(Style.Parse("red"))
                    .AddChoices(["Films/Reserveren", "Inloggen/Registeren", "Afsluiten"]));

            switch (keuze)
            {
                case "Films/Reserveren":
                    Program.ShowScreen(DisplayAndHandleMovies);
                    break;
                case "Inloggen/Registeren":
                    Program.ShowScreen(LoginScreen.LoginMenu);
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
        // MovieSelector movieSelector = new MovieSelector();
        MovieSelector.SelectMovie();
        // Program.ShowScreen(MovieSelector.SelectMovie);
        // Movie selectedMovie = movieSelector.GetSelectedMovie(); // Haal de geselecteerde film op.
        // Playtime selectedPlaytime = movieSelector.GetSelectedPlaytime(); // Haal de geselecteerde speeltijd op.
        AnsiConsole.Clear();

        // Logica voor het selecteren van stoelen hier.


        

        // Nadat de betaling is voltooid, vraag of ze opnieuw willen beginnen of willen afsluiten.
        bool startOver = CE.Confirm("Opnieuw beginnen met een nieuwe film??");

        if (!startOver)
        {
            Environment.Exit(0);
        }
    }
}