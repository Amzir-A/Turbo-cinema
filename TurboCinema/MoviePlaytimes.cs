using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using Spectre.Console;

public class MoviePlaytimes
{
    public string Title { get; set; }
    public List<Playtime> Playtimes { get; set; }

    public static List<MoviePlaytimes> LoadFromJson(string filePath)
    {
        string jsonContent = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<MoviePlaytimes>>(jsonContent);
    }
    public static void DisplayMoviesAndTimes()
    {
        // Load movies and their playtimes
        string filePath = "playtimes.json"; // Ensure this is the correct path
        var moviesWithPlaytimes = MoviePlaytimes.LoadFromJson(filePath);

        foreach (var movie in moviesWithPlaytimes)
        {
            AnsiConsole.MarkupLine($"[underline green]{movie.Title}[/]");
            foreach (var playtime in movie.Playtimes)
            {
                AnsiConsole.MarkupLine($"DateTime: [yellow]{playtime.DateTime}[/], Room: [blue]{playtime.Room}[/]");
            }
            AnsiConsole.WriteLine(); // Adds an empty line for better readability
        }

        // Optionally wait for user input to return to the main menu
        AnsiConsole.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }
}