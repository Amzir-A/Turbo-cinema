using System;

class Program
{
    static void Main(string[] args)
    {
        var reservationSystem = new ReservationSystem();
        var displayManager = new DisplayManager(reservationSystem);

        ConsoleKey key;
        do
        {
            displayManager.DisplaySeats();
            key = Console.ReadKey(true).Key;
            displayManager.UpdateSelection(key);
        } while (key != ConsoleKey.Escape);
    }
}
