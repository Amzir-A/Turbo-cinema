using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Admin
{
    public List<Movie> _movies;
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

    public void SaveMovies()
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

    public void GeneratePlaytimes(List<Movie> movies, DateTime begindatum, DateTime einddatum)
    {
        Random rand = new Random();
        DateTime startDate = DateTime.Now;
        DateTime endDate = startDate.AddDays(14);

        foreach (var movie in movies)
        {
            movie.Playtimes.Clear();
            int numberOfPlaytimes = rand.Next(3, 8);

            for (int i = 0; i < numberOfPlaytimes; i++)
            {
                DateTime randomDate = startDate.AddDays(rand.Next((endDate - startDate).Days));
                DateTime randomTime = randomDate.AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));

                Playtime newPlaytime = new Playtime
                {
                    DateTime = randomTime,
                    Room = "Hall " + rand.Next(1, 11),
                    Seats = GenerateSeats(5, 3)
                };

                movie.Playtimes.Add(newPlaytime);
            }
        }
    }

    public void AddMovie(Movie newMovie)
    {
        _movies.Add(newMovie);
        SaveMovies();
    }
     public void RemovePastPlaytimes()
    {
        DateTime now = DateTime.Now;
        foreach (var movie in _movies)
        {
            movie.Playtimes = movie.Playtimes.Where(pt => pt.DateTime >= now).ToList();
        }
        SaveMovies();
    }
}
