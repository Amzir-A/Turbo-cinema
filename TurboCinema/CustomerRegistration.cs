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

            Console.WriteLine("Welkom bij TurboCinema!");
            
            var firstName = AnsiConsole.Ask<string>("Voornaam: ");
            var lastName = AnsiConsole.Ask<string>("Achternaam: ");
            var dateOfBirth = AnsiConsole.Ask<string>("Geboortedatum (YYYY-MM-DD): ");
            var Email = AnsiConsole.Ask<string>("E-mail adres: ");
            var password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Kies een wachtwoord: ")
                                .PromptStyle("red")
                                .Secret());
            int customerId = customers.Count + 1;

            Customer newCustomer = new Customer
            {
                CustomerId = customerId,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Password = password
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

        var email = AnsiConsole.Ask<string>("E-Mail Adres: ");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Wachtwoord: ")
                .PromptStyle("red")
                .Secret());

        var customer = customers.Find(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            AnsiConsole.MarkupLine("[green]Inloggen succesvol![/]");
            AnsiConsole.Write(new Panel(new Markup(
                $"[bold]Voornaam:[/] {customer.FirstName}\n" +
                $"[bold]Achternaam:[/] {customer.LastName}\n" +
                $"[bold]Geboortedatum:[/] {customer.DateOfBirth}\n" +
                $"[bold]E-mail:[/] {customer.Email}"))
                .Expand()
                .Padding(1, 1)
                .SquareBorder());
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Inloggen mislukt. E-Mail of wachtwoord incorrect.[/]");
        }
    }

    private void SaveCustomers(List<Customer> customers, string fileName)
    {
        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }
}
