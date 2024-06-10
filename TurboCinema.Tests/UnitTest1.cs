using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurboCinema;
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
            var seat1 = new Seat { SeatNumber = "A1", IsReserved = true };
            var seat2 = new Seat { SeatNumber = "A2", IsReserved = true };
            var seats = new List<Seat> { seat1, seat2 };

            var reservation = new Reservation
            {
                ReservationId = 1,
                Seats = seats,
                IsCancelled = false
            };

            var reservationSystem = new ReservationSystem();
            reservationSystem.Reservations.Add(reservation);

            // Act
            reservationSystem.CancelReservation(reservation.ReservationId);

            // Assert
            Assert.IsTrue(seat1.IsReserved == false, "Seat A1 should be available after cancellation.");
            Assert.IsTrue(seat2.IsReserved == false, "Seat A2 should be available after cancellation.");
        }
    }

    public class Seat
    {
        public string SeatNumber { get; set; }
        public bool IsReserved { get; set; }
    }

    public class Reservation
    {
        public int ReservationId { get; set; }
        public List<Seat> Seats { get; set; }
        public bool IsCancelled { get; set; }
    }

    public class ReservationSystem
    {
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();

        public void CancelReservation(int reservationId)
        {
            var reservation = Reservations.Find(r => r.ReservationId == reservationId);
            if (reservation != null)
            {
                reservation.IsCancelled = true;
                foreach (var seat in reservation.Seats)
                {
                    seat.IsReserved = false;
                }
            }
        }
    }
}
