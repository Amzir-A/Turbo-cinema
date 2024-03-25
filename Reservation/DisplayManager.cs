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
        Console.WriteLine("\nBetalen");
        Console.BackgroundColor = ConsoleColor.Black;

        Console.WriteLine("Gebruik de pijltoesten om een stoel te kiezen met enter. Klik vervolgens op betalen om door te gaan.");
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
                    ProceedToPayment();
                }
                else
                {
                    bool wasToggled = reservationSystem.ToggleSeatReservation(selectedSeatIndex);
                    if (!wasToggled)
                    {
                        // Optionally, notify the user that the seat is already reserved
                        Console.WriteLine("\nDeze stoel is al gereserveerd. Druk op een toets om door te gaan.");
                        Console.ReadKey(true);
                    }
                }
                break;
        }
    }


    private void ProceedToPayment()
    {
        if (reservationSystem.GetNewlySelectedSeatsCount() > 0)
        {
            Console.Clear();
            Console.WriteLine("Doorgaan naar betalen...");
            // Proceed with payment logic here, perhaps including calculating the total price
            reservationSystem.SaveReservations(); // Save the current reservation state
            Console.ReadKey();
            Environment.Exit(0); // Or navigate to the next section of your program
        }
        else
        {
            // Inform the user that at least one seat must be selected before continuing
            Console.WriteLine("\nSelecteer a.u.b minstens één stoel. Druk op een toets om door te gaan.");
            Console.ReadKey(true);
        }
    }
}
