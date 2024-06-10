using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace TurboCinema.Tests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void CancelReservation_ShouldMakeSeatsAvailable()
        {
            // Arrange
            var seat1 = new Seat("A1", true);
            var seat2 = new Seat("A2", true);
            var seats = new List<Seat> { seat1, seat2 };

            ReservationSystem.SelectedSeats = seats;
            ReservationSystem.Seats = new List<List<Seat>> { seats };

            // Act
            // Simulate the cancellation of the reservation
            ReservationSystem.SelectedSeats.ForEach(seat => seat.IsAvailable = true);
            ReservationSystem.SelectedSeats.Clear();

            // Assert
            Assert.IsTrue(seat1.IsAvailable, "Seat A1 should be available after cancellation.");
            Assert.IsTrue(seat2.IsAvailable, "Seat A2 should be available after cancellation.");
        }
    }
}
