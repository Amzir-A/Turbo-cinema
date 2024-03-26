using Spectre.Console;
using Newtonsoft.Json;

class ReservationSystem
{
    public static List<Seat> Seats = LoadSeats();

    public ReservationSystem()
    {
        var grid = new Grid();
        grid.AddColumns(10);
        grid.AddRow(new Text[]{
            new Text("ID", new Style(Color.Red, Color.Black)).Centered(),
            new Text("Available", new Style(Color.Red, Color.Black)).Centered()
        });

        for (int i = 0; i < Seats.Count; i++)
        {
            var seat = Seats[i];
            grid.AddRow(new Text[]{
                new Text((i + 1).ToString(), new Style(Color.White, Color.Black)).Centered(),
                new Text(seat.IsAvailable ? "O" : "X", new Style(Color.White, Color.Black)).Centered()
            });
        }

        AnsiConsole.Write(grid);
    }

    public int SelectSeats()
    {
        return AnsiConsole.Prompt(
        new TextPrompt<int>("Select seat by ID")
            .PromptStyle("green")
            .ValidationErrorMessage("[red]That's not a valid seat ID[/]")
            .Validate(ID =>
            {
                {
                    if (ID < 1)
                    {
                        return ValidationResult.Error("[red]ID can't be a negative number.[/]");
                    }
                    else if (ID > Seats?.Count)
                    {
                        return ValidationResult.Error("[red]ID doesn't exist in the list.[/]");
                    }
                    else if (Seats?[ID - 1].IsAvailable == false)
                    {
                        return ValidationResult.Error("[red]Seat isn't available.[/]");
                    }
                    else
                    {
                        return ValidationResult.Success();
                    }

                }
            })
        );
    }

    public static List<Seat> LoadSeats()
    {
        string json = File.ReadAllText("Data/Reservations.json");
        List<Seat>? seats = JsonConvert.DeserializeObject<List<Seat>>(json);
        return seats ?? new List<Seat>();
    }
}