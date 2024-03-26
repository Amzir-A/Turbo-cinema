using Spectre.Console;
using Newtonsoft.Json;


class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        AnsiConsole.Write(
            new FigletText("TurboCinema")
                .Centered()
                .Color(Color.Red));

        AnsiConsole.WriteLine();

        MovieSelector movieSelector = new MovieSelector();
        movieSelector.DisplayMovies();
        var selectedMovie = movieSelector.SelectMovie();

        // select seats
        var reservationSystem = new ReservationSystem();
        var selectedSeat = reservationSystem.SelectSeats();

        var naar = AnsiConsole.Prompt(new ConfirmationPrompt("Naar betaalscherm?"));
        if (naar)
        {
            AnsiConsole.Clear();
            var betaalscherm = new Betaalscherm();
        }

        Console.ReadLine();
    }
}

