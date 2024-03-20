using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class ReservationSystem
{
    public List<Seat> Seats { get; private set; }

    public ReservationSystem()
    {
        Seats = new List<Seat>();
        LoadReservations();
    }

    public void ReserveSeat(int index)
    {
        if (!Seats[index].IsReserved)
        {
            Seats[index].IsReserved = true;
            SaveReservations();
        }
    }

    private void SaveReservations()
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
        }
        else
        {
            // Initialize with default seats or handle accordingly
        }
    }
}
