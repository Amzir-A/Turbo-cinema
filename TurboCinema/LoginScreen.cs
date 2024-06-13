using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.VisualBasic;
using Spectre.Console;
using System.Text.RegularExpressions;

public static class LoginScreen
{
    static string screenName = "Login scherm";
    public static bool IsAdmin { get; set; } = false;
    static string queue = "";
    static string queue2 = "";

    static Dictionary<string, string> QI;
    static Dictionary<string, string> QI2;

    public static void LoginMenu()
    {
        QI = new Dictionary<string, string>
        {
            { "Voornaam", "" },
            { "Achternaam", "" },
            { "Geboortedatum", "" },
            { "Email", "" },
            { "Postcode", "" },
            { "Wachtwoord", "" }
        };

        QI2 = new Dictionary<string, string>
        {
            { "Email", "" },
            { "Wachtwoord", "" }
        };

        while (true)
        {
            Console.Clear();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welkom bij TurboCinema!")
                    .PageSize(10)
                    .AddChoices(new[] { "Reserveringen bekijken", "Maak een account", "Terug naar hoofdmenu" }));

            switch (option)
            {
                case "Reserveringen bekijken":
                    Login();
                    break;
                case "Maak een account":
                    Register();
                    break;
                case "Terug naar hoofdmenu":
                    Program.PreviousScreen();
                    break;
                default:
                    break;
            }
        }
    }


