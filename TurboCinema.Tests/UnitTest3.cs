using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace TurboCinema.Tests
{
    [TestClass]
    public class LoginScreenTests
    {
        [TestMethod]
        public void Register_ShouldAddNewCustomer()
        {

            string fileName = "Data/TestAccountInfo.json";
            List<Customer> customers = new List<Customer>();
            File.WriteAllText(fileName, JsonSerializer.Serialize(customers));

            var consoleInput = new Queue<string>();
            consoleInput.Enqueue("John");
            consoleInput.Enqueue("Doe");
            consoleInput.Enqueue("01-01-1990");
            consoleInput.Enqueue("john.doe@gmail.com");
            consoleInput.Enqueue("1234AB");
            consoleInput.Enqueue("Password1");


            LoginScreen.Register();


            customers = JsonSerializer.Deserialize<List<Customer>>(File.ReadAllText(fileName));
            Assert.AreEqual(1, customers.Count);
            Assert.AreEqual("John", customers[0].FirstName);
            Assert.AreEqual("Doe", customers[0].LastName);
            Assert.AreEqual(new DateTime(1990, 1, 1), customers[0].DateOfBirth);
            Assert.AreEqual("john.doe@gmail.com", customers[0].Email);
            Assert.AreEqual("1234AB", customers[0].Postcode);

            File.Delete(fileName);
        }
    }
}
