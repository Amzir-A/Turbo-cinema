public class Reservation
{
    public string MovieTitle { get; set; }
    public DateTime Playtime { get; set; }
    public List<Seat> Seats { get; set; }
    public string Room { get; set; }
    public List<(string, int, decimal)> FoodAndDrinks { get; set; }

    public Reservation(string movieTitle, DateTime playtime, List<Seat> seats, string room, List<(string, int, decimal)> foodAndDrinks)
    {
        MovieTitle = movieTitle;
        Playtime = playtime;
        Seats = seats;
        Room = room;
        FoodAndDrinks = foodAndDrinks;
    }
}
