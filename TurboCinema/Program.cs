using Newtonsoft.Json;
using Spectre.Console;
using System.Text;

static class Program
{
    public class ScreenState
    {
        public required Delegate ScreenDelegate { get; set; }
        public object[]? Arguments { get; set; }
    }
  
    public static List<ScreenState> screenHistory = new();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ShowScreen(MainScreen.MainMenu);
    }
<<<<<<< Updated upstream

    public static void ShowScreen(object screen)
    {
        if (screen is Delegate scrDelegate)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrDelegate });
            scrDelegate.DynamicInvoke();
        } else if (screen is Action scrAction)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrAction });
            scrAction.Invoke();
        }
    }
    public static void ShowScreen(object screen, object[] arg)
    {
        if (screen is Delegate scrDelegate)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrDelegate, Arguments = arg });
            scrDelegate.DynamicInvoke(arg);
        } else if (screen is Action scrAction)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrAction, Arguments = arg });
            scrAction.DynamicInvoke(arg);
        }
    }

    public static void PreviousScreen()
    {
        if (screenHistory.Count > 1)
        {
            screenHistory.RemoveAt(screenHistory.Count - 1);
            ScreenState previousScreen = screenHistory[^1];
            screenHistory.RemoveAt(screenHistory.Count - 1);

            if (previousScreen.Arguments != null)
                ShowScreen(previousScreen.ScreenDelegate, previousScreen.Arguments);
            else
                ShowScreen(previousScreen.ScreenDelegate);
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
