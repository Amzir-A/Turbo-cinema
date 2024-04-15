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

            var firstName = AnsiConsole.Ask<string>("Voornaam[/]?");
            var lastName = AnsiConsole.Ask<string>("Achternaam[/]?");
            var dateOfBirth = AnsiConsole.Ask<string>("Geboortedatum(JJJJ-MM-DD)[/]?");
            var password = AnsiConsole.Prompt(
                            new TextPrompt<string>("Kies een wachtwoord[/]")
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

        private void SaveCustomers(List<Customer> customers, string fileName)
        {
            string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(fileName, json);
        }
    }
