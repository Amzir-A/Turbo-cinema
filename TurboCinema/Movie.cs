public class Movie
{
    public string Title { get; set; }
    public string Release { get; set; }
    public string Director { get; set; }
    public List<string> Actors { get; set; }
    public string Duration { get; set; }
    public List<string> Genre { get; set; }
    public string AgeRating { get; set; }
    public string Description { get; set; }

    public Movie(string title, string release, string director, List<string> actors, string duration, List<string> genre, string ageRating, string description)
    {
        Title = title;
        Release = release;
        Director = director;
        Actors = actors ?? new List<string>();
        Duration = duration;
        Genre = genre ?? new List<string>();
        AgeRating = ageRating;
        Description = description;
    }
}