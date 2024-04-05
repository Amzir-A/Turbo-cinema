using Spectre.Console;
using Newtonsoft.Json;


class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Toon de hoofdtitel van de applicatie.
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("TurboCinema").Centered().Color(Color.Red));
        AnsiConsole.WriteLine();

        bool runApp = true;
        while (runApp)
        {
            // Toon films en laat de gebruiker een film selecteren.
            MovieSelector movieSelector = new MovieSelector();

            // Vraag of de gebruiker door wil gaan naar het selecteren van stoelen of terug wil naar filmselectie.
            bool proceedToSeats = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar stoelenselectie? [green]Ja[/] of ga [red]terug[/]?"));
            if (!proceedToSeats)
            {
                Console.Clear();
                movieSelector.DisplayMovies();

                continue; // Dit zal de lus herstarten, waardoor de gebruiker opnieuw een film kan selecteren.

            }

            // Logica voor het selecteren van stoelen hier.
            var reservationSystem = new ReservationSystem();
            var selectedSeat = reservationSystem.SelectSeats();

            // Bevestiging voor het verder gaan naar betaalscherm of terug gaan.
            bool proceedToPayment = AnsiConsole.Prompt(new ConfirmationPrompt("Doorgaan naar betaalscherm? [green]Ja[/] of [red]Nee[/]?"));
            if (!proceedToPayment)
            {
                Console.Clear();
                movieSelector.DisplayMovies();
                continue; // Dit zal de lus herstarten, waardoor de gebruiker opnieuw kan beginnen.
            }

            // Logica voor betaalscherm hier.
            AnsiConsole.Clear();
            var betaalscherm = new Betaalscherm();

            // Nadat de betaling is voltooid, vraag of ze opnieuw willen beginnen of willen afsluiten.
            bool startOver = AnsiConsole.Prompt(new ConfirmationPrompt("Opnieuw beginnen met een nieuwe film? [green]Ja[/] of [red]Nee[/]?"));
            if (!startOver)
            {
                runApp = false; // Dit zal de lus beëindigen en het programma afsluiten.
            }
        }
    }
}
