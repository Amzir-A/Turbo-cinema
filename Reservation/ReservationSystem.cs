using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class ReservationSystem
{
    public List<Seat> Seats { get; private set; }
    private int newlySelectedSeats = 0; // Tracks newly selected seats for pricing

    public ReservationSystem()
    {
        Seats = new List<Seat>();
        LoadReservations();
    }

    public bool ToggleSeatReservation(int index)
    {
        if (index >= 0 && index < Seats.Count && !Seats[index].InitiallyReserved)
        {
            Seats[index].IsReserved = !Seats[index].IsReserved;
            // Adjust the count based on the new reservation status
            newlySelectedSeats += Seats[index].IsReserved ? 1 : -1;
            return true;
        }
        return false;
    }

    public int GetNewlySelectedSeatsCount()
    {
        return newlySelectedSeats;
    }

    public void SaveReservations()
    {
        var json = JsonConvert.SerializeObject(Seats, Formatting.Indented);
        File.WriteAllText("reservations.json", json);
    }
    private void LoadReservations()
    {
        if (File.Exists("reservations.json"))
        {
            var json = File.ReadAllText("reservations.json");
            Seats = JsonConvert.DeserializeObject<List<Seat>>(json) ?? new List<Seat>();

            // Mark each seat's initially reserved status
            foreach (var seat in Seats)
            {
                seat.SetInitiallyReserved();
            }
        }
        else
        {
            // Initialize with default seats or handle accordingly
        }
    }

}
