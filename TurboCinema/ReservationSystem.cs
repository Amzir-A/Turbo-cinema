using Spectre.Console;
using Newtonsoft.Json;

class ReservationSystem
{
    public static List<List<Seat>> Seats = LoadSeats();
    List<Seat> SelectedSeats = new List<Seat>();
    int x, y = 0;
    Movie SelectedMovie;
    Playtime selectedPlaytime;

    public ReservationSystem(Movie selectedMovie, Playtime selectedPlaytime)
    {
        NavigateSeats();
        this.SelectedMovie = selectedMovie;
        this.selectedPlaytime = selectedPlaytime;
    }

    public void NavigateSeats()
    {
        DisplaySeats();

        while (true)
        {
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    y = Math.Max(0, y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    y = Math.Min(Seats.Count - 1, y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    x = Math.Max(0, x - 1);
                    break;
                case ConsoleKey.RightArrow:
                    x = Math.Min(Seats[y].Count - 1, x + 1);
                    break;
                case ConsoleKey.Enter:
                    Seat selectedSeat = Seats[y][x];
                    if (SelectedSeats.Contains(selectedSeat))
                    {
                        SelectedSeats.Remove(selectedSeat);
                    }
                    else
                    {
                        if (selectedSeat.IsAvailable)
                        {
                            if (SelectedSeats.Count < 5)
                            {
                                SelectedSeats.Add(selectedSeat);
                                selectedSeat.IsAvailable = false;
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[red]U kunt maximaal 5 stoelen reserveren.[/]");
                                System.Threading.Thread.Sleep(2000);
                            }
                        }
                    }
                    break;

                case ConsoleKey.Spacebar:
                    if (SelectedSeats.Count > 0)
                    {
                        ProceedToPayment();
                        return; 
                    }
                    AnsiConsole.MarkupLine("[red]Geen stoel geselecteerd. Selecteer ten minste één stoel om door te gaan.[/]");
                    break;
            }

            DisplaySeats();
        }
    }



    public void DisplaySeats()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new Text("------------------------------ [ SCHERM ] ------------------------------", new Style(Color.Yellow, Color.Black)).Centered());
        AnsiConsole.WriteLine();

        Table tableSeats = new Table().Centered();
        tableSeats.Border = TableBorder.None;
        tableSeats.AddColumns("", "", "", "");

        for (int i = 0; i < Seats.Count; i++)
        {
            List<Panel> row = new List<Panel>();
            for (int j = 0; j < Seats[i].Count; j++)
            {
                string color = Seats[i][j].IsAvailable ? "green" : "red";
                Color bgColor = Color.Red;

                if (i == y && j == x)
                {
                    bgColor = Color.Yellow;
                }

                string check = " ";
                if (SelectedSeats.Contains(Seats[i][j]))
                {
                    check = "✓";
                }

                row.Add(
                    new Panel(new Markup($"[white on {color}]  {Seats[i][j].ID} {check}  [/]"))
                    {
                        Border = BoxBorder.Heavy,
                        BorderStyle = new Style(bgColor)
                    }
                );
            }
            tableSeats.AddRow(row.ToArray());
        }

        AnsiConsole.Write(tableSeats);
        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Text("[ Projector ]", new Style(Color.Yellow, Color.Black)).Centered());

        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine("Druk op spatie om keuze te bevestigen.");
    }
    public static void SaveSeats()
    {
        string json = JsonConvert.SerializeObject(Seats, Formatting.Indented);
        File.WriteAllText("Data/Reservations.json", json);
    }


    public int SelectSeats()
    {
        return 0;
    }

    public static List<List<Seat>> LoadSeats()
    {
        string json = File.ReadAllText("Data/Reservations.json");
        List<List<Seat>>? seats = JsonConvert.DeserializeObject<List<List<Seat>>>(json);
        return seats ?? new List<List<Seat>>();
    }

    public void ProceedToPayment()
    {
        if (SelectedMovie != null && selectedPlaytime != null)
        {
            Betaalscherm betaalscherm = new Betaalscherm(SelectedSeats, SelectedMovie, selectedPlaytime);
            betaalscherm.DisplayPaymentScreen();
        }
    }
}