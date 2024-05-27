using Spectre.Console;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class ReservationSystem
{
    public static List<List<Seat>> Seats = new List<List<Seat>>();
    private static List<Seat> SelectedSeats = new List<Seat>();
    private static int x, y = 0;
    public static Movie? SelectedMovie;
    public static Playtime? SelectedPlaytime;

    public static void NavigateSeats()
    {
        if (SelectedMovie != null && SelectedPlaytime != null)
        {
            Seats = LoadSeats(SelectedMovie.Title, SelectedPlaytime.DateTime);
        }
        else
        {
            AnsiConsole.MarkupLine("[red]Geen film of speeltijd geselecteerd.[/]");
            return;
        }

        string unavailableMessage = string.Empty;
        bool displayNoSeatsSelectedMessage = false;

        DisplaySeats();

        while (true)
        {
            if (!string.IsNullOrEmpty(unavailableMessage))
            {
                AnsiConsole.MarkupLine(unavailableMessage);
                unavailableMessage = string.Empty;
            }

            if (displayNoSeatsSelectedMessage)
            {
                AnsiConsole.MarkupLine("[red]Geen stoel geselecteerd. Selecteer ten minste één stoel om door te gaan.[/]");
                displayNoSeatsSelectedMessage = false;
            }

            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    y = Math.Max(0, y - 1);
                    break;
                case ConsoleKey.DownArrow:
                    y = Math.Min(Seats.Count - 1 + 2, y + 1);
                    break;
                case ConsoleKey.LeftArrow:
                    if (y < Seats.Count)
                    {
                        x = Math.Max(0, x - 1);
                    }
                    break;
                case ConsoleKey.RightArrow:
                    if (y < Seats.Count)
                    {
                        x = Math.Min(Seats[y].Count - 1, x + 1);
                    }
                    break;
                case ConsoleKey.Enter:
                    if (y == Seats.Count)
                    {
                        if (SelectedSeats.Count > 0)
                        {
                            AnsiConsole.Clear();
                            ProceedToPayment();
                            return;
                        }
                        else
                        {
                            displayNoSeatsSelectedMessage = true;
                        }
                    }
                    else if (y == Seats.Count + 1)
                    {
                        SelectedSeats = new List<Seat>();
                        x = 0; y = 0;

                        Program.PreviousScreen();
                        return;
                    }
                    else
                    {
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
                                }
                                else
                                {
                                    AnsiConsole.MarkupLine("[red]U kunt maximaal 5 stoelen reserveren.[/]");
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                            else
                            {
                                unavailableMessage = "[red]Stoel niet beschikbaar[/]";
                            }
                        }
                    }
                    break;
            }

            DisplaySeats();
        }
    }

    public static void DisplaySeats()
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

        if (y == Seats.Count)
        {
            AnsiConsole.Write(new Text("[ Doorgaan ]", new Style(Color.Yellow, Color.Grey)).Centered());
            AnsiConsole.Write(new Text("[ Terug ]", new Style(Color.Yellow, Color.Black)).Centered());
        }
        else if (y == Seats.Count + 1)
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

    public static void UpdateSeatsAvailability()
    {
        if (SelectedSeats != null && SelectedSeats.Any())
        {
            foreach (var seat in SelectedSeats)
            {
                seat.IsAvailable = false;
            }

            if (SelectedMovie != null && SelectedPlaytime != null)
            {
                SaveSeats(SelectedMovie.Title, SelectedPlaytime.DateTime, Seats);
            }
        }
    }



    public static List<List<Seat>> LoadSeats(string movieTitle, DateTime playtime)
    {
        string json = File.ReadAllText("Data/MoviesAndPlaytimes.json");
        var movies = JsonConvert.DeserializeObject<List<Movie>>(json);

        var movie = movies.FirstOrDefault(m => m.Title == movieTitle);
        if (movie != null)
        {
            var selectedPlaytime = movie.Playtimes.FirstOrDefault(p => p.DateTime == playtime);
            if (selectedPlaytime != null)
            {
                return selectedPlaytime.Seats;
            }
        }
        return new List<List<Seat>>();
    }

    public static void SaveSeats(string movieTitle, DateTime playtime, List<List<Seat>> seats)
    {
        string json = File.ReadAllText("Data/MoviesAndPlaytimes.json");
        var movies = JsonConvert.DeserializeObject<List<Movie>>(json);

        var movie = movies.FirstOrDefault(m => m.Title == movieTitle);
        if (movie != null)
        {
            var selectedPlaytime = movie.Playtimes.FirstOrDefault(p => p.DateTime == playtime);
            if (selectedPlaytime != null)
            {
                selectedPlaytime.Seats = seats;
            }
        }

        string updatedJson = JsonConvert.SerializeObject(movies, Formatting.Indented);
        File.WriteAllText("Data/MoviesAndPlaytimes.json", updatedJson);
    }

    public static void ProceedToPayment()
    {
        if (SelectedMovie != null && SelectedPlaytime != null)
        {
            Betaalscherm betaalscherm = new Betaalscherm(SelectedSeats, SelectedMovie, SelectedPlaytime);
            SelectedSeats = new List<Seat>();
            Program.ShowScreen(betaalscherm.DisplayPaymentScreen);
        }
    }
}
