using Spectre.Console;
using System.Text.Json;

public class Betaalscherm
{
    private List<Seat> selectedSeats;
    private const int SeatPrice = 7;
    private Movie selectedMovie;
    private Playtime selectedPlaytime;

    public Betaalscherm(List<Seat> selectedSeats, Movie selectedMovie, Playtime selectedPlaytime)
    {
        this.selectedSeats = selectedSeats;
        this.selectedMovie = selectedMovie;
        this.selectedPlaytime = selectedPlaytime;
    }

    public void DisplayPaymentScreen()
    {
        int totalPrice = this.selectedSeats.Count * SeatPrice;
        AnsiConsole.Markup($"U heeft gekozen voor de film: [green]{selectedMovie.Title}[/] op [green]{selectedPlaytime.DateTime}[/]\n");
        AnsiConsole.Markup($"Totale prijs: €{totalPrice}\n");

        bool hasAccount = CE.Confirm("Heeft u een account bij ons?");
        if (hasAccount)
        {
            string email = AnsiConsole.Ask<string>("Wat is uw emailadres?");
            Customer customer = FindCustomerByEmail(email);
            if (customer != null)
            {
                ProcessPayment(totalPrice);
                SaveReservation(customer, totalPrice, selectedPlaytime);
                AnsiConsole.Markup("[green]Uw reservering is toegevoegd aan uw account.[/]");
            }
            else
            {
                AnsiConsole.Markup("[red]Geen account gevonden met dat emailadres.[/]");
            }
        }
        else
        {
            string email = AnsiConsole.Ask<string>("Wat is uw emailadres?");
            ProcessPayment(totalPrice);
            NoAccount(email, totalPrice);
        }
    }

    private void ProcessPayment(int totalPrice)
    {
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
        }
        else if (methode == "Visa" || methode == "Mastercard")
        {
            var cardNumber = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer uw [green]kaartnummer[/] in")
                    .PromptStyle("green")
                    .Secret());

            var expirationDate = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer de [green]vervaldatum[/] in (MM/YY)")
                    .PromptStyle("green"));

            var cvc = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer de [green]CVC[/] in")
                    .PromptStyle("green")
                    .Secret());

            AnsiConsole.Markup($"[green]U heeft gekozen voor {methode} met kaartnummer {cardNumber} en vervaldatum {expirationDate}[/]\n\n");
        }
        else
        {
            AnsiConsole.Markup($"[green]U heeft gekozen voor {methode}[/]\n\n");
        }


        Console.WriteLine($"Bedrag: €{totalPrice},00");

        if (methode != "Contant [grey](Op locatie)[/]")
        {
            if (CE.Confirm("Wilt u betalen?"))
            {
                AnsiConsole.Clear();
                CE.Wait("Verwerken betaling");
                AnsiConsole.Markup("[green]Reservering voltooid![/]\n");
            }
            else
            {
                AnsiConsole.Clear();
                AnsiConsole.Markup("[red]Reservering canceled![/]\n");
            }
        }
        else
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup("[green]Reservering voltooid![/]\n");
        }
    }


    private Customer FindCustomerByEmail(string email)
    {
        var customers = LoadCustomers("Data/AccountInfo.json");
        return customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    private void SaveReservation(Customer customer, int totalPrice, Playtime selectedPlaytime)
    {
        var reservation = new Reservation(selectedMovie.Title, selectedPlaytime.DateTime, selectedSeats, selectedPlaytime.Room);
        customer.Reservations.Add(reservation);
        SaveCustomers(customer, "Data/AccountInfo.json");
    }

    private List<Customer> LoadCustomers(string fileName)
    {
        if (!File.Exists(fileName))
        {
            return new List<Customer>();
        }

        string json = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<List<Customer>>(json) ?? new List<Customer>();
    }

    private void SaveCustomers(Customer customer, string fileName)
    {
        var customers = LoadCustomers(fileName);
        var existingCustomer = customers.Find(c => c.Email == customer.Email);
        if (existingCustomer != null)
        {
            customers.Remove(existingCustomer);
        }
        customers.Add(customer);

        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }

    private void NoAccount(string email, int totalPrice)
    {
        Customer nonAccountCustomer = new Customer()
        {
            Email = email,
            Reservations = new List<Reservation>()
        };
        var reservation = new Reservation(selectedMovie.Title, selectedPlaytime.DateTime, selectedSeats, selectedPlaytime.Room);
        nonAccountCustomer.Reservations.Add(reservation);
        SaveCustomers(nonAccountCustomer, "Data/AccountInfo.json");
    }
}