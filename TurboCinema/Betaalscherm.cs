using Spectre.Console;
using System.Text.Json;

public class Betaalscherm
{
    private List<Seat> selectedSeats;
    private const int SeatPrice = 7;
    private Movie selectedMovie;
    private Playtime selectedPlaytime;
    private List<(string, int, decimal)> selectedFoodAndDrinks;
    private List<Customer> customers;
    
    public int CalculateTotalPrice()
    {
        int totalPrice = selectedSeats.Count * SeatPrice;
        foreach (var item in selectedFoodAndDrinks)
        {
            totalPrice += (int)(item.Item2 * item.Item3);
        }
        return totalPrice;
    }

    public Betaalscherm(List<Seat> selectedSeats, Movie selectedMovie, Playtime selectedPlaytime, List<(string, int, decimal)> selectedFoodAndDrinks, List<Customer> customers = null)
    {
        this.selectedSeats = selectedSeats;
        this.selectedMovie = selectedMovie;
        this.selectedPlaytime = selectedPlaytime;
        this.selectedFoodAndDrinks = selectedFoodAndDrinks;
        this.customers = customers ?? LoadCustomers("Data/AccountInfo.json");
    }

    public void DisplayPaymentScreen()
    {
        int totalPrice = this.selectedSeats.Count * SeatPrice;

        foreach (var item in selectedFoodAndDrinks)
        {
            totalPrice += (int)(item.Item2 * item.Item3);
        }

        int choice = 0;

        Style style_x = new Style(Color.Yellow, Color.Grey);
        Style style_y = new Style(Color.Yellow, Color.Black);
        Style style_z = new Style(Color.Yellow, Color.Black);

        while (true)
        {
            AnsiConsole.Clear();
            AnsiConsole.Markup($"U heeft gekozen voor de film: [green]{selectedMovie.Title}[/] op [green]{selectedPlaytime.DateTime}[/]\n");
            AnsiConsole.Markup($"Totale prijs: €{totalPrice}\n");
            AnsiConsole.WriteLine();

            AnsiConsole.MarkupLine("[bold]Geselecteerde stoelen:[/]");
            foreach (var seat in selectedSeats)
            {
                AnsiConsole.MarkupLine($"[green]{seat.ID}[/]");
            }

            AnsiConsole.MarkupLine("[bold]Geselecteerde eten en drinken:[/]");
            foreach (var item in selectedFoodAndDrinks)
            {
                AnsiConsole.MarkupLine($"[green]{item.Item1} - {item.Item2}x €{item.Item3}[/]");
            }

            AnsiConsole.WriteLine();
            AnsiConsole.Write(new Text("Wilt u inloggen of doorgaan zonder account?", new Style(Color.Green)).Centered());
            AnsiConsole.Write(new Text($"[ Inloggen ]", style_x).Centered());
            AnsiConsole.Write(new Text($"[ Doorgaan ]", style_y).Centered());
            AnsiConsole.Write(new Text($"[ Terug ]\n", style_z).Centered());

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    choice = Math.Max(0, choice - 1);
                    break;
                case ConsoleKey.DownArrow:
                    choice = Math.Min(2, choice + 1);
                    break;
                case ConsoleKey.Enter:
                    AnsiConsole.Clear();

                    if (choice == 0)
                    {
                        while (true)
                        {
                            string email = AnsiConsole.Ask<string>("Wat is uw emailadres?");
                            Customer customer = FindCustomerByEmail(email);
                            if (customer != null)
                            {
                                ProcessPayment(totalPrice, customer);
                                break;
                            }
                            else
                            {
                                AnsiConsole.Markup("[red]Geen account gevonden met dat emailadres.[/]");
                            }
                        }
                    }
                    else if (choice == 1)
                    {
                        ProcessPayment(totalPrice, null);
                    }
                    else if (choice == 2)
                    {
                        ReservationSystem.SelectedSeats = new List<Seat>();
                        selectedSeats = new List<Seat>();
                        Program.PreviousScreen();
                    }

                    return;
            }

            if (choice == 0)
            {
                style_x = new Style(Color.Yellow, Color.Grey);
                style_y = new Style(Color.Yellow, Color.Black);
                style_z = new Style(Color.Yellow, Color.Black);
            }
            else if (choice == 1)
            {
                style_x = new Style(Color.Yellow, Color.Black);
                style_y = new Style(Color.Yellow, Color.Grey);
                style_z = new Style(Color.Yellow, Color.Black);
            }
            else if (choice == 2)
            {
                style_x = new Style(Color.Yellow, Color.Black);
                style_y = new Style(Color.Yellow, Color.Black);
                style_z = new Style(Color.Yellow, Color.Grey);
            }
        }
    }

    private void ProcessPayment(int totalPrice, Customer customer)
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
                if (customer != null)
                {
                    SaveReservation(customer, totalPrice, selectedPlaytime);
                }
                
                ConfirmationScreen.Show(ReservationSystem.SelectedMovie, ReservationSystem.SelectedPlaytime, FoodAndDrinksScreen.SelectedItems);
                ReservationSystem.UpdateSeatsAvailability();
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
            // AnsiConsole.Markup("[green]Reservering voltooid![/]\n");
            ReservationSystem.UpdateSeatsAvailability();
            ConfirmationScreen.Show(ReservationSystem.SelectedMovie, ReservationSystem.SelectedPlaytime, FoodAndDrinksScreen.SelectedItems);
        }
    }

    public Customer FindCustomerByEmail(string email)
    {
        var customers = LoadCustomers("Data/AccountInfo.json");
        Customer customer = null;

        do
        {
            customer = customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (customer == null)
            {
                AnsiConsole.Markup("[red]Geen account gevonden met dat emailadres. Probeer opnieuw.[/]\n");
                email = AnsiConsole.Ask<string>("Wat is uw emailadres?");
            }
        }
        while (customer == null);

        return customer;
    }

    private void SaveReservation(Customer customer, int totalPrice, Playtime selectedPlaytime)
    {
        var reservation = new Reservation(selectedMovie.Title, selectedPlaytime.DateTime, selectedSeats, selectedPlaytime.Room, selectedFoodAndDrinks);
        customer.Reservations.Add(reservation);
        SaveCustomers(customer, "Data/AccountInfo.json");
    }

    public List<Customer> LoadCustomers(string fileName)
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
        var reservation = new Reservation(selectedMovie.Title, selectedPlaytime.DateTime, selectedSeats, selectedPlaytime.Room, selectedFoodAndDrinks);
        nonAccountCustomer.Reservations.Add(reservation);
        SaveCustomers(nonAccountCustomer, "Data/AccountInfo.json");
    }
}
