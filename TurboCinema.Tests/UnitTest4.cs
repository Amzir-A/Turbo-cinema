using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Spectre.Console;
using TurboCinema;

namespace TurboCinema.Tests
{
    [TestClass]
    public class BetaalschermTests
    {
        [TestMethod]
        public void CalculateTotalPrice_ShouldReturnCorrectTotal()
        {
            // Arrange
            var seats = new List<Seat> { new Seat("1", true), new Seat("2", true) };
            var movie = new Movie("Test Movie", "01-01-2024", "Test Director", new List<string> { "Actor 1", "Actor 2" }, "120 minutes", new List<string> { "Action" }, "PG-13", "Test Description");
            var playtime = new Playtime { DateTime = DateTime.Now, Room = "Room 1" };
            var foodAndDrinks = new List<(string, int, decimal)>
            {
                ("Popcorn", 2, 3.5m),
                ("Soda", 1, 2.0m)
            };
            var betaalScherm = new Betaalscherm(seats, movie, playtime, foodAndDrinks);

            // Act
             int totalPrice = betaalScherm.CalculateTotalPrice();

            // Assert
            Assert.AreEqual(23, totalPrice);
        }

        [TestMethod]
        public void LoadCustomers_ShouldReturnCorrectCustomerList()
        {
            // Arrange
            string fileName = "Data/TestAccountInfo.json";
            List<Customer> customers = new List<Customer>
            {
                new Customer { Email = "test@example.com", Reservations = new List<Reservation>() }
            };
            File.WriteAllText(fileName, JsonSerializer.Serialize(customers));

            var betaalScherm = new Betaalscherm(null, null, null, null);

            // Act
            List<Customer> loadedCustomers = betaalScherm.LoadCustomers(fileName);

            // Assert
            Assert.AreEqual(1, loadedCustomers.Count);
            Assert.AreEqual("test@example.com", loadedCustomers[0].Email);

            File.Delete(fileName); // Clean up
        }

        [TestMethod]
        public void FindCustomerByEmail_ShouldReturnCorrectCustomer()
        {
            // Arrange
            string fileName = "Data/TestAccountInfo.json";
            List<Customer> customers = new List<Customer>
            {
                new Customer { Email = "test@example.com", Reservations = new List<Reservation>() }
            };
            File.WriteAllText(fileName, JsonSerializer.Serialize(customers));

            var betaalScherm = new Betaalscherm(null, null, null, null);

            // Act
            Customer customer = betaalScherm.FindCustomerByEmail("test@example.com");

            // Assert
            Assert.IsNotNull(customer);
            Assert.AreEqual("test@example.com", customer.Email);

            File.Delete(fileName); // Clean up
        }
    }
}