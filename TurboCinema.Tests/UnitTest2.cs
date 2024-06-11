// Als gebruiker wil ik dat mijn privacy wordt gerespecteerd en mijn persoonlijke gegevens veilig worden opgeslagen.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TurboCinema;

namespace TurboCinema.Tests
{
    [TestClass]
    public class MovieSelectorTest
    {
        private string testFile = "Data/TestAccountInfo.json";

        [TestInitialize]
        public void Setup()
        {
            // Setup: Zorg dat het testbestand leeg is
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }
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
        public void CustomerData_ShouldBeStoredSecurely()
        {
            // Arrange
            var originalPassword = "Password123";
            var customers = new List<Customer>
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Email = "john.doe@example.com",
                    Postcode = "1234AB",
                    Password = LoginScreen.HashPassword(originalPassword),
                    Reservations = new List<Reservation>()
                }
            };

            // Act
            LoginScreen.SaveCustomers(customers, testFile);
            var loadedCustomers = LoginScreen.LoadCustomers(testFile);
            var savedCustomer = loadedCustomers[0];

            // Assert
            Assert.AreEqual("John", savedCustomer.FirstName);
            Assert.AreEqual("Doe", savedCustomer.LastName);
            Assert.AreEqual(new DateTime(1990, 1, 1), savedCustomer.DateOfBirth);
            Assert.AreEqual("john.doe@example.com", savedCustomer.Email);
            Assert.AreEqual("1234AB", savedCustomer.Postcode);
            Assert.AreNotEqual(originalPassword, savedCustomer.Password); // Password should be hashed
            Assert.AreEqual(LoginScreen.HashPassword(originalPassword), savedCustomer.Password); // Hash should match
        }

        [TestMethod]
        public void SelectMovie_ShouldReturnCorrectMovie()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie("Movie 1", "01-01-2024", "Director 1", new List<string> { "Actor 1" }, "120 minutes", new List<string> { "Genre 1" }, "PG-13", "Description 1"),
                new Movie("Movie 2", "02-02-2024", "Director 2", new List<string> { "Actor 2" }, "130 minutes", new List<string> { "Genre 2" }, "R", "Description 2"),
                new Movie("Movie 3", "03-03-2024", "Director 3", new List<string> { "Actor 3" }, "140 minutes", new List<string> { "Genre 3" }, "G", "Description 3")
            };

            MovieSelector.movies = movies;
            MovieSelector.copyOfMovies = movies.ToList();

            // Act
            MovieSelector.SelectMovie("Movie 2");
            var selectedMovie = MovieSelector.GetSelectedMovie();

            // Assert
            Assert.AreEqual("Movie 2", selectedMovie.Title);
            Assert.AreEqual("02-02-2024", selectedMovie.Release);
            Assert.AreEqual("Director 2", selectedMovie.Director);
        }
    }
}
