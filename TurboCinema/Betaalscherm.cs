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
                SaveReservation(customer, totalPrice);
                AnsiConsole.Markup("[green]Uw reservering is toegevoegd aan uw account.[/]");
            }
            else
            {
                AnsiConsole.Markup("[red]Geen account gevonden met dat emailadres.[/]");
            }
        }
        else
        {
            ProcessPayment(totalPrice);
            AnsiConsole.Markup("[yellow]U heeft geen account; de reservering wordt niet gekoppeld aan een account.[/]");
        }
    }

    private void ProcessPayment(int totalPrice)
    {
        // Simuleer een betalingsproces
        AnsiConsole.Status()
            .Start("Betaling verwerken...", ctx =>
            {
                System.Threading.Thread.Sleep(2000);
                ctx.SpinnerStyle(Style.Parse("green"));
                ctx.Status($"€{totalPrice} is betaald.");
            });
    }

    private Customer FindCustomerByEmail(string email)
    {
        var customers = LoadCustomers("AccountInfo.json");
        return customers.FirstOrDefault(c => c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
    }

    private void SaveReservation(Customer customer, int totalPrice)
    {
        var reservation = new Reservation(selectedMovie.Title, selectedPlaytime.DateTime, selectedSeats);
        customer.Reservations.Add(reservation);
        SaveCustomers(customer, "AccountInfo.json");
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
}