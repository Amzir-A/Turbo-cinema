using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Spectre.Console;


public class AccountRegistration
{
    public void Register()
    {
        List<Customer> customers = LoadCustomers("Data/AccountInfo.json");

    Console.WriteLine("Welkom bij TurboCinema! Maak een account aan om te beginnen.");

        var firstName = ValidateName("Wat is je voornaam?");
        var lastName = ValidateName("Wat is je achternaam?");
        var dateOfBirth = ValidateDateOfBirth("Wat is je geboortedatum? (MM-DD-YYYY)");
        var email = ValidateEmail("Wat is je email?");
        var postcode = AnsiConsole.Ask<string>("Wat is je postcode?");
        var password = ValidatePassword();

        Customer newCustomer = new Customer
        {
            ID = customers.Count + 1,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            Email = email,
            Postcode = postcode,
            Password = password,
            Reservations = new List<Reservation>()
            };

        customers.Add(newCustomer);
        SaveCustomers(customers, "Data/AccountInfo.json");

    }
    private static string ValidateName(string prompt)
    {
        string name;
        do
        {
            name = AnsiConsole.Ask<string>(prompt);
            if (!IsValidName(name))
            {
                AnsiConsole.MarkupLine("[red]Ongeldige invoer. Voer alleen letters in.[/]");
            }
        } while (!IsValidName(name));
        return name;
    }
    private static bool IsValidName(string name)
    {
        foreach (char c in name)
        {
            if (!char.IsLetter(c))
            {
                return false;
            }
        }
        return true;
    }

    private static DateTime ValidateDateOfBirth(string prompt)
    {
        DateTime dateOfBirth;
        do
        {
            var input = AnsiConsole.Ask<string>(prompt);
            if (!DateTime.TryParse(input, out dateOfBirth))
            {
                AnsiConsole.MarkupLine("[red]Ongeldige invoer. Voer de geboortedatum in het juiste formaat (bijv. 01-01-1990).[/]");
            }
        } while (dateOfBirth == DateTime.MinValue);
        return dateOfBirth;
    }
    private static string ValidatePassword()
    {
        string password;
        do
        {
            password = AnsiConsole.Prompt(
                new TextPrompt<string>("Voer een wachtwoord in (minstens 5 tekens, minstens één hoofdletter en één cijfer)")
                    .PromptStyle("red")
                    .Secret());
            if (!IsValidPassword(password))
            {
                AnsiConsole.MarkupLine("[red]Ongeldig wachtwoord. Het wachtwoord moet minstens 5 tekens lang zijn en minstens één hoofdletter en één cijfer bevatten.[/]");
            }
        } while (!IsValidPassword(password));
        return password;
    }

    private static bool IsValidPassword(string password)
    {
        if (password.Length < 5)
        {
            return false;
        }
        
        bool hasCapital = false;
        foreach (char c in password)
        {
            if (char.IsUpper(c))
            {
                hasCapital = true;
                break;
            }
        }
        
        bool hasNumber = false;
        foreach (char c in password)
        {
            if (char.IsDigit(c))
            {
                hasNumber = true;
                break;
            }
        }
        
        return hasCapital && hasNumber;
    }
    private static string ValidateEmail(string prompt)
    {
        string email;
        do
        {
            email = AnsiConsole.Ask<string>(prompt);
            if (!IsValidEmail(email))
            {
                AnsiConsole.MarkupLine("[red]Ongeldige invoer. Voer een geldig e-mailadres in.[/]");
            }
        } while (!IsValidEmail(email));
        return email;
    }

    private static bool IsValidEmail(string email)
    {
        return email.Contains("@");
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
        List<Customer> customers = LoadCustomers("Data/AccountInfo.json");

        var email = AnsiConsole.Ask<string>("Voer je email in");
        var password = AnsiConsole.Prompt(
            new TextPrompt<string>("Voer je password in")
                .PromptStyle("red")
                .Secret());

        var customer = customers.Find(c => c.Email == email && c.Password == password);

        if (customer != null)
        {
            AnsiConsole.MarkupLine("[green]Je bent succesvol ingelogd![/]");
            string reservationInfo = string.Join("\n", customer.Reservations.Select(r =>
            $"- {r.MovieTitle} op {r.PlayTime:g} in zaal: {r.Room} met de stoelen {string.Join(", ", r.SelectedSeats.Select(s => s.ID))}"));

            AnsiConsole.Write(new Panel(new Markup(
                $"[bold]Voornaam:[/] {customer.FirstName}\n" +
                $"[bold]Achternaam:[/] {customer.LastName}\n" +
                $"[bold]Geboortedatum:[/] {customer.DateOfBirth}\n" +
                $"[bold]Postcode:[/] {customer.Postcode}\n" +
                $"[bold] Bookings:[/]\n {reservationInfo}\n" +
                $"[bold]Email:[/] {customer.Email}"))
                .Expand()
                .Padding(1, 1)
                .SquareBorder());
            
            AnsiConsole.MarkupLine("Wil je een reservatie annuleren? (ja/nee)");
            var cancelReservation = Console.ReadLine();
            if (cancelReservation != null && cancelReservation.ToLower() == "ja")
            {
                CancelReservation(customer, customers);
            }
            else if (cancelReservation == "nee")
            {
                Program.DisplayMainMenu();
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Login gefaald. Incorrecte email of wachtwoord.[/]");
        }
    }

<<<<<<< Updated upstream:TurboCinema/CustomerRegistration.cs
    private void SaveCustomers(List<Customer> customers, string fileName)
=======
        private static void CancelReservation(Customer customer, List<Customer> customers)
    {
        if (customer.Reservations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]Geen reserveringen om te annuleren.[/]");
            return;
        }

        var reservationTitles = customer.Reservations.Select((r, index) => $"{index + 1}. {r.MovieTitle} op {r.PlayTime:g}").ToList();
        var selectedReservation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer een reservering om te annuleren")
                .PageSize(10)
                .AddChoices(reservationTitles));

        int reservationIndex = reservationTitles.IndexOf(selectedReservation);
        var reservationToCancel = customer.Reservations[reservationIndex];
        customer.Reservations.Remove(reservationToCancel);

        SaveCustomers(customers, "Data/AccountInfo.json");
        AnsiConsole.MarkupLine("[green]Reservering succesvol geannuleerd![/]");
    }

    static void SaveCustomers(List<Customer> customers, string fileName)
>>>>>>> Stashed changes:TurboCinema/LoginScreen.cs
    {
        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }
}
