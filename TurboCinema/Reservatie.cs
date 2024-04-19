public class Reservation
{
    public string MovieTitle { get; set; }
    public DateTime PlayTime { get; set; }
    public List<Seat> SelectedSeats { get; set; } // Zorg ervoor dat List<Seat> toegankelijk is

    public Reservation(string movieTitle, DateTime playTime, List<Seat> selectedSeats)
    {
        MovieTitle = movieTitle;
        PlayTime = playTime;
        SelectedSeats = selectedSeats;
    }
}
