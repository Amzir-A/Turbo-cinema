using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TurboCinema.Tests
{
    [TestClass]
    public class MovieSelectorTest
    {
        private string testFile = "Data/TestAccountInfo.json";
        private string testMoviesAndPlaytimes = "Data/MoviesAndPlaytimes.json";

        [TestInitialize]
        public void Setup()
        {
            // Ensure test files are clean
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            if (File.Exists(testMoviesAndPlaytimes))
            {
                File.Delete(testMoviesAndPlaytimes);
            }

            // Create test movie data
            var testMovies = new List<Movie>
            {
                new Movie("Action Movie", "01-01-2024", "Director 1", new List<string> { "Actor 1" }, "120 minutes", new List<string> { "Action" }, "PG-13", "Description 1"),
                new Movie("Drama Movie", "02-02-2024", "Director 2", new List<string> { "Actor 2" }, "130 minutes", new List<string> { "Drama" }, "R", "Description 2"),
                new Movie("Comedy Movie", "03-03-2024", "Director 3", new List<string> { "Actor 3" }, "140 minutes", new List<string> { "Comedy" }, "G", "Description 3")
            };

            string json = JsonConvert.SerializeObject(testMovies, Formatting.Indented);
            File.WriteAllText(testMoviesAndPlaytimes, json);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (File.Exists(testFile))
            {
                File.Delete(testFile);
            }

            if (File.Exists(testMoviesAndPlaytimes))
            {
                File.Delete(testMoviesAndPlaytimes);
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

        [TestMethod]
        public void DisplaySortedMovies_ShouldSortMoviesByGenre()
        {
            // Arrange
            MovieSelector.movies = MovieSelector.LoadMovies();
            MovieSelector.copyOfMovies = MovieSelector.movies.ToList();

            // Act
            MovieSelector.DisplaySortedMovies("genre");

            // Assert
            var sortedMovies = MovieSelector.movies;
            Assert.AreEqual("Action Movie", sortedMovies[0].Title);
            Assert.AreEqual("Comedy Movie", sortedMovies[1].Title);
            Assert.AreEqual("Drama Movie", sortedMovies[2].Title);
        }

        [TestMethod]
        public void DisplaySortedMovies_ShouldSortMoviesByActor()
        {
            // Arrange
            MovieSelector.movies = MovieSelector.LoadMovies();
            MovieSelector.copyOfMovies = MovieSelector.movies.ToList();

            // Act
            MovieSelector.DisplaySortedMovies("actor");

            // Assert
            var sortedMovies = MovieSelector.movies;
            Assert.AreEqual("Action Movie", sortedMovies[0].Title);
            Assert.AreEqual("Drama Movie", sortedMovies[1].Title);
            Assert.AreEqual("Comedy Movie", sortedMovies[2].Title);
        }

        [TestMethod]
        public void DisplaySortedMovies_ShouldSortMoviesByReleaseDate()
        {
            // Arrange
            MovieSelector.movies = MovieSelector.LoadMovies();
            MovieSelector.copyOfMovies = MovieSelector.movies.ToList();

            // Act
            MovieSelector.DisplaySortedMovies("publicatiedatum");

            // Assert
            var sortedMovies = MovieSelector.movies;
            Assert.AreEqual("Action Movie", sortedMovies[0].Title);
            Assert.AreEqual("Drama Movie", sortedMovies[1].Title);
            Assert.AreEqual("Comedy Movie", sortedMovies[2].Title);
        }

        [TestMethod]
        public void DisplaySortedMovies_ShouldSortMoviesByDuration()
        {
            // Arrange
            MovieSelector.movies = MovieSelector.LoadMovies();
            MovieSelector.copyOfMovies = MovieSelector.movies.ToList();

            // Act
            MovieSelector.DisplaySortedMovies("lengte");

            // Assert
            var sortedMovies = MovieSelector.movies;
            Assert.AreEqual("Action Movie", sortedMovies[0].Title);
            Assert.AreEqual("Drama Movie", sortedMovies[1].Title);
            Assert.AreEqual("Comedy Movie", sortedMovies[2].Title);
        }
    }
}
