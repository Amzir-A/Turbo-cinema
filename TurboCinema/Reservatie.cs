public class Reservation
{
    public string MovieTitle { get; set; }
    public DateTime PlayTime { get; set; }
    public List<Seat> SelectedSeats { get; set; }
    public string? Room { get; set; }

    public Reservation(string movieTitle, DateTime playTime, List<Seat> selectedSeats, string? room)
    {
        MovieTitle = movieTitle;
        PlayTime = playTime;
        SelectedSeats = selectedSeats;
        Room = room;
    }
}
