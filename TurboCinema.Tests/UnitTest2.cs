using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TurboCinema;

namespace TurboCinema.Tests
{
    [TestClass]
    public class ReservationSystemTests
    {
        private const string TestFilePath = "Data/MoviesAndPlaytimes.json";
        private List<Movie> _originalMovies;

        [TestInitialize]
        public void TestInitialize()
        {
            // Backup the original file content before each test
            if (File.Exists(TestFilePath))
            {
                _originalMovies = JsonConvert.DeserializeObject<List<Movie>>(File.ReadAllText(TestFilePath));
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Restore the original file content after each test
            if (_originalMovies != null)
            {
                string json = JsonConvert.SerializeObject(_originalMovies, Formatting.Indented);
                File.WriteAllText(TestFilePath, json);
            }
        }

        [TestMethod]
        public void TestLoadSeats()
        {
            string movieTitle = "Test Movie";
            DateTime playtime = new DateTime(2022, 1, 1);

            var seats = ReservationSystem.LoadSeats(movieTitle, playtime);

            // Assert
            Assert.IsNotNull(seats);
        }

        [TestMethod]
        public void TestSaveSeats()
        {
            // Arrange
            string movieTitle = "Test Movie";
            DateTime playtime = new DateTime(2022, 1, 1);
            var seats = new List<List<Seat>>
            {
                new List<Seat>
                {
                    new Seat("1", true),
                    new Seat("2", true)
                }
            };


            var testMovie = new Movie(
                "Test Movie",
                "01-01-2022",
                "Test Director",
                new List<string> { "Actor 1", "Actor 2" },
                "120 minutes",
                new List<string> { "Genre 1" },
                "PG-13",
                "Test description"
            );

            var testPlaytime = new Playtime
            {
                DateTime = playtime,
                Seats = new List<List<Seat>>()
            };

            testMovie.Playtimes = new List<Playtime> { testPlaytime };

            File.WriteAllText(TestFilePath, JsonConvert.SerializeObject(new List<Movie> { testMovie }, Formatting.Indented));

            // Act
            ReservationSystem.SaveSeats(movieTitle, playtime, seats);

            // Assert
            string json = File.ReadAllText(TestFilePath);
            var movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            var movie = movies.FirstOrDefault(m => m.Title == movieTitle);
            Assert.IsNotNull(movie, "Movie not found");

            var selectedPlaytime = movie.Playtimes?.FirstOrDefault(p => p.DateTime == playtime);
            Assert.IsNotNull(selectedPlaytime, "Playtime not found");

            // Custom comparison of the nested lists
            Assert.AreEqual(seats.Count, selectedPlaytime.Seats.Count, "Number of seat rows do not match");
            for (int i = 0; i < seats.Count; i++)
            {
                Assert.AreEqual(seats[i].Count, selectedPlaytime.Seats[i].Count, $"Number of seats in row {i} do not match");
                for (int j = 0; j < seats[i].Count; j++)
                {
                    Assert.AreEqual(seats[i][j].ID, selectedPlaytime.Seats[i][j].ID, $"Seat ID at row {i}, column {j} does not match");
                    Assert.AreEqual(seats[i][j].IsAvailable, selectedPlaytime.Seats[i][j].IsAvailable, $"Seat availability at row {i}, column {j} does not match");
                }
            }
        }
    }
}
