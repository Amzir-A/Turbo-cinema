using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

public class Admin : IReservationService
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
                    var seatsArray = GenerateSeats(numRows, numSeatsPerRow);
                    playtime.Seats = seatsArray.Select(row => row.ToList()).ToList();
                }
            }
        }
        SaveMovies();
    }

    private Seat[][] GenerateSeats(int numRows, int numSeatsPerRow)
    {
        var rows = new Seat[numRows][];
        for (int i = 0; i < numRows; i++)
        {
            rows[i] = new Seat[numSeatsPerRow];
            for (int j = 0; j < numSeatsPerRow; j++)
            {
                rows[i][j] = new Seat($"Rij {i + 1} - Stoel {j + 1}", true);
            }
        }
        return rows;
    }

    public void GeneratePlaytimes(List<Movie> movies, DateTime startDate, DateTime endDate)
    {
        Random rand = new Random();

        foreach (var movie in movies)
        {
            int numberOfPlaytimes = rand.Next(3, 8);

            for (int i = 0; i < numberOfPlaytimes; i++)
            {
                DateTime randomDate = startDate.AddDays(rand.Next((endDate - startDate).Days));
                DateTime randomTime = randomDate.AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));

                Seat[][] seatsArray = GenerateSeats(5, 3);

                Playtime newPlaytime = new Playtime
                {
                    DateTime = randomTime,
                    Room = "Hall " + rand.Next(1, 11),
                    Seats = seatsArray.Select(row => row.ToList()).ToList()
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
