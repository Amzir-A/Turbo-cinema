using Spectre.Console;
using Newtonsoft.Json;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        bool runApp = true;
        while (runApp)
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
            AnsiConsole.Write(new Rule("Welkom bij TurboCinema!").Centered().RuleStyle("red dim"));
            AnsiConsole.WriteLine();
            

            // Toon het hoofdmenu en laat de gebruiker een keuze maken.
            var keuze = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hoofdmenu")
                    .PageSize(10)
                    .AddChoices(new[] { "Films", "Inloggen in account", "Menukaart bioscoop", "Afsluiten" }));

            switch (keuze)
            {
                case "Films en tijden":
                    DisplayAndHandleMovies();
                    break;
                case "Inloggen in account":
                    // Implementeer inloglogica hier.
                    break;
                case "Menukaart bioscoop":
                    // Implementeer logica voor menukaart hier.
                    break;
                case "Afsluiten":
                    runApp = false;
                    break;
                default:
                    AnsiConsole.MarkupLine("[red]Ongeldige keuze[/]");
                    break;
            }
        }
    }

    static void DisplayAndHandleMovies()
    {
        MovieSelector movieSelector = new MovieSelector();
        movieSelector.DisplayMovies();
        movieSelector.SelectMovie();

        bool proceedToSeats = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar stoelenselectie? [green]Ja[/] of ga [red]terug[/]?"));
        if (!proceedToSeats) return;

        // Logica voor het selecteren van stoelen hier.
        var reservationSystem = new ReservationSystem();
        var selectedSeat = reservationSystem.SelectSeats();

        bool proceedToPayment = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar betaalscherm? [green]Ja[/] of [red]Nee[/]?"));
        if (!proceedToPayment) return;

        // Logica voor betaalscherm hier.
        AnsiConsole.Clear();
        var betaalscherm = new Betaalscherm();

        // Nadat de betaling is voltooid, vraag of ze opnieuw willen beginnen of willen afsluiten.
        bool startOver = AnsiConsole.Prompt(new ConfirmationPrompt("Opnieuw beginnen met een nieuwe film? [green]Ja[/] of [red]Nee[/]?"));
        if (!startOver)
        {
            // Dit zal terugkeren naar het hoofdmenu in plaats van de applicatie te sluiten.
            return;
        }
    }
}

