using System;

public class DisplayManager
{
    private ReservationSystem reservationSystem;
    private int selectedSeatIndex = 0;
    private bool isContinueSelected = false;

    public DisplayManager(ReservationSystem reservationSystem)
    {
        this.reservationSystem = reservationSystem;
    }

    public void DisplaySeats()
    {
        Console.Clear();
        for (int i = 0; i < reservationSystem.Seats.Count; i++)
        {
            var seat = reservationSystem.Seats[i];
            Console.BackgroundColor = i == selectedSeatIndex && !isContinueSelected ? ConsoleColor.Yellow : ConsoleColor.Black;
            Console.Write($"{seat.Id} {(seat.IsReserved ? "(X)" : "")} ");
        }
        Console.BackgroundColor = ConsoleColor.Black;

        // Display Continue option
        Console.BackgroundColor = isContinueSelected ? ConsoleColor.Yellow : ConsoleColor.Black;
        Console.WriteLine("\nContinue to payment");
        Console.BackgroundColor = ConsoleColor.Black;

        Console.WriteLine("Use arrow keys to navigate, Enter to select, Escape to exit.");
    }

    public void UpdateSelection(ConsoleKey key)
    {
        switch (key)
        {
            case ConsoleKey.LeftArrow:
                if (isContinueSelected)
                {
                    isContinueSelected = false;
                }
                else
                {
                    selectedSeatIndex = Math.Max(0, selectedSeatIndex - 1);
                }
                break;
            case ConsoleKey.RightArrow:
                if (!isContinueSelected)
                {
                    if (selectedSeatIndex < reservationSystem.Seats.Count - 1)
                    {
                        selectedSeatIndex++;
                    }
                    else
                    {
                        isContinueSelected = true;
                    }
                }
                break;
            case ConsoleKey.Enter:
                if (isContinueSelected)
                {
                    // Proceed to payment logic
                    ProceedToPayment();
                }
                else
                {
                    reservationSystem.ReserveSeat(selectedSeatIndex);
                }
                break;
        }
    }

    private void ProceedToPayment()
    {
        Console.Clear();
        Console.WriteLine("Proceeding to payment...");
        // Add payment logic here
        Console.ReadKey();
        Environment.Exit(0); // Or navigate to the next section
    }
}
