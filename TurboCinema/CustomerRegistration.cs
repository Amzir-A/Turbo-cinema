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

            Console.WriteLine("Welcome to Customer Registration!");

            // Corrected the markup tags by removing them
            var firstName = AnsiConsole.Ask<string>("What is your first name?");
            var lastName = AnsiConsole.Ask<string>("What is your last name?");
            var dateOfBirth = AnsiConsole.Ask<string>("What is your date of birth (YYYY-MM-DD)?");
            var Email = AnsiConsole.Ask<string>("What is your email address?");
            var password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Please choose a password")
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

        var email = AnsiConsole.Ask<string>("What is your email?");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Please enter your password")
                .PromptStyle("red")
                .Secret());

        var customer = customers.Find(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            AnsiConsole.MarkupLine("[green]You have successfully logged in![/]");
            // Use AnsiConsole to display customer details
            AnsiConsole.Write(new Panel(new Markup(
                $"[bold]First Name:[/] {customer.FirstName}\n" +
                $"[bold]Last Name:[/] {customer.LastName}\n" +
                $"[bold]Date of Birth:[/] {customer.DateOfBirth}\n" +
                $"[bold]Email:[/] {customer.Email}"))
                .Expand()
                .Padding(1, 1)
                .SquareBorder());
            // Do not display the password for security reasons
            // Display other details if necessary
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Login failed. Incorrect email or password.[/]");
        }
    }

    private void SaveCustomers(List<Customer> customers, string fileName)
    {
        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }
}
