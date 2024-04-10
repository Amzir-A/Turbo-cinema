using Spectre.Console;

class Betaalscherm
{

    private List<Seat> selectedSeats;
    private const int SeatPrice = 7;

    public Betaalscherm(List<Seat> selectedSeats)
    {
        this.selectedSeats = selectedSeats;
    }
    public void DisplayPaymentScreen()
    {

        int totalPrice = this.selectedSeats.Count * SeatPrice;
        // Ask for the user's favorite fruit
        var methode = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer [green]betaalmethode[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Scroll omhoog of omlaag om meer betaalmethodes te zien)[/]")
                .AddChoices(new[] {
                    "Ideal", "Visa", "Mastercard",
                    "Contant [grey](Op locatie)[/]",
                }));

        if (methode == "Ideal")
        {
            var bank = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer [green]bank[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Scroll omhoog of omlaag om meer banken te zien)[/]")
                .AddChoices(new[] {
                    "ABN AMRO",
                    "ING Bank",
                    "Rabobank",
                    "SNS Bank",
                    "ASN Bank",
                    "RegioBank",
                    "Triodos Bank",
                    "Knab",
                    "Bunq"
                }));

            AnsiConsole.Markup($"[green]U heeft gekozen voor {methode} en {bank}[/]\n\n");
            Console.WriteLine($"Bedrag: €{totalPrice},00");

            var betaald = AnsiConsole.Prompt(new ConfirmationPrompt("Wilt u betalen?"));
            AnsiConsole.Clear();

            if (betaald)
            {
                AnsiConsole.Markup("[green]Je reservering is geboekt![/]");
            }
            else
            {
                AnsiConsole.Markup("[yellow]Je wordt omgeleid naar het beginscherm...[/]");
            }
        }
    }
}