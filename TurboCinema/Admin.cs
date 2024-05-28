using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Admin
{
    private List<Movie> _movies;
    private string _moviesFilePath;

    public Admin(string moviesFilePath)
    {
        _moviesFilePath = moviesFilePath;
        _movies = LoadMovies(_moviesFilePath);
    }

    private List<Movie> LoadMovies(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<Movie>>(json);
    }

    private void SaveMovies()
    {
        var json = JsonConvert.SerializeObject(_movies, Formatting.Indented);
        File.WriteAllText(_moviesFilePath, json);
    }

    public void SetHallSize(string hallName, int numRows, int numSeatsPerRow)
    {
        foreach (var movie in _movies)
        {
            foreach (var playtime in movie.Playtimes)
            {
                if (playtime.Room == hallName)
                {
                    playtime.Seats = GenerateSeats(numRows, numSeatsPerRow);
                }
            }
        }
        SaveMovies();
    }

    private List<List<Seat>> GenerateSeats(int numRows, int numSeatsPerRow)
    {
        var rows = new List<List<Seat>>();
        for (int i = 1; i <= numRows; i++)
        {
            var row = new List<Seat>();
            for (int j = 1; j <= numSeatsPerRow; j++)
            {
                row.Add(new Seat($"Rij {i} - Stoel {j}", true));
            }
            rows.Add(row);
        }
        return rows;
    }
}