public static void Register()
    {
        int index = 0;

        Console.Clear();
        CE.WL("\n");
        AnsiConsole.Write(new Text("Welkom bij TurboCinema! Maak een account aan om te beginnen.", new Style(Color.Yellow, Color.Black)).Centered());
        Console.CursorVisible = false;  // Hide the cursor

        // Re-render the entire form
        RenderForm(QI, index);
        RenderFormButtons(QI, index);

        while (true)
        {
            var key = Console.ReadKey(true);

            
            var question = QI.ElementAt(Math.Min(index, QI.Count-1)).Key;
            var input = QI.ElementAt(Math.Min(index, QI.Count-1)).Value;
        

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (index > 0)
                        index--;
                    break;
                case ConsoleKey.DownArrow:
                    if (index < QI.Count + 1)
                        index++;
                    break;
                case ConsoleKey.Backspace:
                    // Use index to get the question and remove the last character from the input
                    if (index < QI.Count)
                    {if (input.Length > 0)
                        QI[question] = input.Remove(input.Length - 1);}
                    break;
                case ConsoleKey.Tab:
                    if (index < QI.Count - 1)
                        index++;
                    break;
                
                case ConsoleKey.LeftArrow:
                break;
                case ConsoleKey.RightArrow:
                break;

                case ConsoleKey.Enter:
                    if (index < QI.Count - 1)
                        index++;
                    if (index == QI.Count + 1)
                    {
                        Program.ShowScreen(MainScreen.MainMenu);
                    }
                    else if (index == QI.Count)
                    {
                        string errorMsg = ValidateInputs();
                        if (errorMsg == "")
                        {
                            Console.Clear();
                            CE.WL("\n");
                            AnsiConsole.Write(new Text("Welkom bij TurboCinema! Maak een account aan om te beginnen.", new Style(Color.Yellow, Color.Black)).Centered());
                            RenderForm(QI, index);
                            RenderFormButtons(QI, index);
                            queue = "";
                            
                            // Register the user
                            List<Customer> customers = LoadCustomers("Data/AccountInfo.json");


                            Customer newCustomer = new Customer
                            {
                                FirstName = QI["Voornaam"],
                                LastName = QI["Achternaam"],
                                DateOfBirth = DateTime.Parse(QI["Geboortedatum"]),
                                Email = QI["Email"],
                                Postcode = QI["Postcode"],
                                Password = HashPassword(QI["Wachtwoord"]),
                                Reservations = new List<Reservation>()
                            };

                            customers.Add(newCustomer);
                            SaveCustomers(customers, "Data/AccountInfo.json");
                            
                            CE.WL("\n");
                            AnsiConsole.MarkupLine("[green]Account succesvol aangemaakt![/]");
                            AnsiConsole.MarkupLine("tik op enter om terug te gaan naar het hoofdmenu.");
                            Console.ReadLine();

                            Program.ShowScreen(MainScreen.MainMenu);
                            return;
                        } else {
                            queue = errorMsg;
                        }
                    }
                    break;
                default:
                    // Use index to get the question and put the input in the dictionary
                    if (index < QI.Count)
                        QI[question] = input + key.KeyChar;
                    break;
            }

            // Re-render the entire form
            RenderForm(QI, index);
            RenderFormButtons(QI, index);

            // Render the error message
            if (queue != "")
            {
                Console.SetCursorPosition(0, 17);
                AnsiConsole.Write(new Text(queue, new Style(Color.Red, Color.Black)));
            }

        }
    }

    private static void RenderFormButtons(Dictionary<string, string> QI, int index)
    {
        // redner doorgaan en terug buttons
        if (index == QI.Count)
        {
            Console.SetCursorPosition(0, 14);
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Grey)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
        }
        else if(index == QI.Count + 1)
        {
            Console.SetCursorPosition(0, 14);
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Grey)).Centered());
        }
        else {
            Console.SetCursorPosition(0, 14);
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
        
        }
    }

    private static void RenderForm(Dictionary<string, string> QI, int index)
    {
        // Reset cursor position
        Console.SetCursorPosition(0, 5);

        // Render each question and input
        for (int i = 0; i < QI.Count; i++)
        {
            var q = QI.ElementAt(i);
            if (i == index)
            {
                Console.Write("> ");
                AnsiConsole.Write(new Text($"{q.Key}: {q.Value}".PadRight(Console.WindowWidth - 2), new Style(Color.Blue, Color.Black)));
            }
            else
            {
                Console.Write("  ");
                AnsiConsole.Write(new Text($"{q.Key}: {q.Value}".PadRight(Console.WindowWidth - 2), new Style(Color.White, Color.Black)));
            }
            
        }

        // Clear any remaining lines from previous renders
        int currentLineCursor = Console.CursorTop;
        for (int i = currentLineCursor; i < Console.WindowHeight - 1; i++)
        {
            Console.SetCursorPosition(0, i+1);
            Console.Write(new string(' ', Console.WindowWidth));
        }


        // Reset cursor to the position where the next input should be
        if (index < QI.Count)
        {    var currentInput = QI.ElementAt(index).Value;
            int inputLine = 1 + index;
            int inputColumn = 2 + QI.ElementAt(index).Key.Length + 2 + currentInput.Length;
            Console.SetCursorPosition(inputColumn, inputLine);}
    }


    public static string ValidateInputs()
    {
        // Create a list to store the invalid inputs
        List<string> invalidInputs = new List<string>();

        // Validate inputs
        foreach (var q in QI)
        {
            if (q.Value == "")
            {
                return "Vul alle velden in.";
            }
        }

        // Validate email
        if (!IsValidEmail(QI["Email"]))
        {
            invalidInputs.Add("Ongeldige email.");
        }

        // Validate password
        if (!IsValidPassword(QI["Wachtwoord"]))
        {
            invalidInputs.Add("Ongeldig wachtwoord. Voer minimaal 5 tekens, één hoofdletter en één cijfer in.");
        }

        // Validate Name
        if (!IsValidName(QI["Voornaam"]) || !IsValidName(QI["Achternaam"]))
        {
            invalidInputs.Add("Ongeldige naam.");
        }

        // validate date
        if (!DateTime.TryParse(QI["Geboortedatum"], out _))
        {
            invalidInputs.Add("Ongeldige geboortedatum(MM-DD-JJJJ).");
        }

        return string.Join(" ", invalidInputs);
    }




    static bool IsValidName(string name)
    {
        foreach (char c in name)
        {
            if (c == ' ') 
                return true;

            if (!char.IsLetter(c))
            {
                return false;
            }
        }
        return true;
    }


    static bool IsValidPassword(string password)
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

    public static string HashPassword(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    private static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        try
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static List<Customer> LoadCustomers(string fileName)
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

    

    private static void RenderLogin(Dictionary<string, string> QI2, int index)
    {
        // Reset cursor position
        Console.SetCursorPosition(0, 5);

        // Render each question and input
        for (int i = 0; i < QI2.Count; i++)
        {
            var q = QI2.ElementAt(i);
            if (i == index)
            {
                Console.Write("> ");
                if (q.Key == "Wachtwoord")
                {
                    string hidden = "";
                    for (int j = 0; j < q.Value.Length; j++)
                    {
                        hidden += "*";
                    }
                    AnsiConsole.Write(new Text($"{q.Key}: {hidden}".PadRight(Console.WindowWidth - 2), new Style(Color.Blue, Color.Black)));
                }
                else
                    AnsiConsole.Write(new Text($"{q.Key}: {q.Value}".PadRight(Console.WindowWidth - 2), new Style(Color.Blue, Color.Black)));
            }
            else
            {
                Console.Write("  ");
                if (q.Key == "Wachtwoord")
                {
                    string hidden = "";
                    for (int j = 0; j < q.Value.Length; j++)
                    {
                        hidden += "*";
                    }
                    AnsiConsole.Write(new Text($"{q.Key}: {hidden}".PadRight(Console.WindowWidth - 2), new Style(Color.White, Color.Black)));
                }
                else
                    AnsiConsole.Write(new Text($"{q.Key}: {q.Value}".PadRight(Console.WindowWidth - 2), new Style(Color.White, Color.Black)));
            }
            
        }

        // Clear any remaining lines from previous renders
        int currentLineCursor = Console.CursorTop;
        for (int i = currentLineCursor; i < Console.WindowHeight - 1; i++)
        {
            Console.SetCursorPosition(0, i+1);
            Console.Write(new string(' ', Console.WindowWidth));
        }

        // Reset cursor to the position where the next input should be
        if (index < QI2.Count)
        {    var currentInput = QI.ElementAt(index).Value;
            int inputLine = 1 + index;
            int inputColumn = 2 + QI.ElementAt(index).Key.Length + 2 + currentInput.Length;
            Console.SetCursorPosition(inputColumn, inputLine);}
    }

    static void renderloginbutton(Dictionary<string, string> QI2, int index)
    {
        Console.SetCursorPosition(0, 10);
        if (index == QI2.Count)
        {
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Grey)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
        }
        else if (index == QI2.Count + 1)
        {
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Grey)).Centered());
        }
        else
        {
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Black)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
        }
    }


    public static void Login()
    {
        List<Customer> customers = LoadCustomers("Data/AccountInfo.json");
        int index = 0;

        Console.Clear();
        CE.WL("\n");
        AnsiConsole.Write(new Text("Welkom bij TurboCinema! Log in om verder te gaan.", new Style(Color.Yellow, Color.Black)).Centered());
        Console.CursorVisible = false;  // Hide the cursor
        RenderLogin(QI2, index);
        renderloginbutton(QI2, index);

        while (true)
        {
            var key = Console.ReadKey(true);

            
            var question = QI2.ElementAt(Math.Min(index, QI2.Count-1)).Key;
            var input = QI2.ElementAt(Math.Min(index, QI2.Count-1)).Value;

        

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (index > 0)
                        index--;
                    break;
                case ConsoleKey.DownArrow:
                    if (index < QI2.Count + 1)
                        index++;
                    break;
                case ConsoleKey.Backspace:
                    // Use index to get the question and remove the last character from the input
                    if (index < QI2.Count)
                    {if (input.Length > 0)
                        QI2[question] = input.Remove(input.Length - 1);}
                    break;
                case ConsoleKey.Tab:
                    if (index < QI2.Count - 1)
                        index++;
                    break;
                
                case ConsoleKey.LeftArrow:
                break;
                case ConsoleKey.RightArrow:
                break;

                case ConsoleKey.Enter:
                    if (index < QI2.Count)
                        index++;
                    if (index == QI2.Count + 1)
                    {
                        Program.ShowScreen(MainScreen.MainMenu);
                    }
                    else if (index == QI2.Count)
                    {
                        queue = "";
                        Console.Clear();
                        CE.WL("\n");
                        AnsiConsole.Write(new Text("Welkom bij TurboCinema! Log in om verder te gaan.", new Style(Color.Yellow, Color.Black)).Centered());
                        Console.WriteLine("\n\n");
                        RenderLogin(QI2, index);

                        string hashedPassword = HashPassword(QI2["Wachtwoord"]);
                        
                        var customer = customers.Find(c => c.Email == QI2["Email"] && c.Password == hashedPassword);

                        if (QI2["Email"] == "Admin@1324.com" && QI2["Wachtwoord"] == "Admin1234")
                        {
                            AnsiConsole.MarkupLine("[green]Je bent succesvol ingelogd als admin![/]");
                            IsAdmin = true;
                            CE.WL("\n");
                            Console.WriteLine("Druk op Enter om terug te gaan naar het hoofdmenu");
                            Console.ReadLine();
                            Program.ShowScreen(MainScreen.MainMenu);
                        }

                        else if (customer != null)
                        {
                            int index2 = 0;
                            while (true)
                            {
                                Console.Clear();

                                AnsiConsole.MarkupLine("[green]Je bent succesvol ingelogd![/]");
                                string reservationInfo = string.Join("\n", customer.Reservations.Select(r =>
                                $"- {r.MovieTitle} op {r.Playtime:g} in zaal: {r.Room} met de stoelen {string.Join(", ", r.Seats.Select(s => s.ID))}"));

                                AnsiConsole.Write(new Panel(new Markup(
                                    $"[bold]Voornaam:[/] {customer.FirstName}\n" +
                                    $"[bold]Achternaam:[/] {customer.LastName}\n" +
                                    $"[bold]Geboortedatum:[/] {customer.DateOfBirth}\n" +
                                    $"[bold]Postcode:[/] {customer.Postcode}\n" +
                                    $"[bold]Boekingen:[/]\n {reservationInfo}\n" +
                                    $"[bold]Email:[/] {customer.Email}"))
                                    .Expand()
                                    .Padding(1, 1)
                                    .SquareBorder());

                                AnsiConsole.MarkupLine("Wil je (nog) een reservering annuleren? [red]LET OP: Bij herhaadelijk annuleren kan uw account gedeactiveerd worden. [/]");

                                if (index2 == 0)
                                {
                                    AnsiConsole.Write(new Text("[ Ja ]", new Style(Color.Yellow, Color.Grey)).Centered());
                                    AnsiConsole.Write(new Text("[ Nee ]", new Style(Color.Yellow, Color.Black)).Centered());
                                }
                                else if (index2 == 1)
                                {
                                    AnsiConsole.Write(new Text("[ Ja ]", new Style(Color.Yellow, Color.Black)).Centered());
                                    AnsiConsole.Write(new Text("[ Nee ]", new Style(Color.Yellow, Color.Grey)).Centered());
                                }

                                if (queue != "")
                                {
                                    Console.SetCursorPosition(0, 15);
                                    AnsiConsole.Write(new Text(queue, new Style(Color.Red, Color.Black)));
                                }

                                if (queue2 != "")
                                {
                                    Console.SetCursorPosition(0, 15);
                                    AnsiConsole.Write(new Text(queue2, new Style(Color.Green, Color.Black)));
                                }

                                var key2 = Console.ReadKey(true);
                                
                                switch (key2.Key)
                                {
                                    case ConsoleKey.UpArrow:
                                        if (index2 > 0)
                                            index2--;
                                        break;
                                    case ConsoleKey.DownArrow:
                                        if (index2 < 2)
                                            index2++;
                                        break;
                                    case ConsoleKey.Enter:
                                        if (index2 == 0)
                                        {
                                            CancelReservation(customer, customers);
                                        } else if (index2 == 1)
                                        {
                                            Program.ShowScreen(MainScreen.MainMenu);
                                        }
                                        break;
                                }

                            }
                            

                        }
                        else {
                            queue = "Inloggen mislukt. Incorrect email-adres of wachtwoord.";
                        }
                    }
                    break;
                default:
                    // Use index to get the question and put the input in the dictionary
                    if (index < QI2.Count)
                        QI2[question] = input + key.KeyChar;
                    break;
            }

            // Re-render the entire form
            RenderLogin(QI2, index);
            renderloginbutton(QI2, index);

            if (queue != "")
            {
                Console.SetCursorPosition(0, 14);
                AnsiConsole.Write(new Text(queue, new Style(Color.Red, Color.Black)));
            }
            queue = "";

            if (queue2 != "")
            {
                Console.SetCursorPosition(0, 14);
                AnsiConsole.Write(new Text(queue2, new Style(Color.Green, Color.Black)));
            }

            queue2 = "";
        }

    }


    public static void SaveCustomers(List<Customer> customers, string fileName)
    {
        string json = JsonSerializer.Serialize(customers, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fileName, json);
    }

    private static void CancelReservation(Customer customer, List<Customer> customers)
    {
        if (customer.Reservations.Count == 0)
        {
            queue = "Geen reserveringen om te annuleren.";
            return;
        }

        var reservationTitles = customer.Reservations.Select((r, index) => $"{index + 1}. {r.MovieTitle} op {r.Playtime:g}").ToList();
        var selectedReservation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Selecteer een reservering om te annuleren")
                .PageSize(10)
                .AddChoices(reservationTitles));

        int reservationIndex = reservationTitles.IndexOf(selectedReservation);
        var reservationToCancel = customer.Reservations[reservationIndex];

        var movie = ReservationSystem.LoadSeats(reservationToCancel.MovieTitle, reservationToCancel.Playtime);
        foreach (var seat in reservationToCancel.Seats)
        {
            var seatToUpdate = movie.SelectMany(row => row).FirstOrDefault(s => s.ID == seat.ID);
            if (seatToUpdate != null)
            {
                seatToUpdate.IsAvailable = true;
            }
        }
        ReservationSystem.SaveSeats(reservationToCancel.MovieTitle, reservationToCancel.Playtime, movie);

        customer.Reservations.Remove(reservationToCancel);
        SaveCustomers(customers, "Data/AccountInfo.json");

        queue2 = "Reservering succesvol geannuleerd! Uw geld wordt binnen 2-3 werkdagen teruggestort.";
    }
    public static void CancelReservation(Customer customer, List<Customer> customers, int reservationIndex)
    {
        if (customer.Reservations.Count == 0)
        {
            queue = "Geen reserveringen om te annuleren.";
            return;
        }

        if (reservationIndex < 0 || reservationIndex >= customer.Reservations.Count)
        {
            queue = "Ongeldige reserveringsindex.";
            return;
        }

        var reservationToCancel = customer.Reservations[reservationIndex];

        var movie = ReservationSystem.LoadSeats(reservationToCancel.MovieTitle, reservationToCancel.Playtime, "Data/TestMoviesAndPlaytimes.json");
        foreach (var seat in reservationToCancel.Seats)
        {
            var seatToUpdate = movie.SelectMany(row => row).FirstOrDefault(s => s.ID == seat.ID);
            if (seatToUpdate != null)
            {
                seatToUpdate.IsAvailable = true;
            }
        }
        ReservationSystem.SaveSeats(reservationToCancel.MovieTitle, reservationToCancel.Playtime, movie, "Data/TestMoviesAndPlaytimes.json");

        customer.Reservations.Remove(reservationToCancel);
        SaveCustomers(customers, "Data/AccountInfo.json");

        queue2 = "[green]Reservering succesvol geannuleerd! Uw geld wordt binnen 2-3 werkdagen teruggestort.[/]";
    }
}
