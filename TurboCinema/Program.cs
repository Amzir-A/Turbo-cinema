using Newtonsoft.Json;
using System.Text;

static class Program
{
    class ScreenState
    {
        public required Delegate ScreenDelegate { get; set; }
        public object[]? Arguments { get; set; }
    }

    static Stack<ScreenState> screenHistory = new();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ShowScreen(MainScreen.MainMenu);
    }
<<<<<<< Updated upstream

    public static void ShowScreen(Action screen)
    {
        screenHistory.Push(new ScreenState { ScreenDelegate = screen });
        screen.Invoke();
    }
    public static void ShowScreen<T>(Action<T> screen, T arg)
    {
        screenHistory.Push(new ScreenState { ScreenDelegate = screen, Arguments = new object[] { arg } });
        screen.Invoke(arg);
    }

    public static void PreviousScreen()
    {
        if (screenHistory.Count > 1)
        {
            screenHistory.Pop();
            ScreenState previousScreen = screenHistory.Peek();
            if (previousScreen.Arguments != null)
                previousScreen.ScreenDelegate.DynamicInvoke(previousScreen.Arguments);
            else
                previousScreen.ScreenDelegate.DynamicInvoke();
        }
    }
<<<<<<< HEAD

    static void DisplayAndHandleMovies()
    {
        MovieSelector movieSelector = new MovieSelector();
        Movie selectedMovie = movieSelector.GetSelectedMovie(); // Haal de geselecteerde film op.
        Playtime selectedPlaytime = movieSelector.GetSelectedPlaytime(); // Haal de geselecteerde speeltijd op.
        AnsiConsole.Clear();

        bool proceedToSeats = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar stoelenselectie? [green]Ja[/] of [red]nee[/]?"));
        if (!proceedToSeats) return;

        // Logica voor het selecteren van stoelen hier.
        var reservationSystem = new ReservationSystem(selectedMovie, selectedPlaytime); // Geef de geselecteerde film door aan het reserveringssysteem.
        var selectedSeat = reservationSystem.SelectSeats();

        bool proceedToPayment = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar betaalscherm? [green]Ja[/] of [red]Nee[/]?"));
        if (!proceedToPayment) return;

        // Logica voor betaalscherm hier.
        AnsiConsole.Clear();
        reservationSystem.ProceedToPayment();

        // Nadat de betaling is voltooid, vraag of ze opnieuw willen beginnen of willen afsluiten.
        bool startOver = AnsiConsole.Prompt(new ConfirmationPrompt("Opnieuw beginnen met een nieuwe film? [green]Ja[/] of [red]Nee[/]?"));
        if (!startOver)
        {
            Environment.Exit(0);
        }
    }

=======
    public static void DisplayMainMenu()
    {
        // Code to display the main menu goes here
        ShowScreen(MainScreen.MainMenu);
    }
>>>>>>> Stashed changes
=======
>>>>>>> main
}
