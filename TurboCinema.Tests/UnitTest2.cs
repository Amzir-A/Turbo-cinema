using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurboCinema;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace TurboCinema.Tests
{
    [TestClass]
    public class ReservationSystemTests
    {
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
            var seats = new List<List<Seat>>();

            // Act
            ReservationSystem.SaveSeats(movieTitle, playtime, seats);

            // Assert
            string json = File.ReadAllText("Data/MoviesAndPlaytimes.json");
            var movies = JsonConvert.DeserializeObject<List<Movie>>(json);
            var movie = movies.FirstOrDefault(m => m.Title == movieTitle);
            var selectedPlaytime = movie?.Playtimes.FirstOrDefault(p => p.DateTime == playtime);
            Assert.AreEqual(seats, selectedPlaytime?.Seats);
        }
    }
}
