using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurboCinema; // Zorg ervoor dat je de juiste namespace gebruikt

namespace TurboCinema.Tests
{
    [TestClass]
    public class MovieSelectorTests
    {
    [TestMethod]
    public void GetSelectedMovie_ShouldReturnCorrectMovie()
    {
        // Arrange
        var movies = new List<Movie>
        {
            new("Dune: Part Two", "29-02-2024", "Denis Villeneuve", new List<string> { "Timothee Chalamet", "Zendaya", "Rebecca Ferguson", "Dave Bautista", "Florence Pugh" }, "166 minutes", new List<string> { "Science fiction", "Action" }, "12", "In Dune: Part Two, Paul Atreides' legendary journey continues in the company of Chani and the Fremen as he seeks revenge on those who caused his family's downfall. Paul will have to choose between the love of his life and the fate of the universe to avoid the terrible future he alone has foreseen.")
        };
        MovieSelector.movies = movies;
        MovieSelector.selectedIndex = 0;

        var selectedMovie = MovieSelector.GetSelectedMovie();

        Assert.AreEqual("Dune: Part Two", selectedMovie.Title);
    }

    [TestMethod]
    public void ResetMovies_ShouldResetMovieList()
    {
        // Arrange
        var originalMovies = new List<Movie>
        {
            new Movie(
                title: "Movie 1",
                release: "01-01-2024",
                director: "Director 1",
                actors: new List<string> { "Actor 1" },
                duration: "120 minutes",
                genre: new List<string> { "Genre 1" },
                ageRating: "PG-13",
                description: "Description 1"
            )
        };
        MovieSelector.movies = originalMovies;
        MovieSelector.copyOfMovies = originalMovies.ToList();

        MovieSelector.ResetMovies();

        CollectionAssert.AreEqual(originalMovies, MovieSelector.movies);
    }
}
}
