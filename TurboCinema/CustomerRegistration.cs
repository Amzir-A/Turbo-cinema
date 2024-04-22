using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Spectre.Console;


    public class AccountRegistration
    {
        public void Register()
        {
            List<Customer> customers = LoadCustomers("AccountInfo.json");

            Console.WriteLine("welkom bij TurboCinema! Maak een account aan om te beginnen.");
            
            var firstName = AnsiConsole.Ask<string>("Wat is je voornaam?");
            var lastName = AnsiConsole.Ask<string>("Wat is je achternaam?");
            var dateOfBirth = AnsiConsole.Ask<string>("Wat is je geboortedatum?");
            var Email = AnsiConsole.Ask<string>("Wat is je email?");
            var Postcode = AnsiConsole.Ask<string>("Wat is je postcode?");
            var password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Voer een wachtwoord in")
                                .PromptStyle("red")
                                .Secret());
            int customerId = customers.Count + 1;

            Customer newCustomer = new Customer
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Password = password,
                Email = Email,
                Postcode = Postcode,
                Reservations = new List<Reservation>()
                };

            customers.Add(newCustomer);
            SaveCustomers(customers, "AccountInfo.json");

        }

        private List<Customer> LoadCustomers(string fileName)
        {
            List<Customer> customers;

            if (File.Exists(fileName))
            {
                string json = File.ReadAllText(fileName);
                customers = JsonSerializer.Deserialize<List<Customer>>(json);
            }
            else
            {
                customers = new List<Customer>();
            }

            return customers;
        }

    public void Login()
    {
        List<Customer> customers = LoadCustomers("AccountInfo.json");

        var email = AnsiConsole.Ask<string>("Voer je email");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer je password in")
                .PromptStyle("red")
                .Secret());

        var customer = customers.Find(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            AnsiConsole.MarkupLine("[green]You have successfully logged in![/]");
            string reservationInfo = string.Join("\n", customer.Reservations.Select(r =>
            $"- {r.MovieTitle} on {r.PlayTime:g} in Room: {r.Room} with seats {string.Join(", ", r.SelectedSeats.Select(s => s.ID))}"));

            AnsiConsole.Write(new Panel(new Markup(
                $"[bold]voornaam:[/] {customer.FirstName}\n" +
                $"[bold]achternaam:[/] {customer.LastName}\n" +
                $"[bold]Geboortedatum:[/] {customer.DateOfBirth}\n" +
                $"[bold]Postcode:[/] {customer.Postcode}\n" +
                $"[bold] Bookings:[/]\n {reservationInfo}\n" +
                $"[bold]Email:[/] {customer.Email}"))
                .Expand()
                .Padding(1, 1)
                .SquareBorder());
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Login failed. Incorrecte email of password.[/]");
        }
    }

    private void SaveCustomers(List<Customer> customers, string fileName)
    {
        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }
}