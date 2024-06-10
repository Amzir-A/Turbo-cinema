using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;


namespace TurboCinema.Tests
{
    [TestClass]
    public class ReservationCancellationTests
    {
        private string testFile = "Data/TestMoviesAndPlaytimes.json";

        [TestInitialize]
        public void Setup()
        {
            // Create test data
            var testMovies = new List<Movie>
            {
                new Movie(
                    title: "Test Movie",
                    release: "29-02-2024",
                    director: "John Doe",
                    actors: new List<string> { "Actor 1", "Actor 2" },
                    duration: "120 minutes",
                    genre: new List<string> { "Genre 1" },
                    ageRating: "PG-13",
                    description: "Test Description")
                {
                    Playtimes = new List<Playtime>
                    {
                        new Playtime
                        {
                            DateTime = new DateTime(2024, 6, 1, 20, 0, 0),
                            Seats = new List<List<Seat>>
                            {
                                new List<Seat>
                                {
                                    new Seat("A1", true),
                                    new Seat("A2", true)
                                },
                                new List<Seat>
                                {
                                    new Seat("B1", true),
                                    new Seat("B2", true)
                                }
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(testMovies, Formatting.Indented);
            File.WriteAllText(testFile, json);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
        }

        [TestMethod]
        public void CancelReservation_ShouldMakeSeatsAvailable()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = "john.doe@example.com",
                    Postcode = "1234AB",
                    Password = "Password123",
                    Reservations = new List<Reservation>
                    {
                        new Reservation(
                            movieTitle: "Test Movie",
                            playtime: new DateTime(2024, 6, 1, 20, 0, 0),
                            seats: new List<Seat>
                            {
                                new Seat("A1", false),
                                new Seat("A2", false)
                            },
                            room: "Room 1",
                            foodAndDrinks: new List<(string, int, decimal)>())
                    }
                }
            };

            // Act
            var customer = customers[0];
            var reservationToCancel = customer.Reservations[0];
            LoginScreen.CancelReservation(customer, customers, 0);

            // Assert
            var updatedSeats = ReservationSystem.LoadSeats("Test Movie", new DateTime(2024, 6, 1, 20, 0, 0), testFile);
            Assert.IsTrue(updatedSeats[0][0].IsAvailable); // A1 should be available
            Assert.IsTrue(updatedSeats[0][1].IsAvailable); // A2 should be available
        }
    }
}
