using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        bool hasAccount = AnsiConsole.Confirm("Heeft u een account bij ons?");
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
            Console.WriteLine($"Bedrag: €{totalPrice},00");
    
            var betaald = AnsiConsole.Prompt(new ConfirmationPrompt("Wilt u betalen?"));
            AnsiConsole.Clear();
    
            if (betaald)
            {
                AnsiConsole.Markup("Werwerken betaling...");
                System.Threading.Thread.Sleep(2000);
                AnsiConsole.Markup("[green]Betaling gelukt![/]\n");
            }
        else
        {
            AnsiConsole.Markup("[yellow]Je wordt omgeleid naar het beginscherm...[/]");
        }
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