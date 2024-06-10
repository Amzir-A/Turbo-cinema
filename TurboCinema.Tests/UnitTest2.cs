// Als gebruiker wil ik dat mijn privacy wordt gerespecteerd en mijn persoonlijke gegevens veilig worden opgeslagen.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

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
    }
}