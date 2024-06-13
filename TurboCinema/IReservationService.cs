public interface IReservationService
{
    void SetHallSize(string hallName, int numRows, int numSeatsPerRow);
    void GeneratePlaytimes(List<Movie> movies, DateTime startDate, DateTime endDate);
    void AddMovie(Movie newMovie);
    void RemovePastPlaytimes();
}
