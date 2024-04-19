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
                    .PageSize(10).HighlightStyle(Style.Parse("red"))
                    .AddChoices(new[] { "Films", "Inloggen", "Menukaart bioscoop", "Afsluiten" }));

            switch (keuze)
            {
                case "Films":
                    DisplayAndHandleMovies();
                    break;
                case "Inloggen":
                    HandleLoginOrRegistration();
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
        Console.ReadLine();
        }
    }

    static void HandleLoginOrRegistration()
    {
        bool proceedToLogin = AnsiConsole.Prompt(new ConfirmationPrompt("Heeft u een account? [green]Ja[/] of [red]nee[/]?"));
        try
        {
            AccountRegistration CustomerRegistration = new AccountRegistration();
            if (!proceedToLogin)
            {
                CustomerRegistration.Register();
            }
            else
            {
                CustomerRegistration.Login();
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
        }
    }

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
            // Dit zal terugkeren naar het hoofdmenu in plaats van de applicatie te sluiten.
            return;
        }
    }

}

