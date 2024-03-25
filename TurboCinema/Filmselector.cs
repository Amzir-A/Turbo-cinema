using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class FilmSelector
{
    private List<Film> films;
    private int huidigeIndex = 0;

    // Constructor om een FilmSelector aan te maken met een lijst van films.
    public FilmSelector(List<Film> films)
    {
        // Zorg ervoor dat 'films' niet null is; zo wel, gooi een ArgumentNullException.
        this.films = films ?? throw new ArgumentNullException(nameof(films));
        // Check of de filmlijst niet leeg is, zo wel, gooi een ArgumentException.
        if (films.Count == 0)
        {
            throw new ArgumentException("Filmlijst mag niet leeg zijn.", nameof(films));
        }
    }

    
    // Toont films in de console en laat gebruikers navigeren en een film selecteren.
    public void ToonFilms()
    {
        ConsoleKeyInfo keyInfo;

        do
        {
            // Maak de console schoon voor elke navigatieactie.
            Console.Clear();
            Console.WriteLine("Gebruik de pijltjestoetsen om door de films te navigeren. Druk op Enter om te selecteren.\n");
            // Loop door elke film in de lijst en toon deze.
            for (int i = 0; i < films.Count; i++)
            {
                // Markeer de huidige geselecteerde film met een andere achtergrondkleur.
                if (i == huidigeIndex)
                {
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                // Toon de film. De `ToString` methode van Film wordt hier gebruikt.
                Console.WriteLine($"{i+1}. {films[i]}");

                if (i == huidigeIndex)
                {
                    Console.ResetColor();
                }
            }

            keyInfo = Console.ReadKey();
            // Update de huidigeIndex op basis van pijltjestoetsen.
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                huidigeIndex = (huidigeIndex - 1 + films.Count) % films.Count;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                huidigeIndex = (huidigeIndex + 1) % films.Count;
            }

        } while (keyInfo.Key != ConsoleKey.Enter); // Herhaal tot de gebruiker 'Enter' drukt om een film te selecteren.

        Console.Clear();
        Film geselecteerdeFilm = films[huidigeIndex];
        Console.WriteLine($"Titel: {geselecteerdeFilm.Title}");
        Console.WriteLine($"Release: {geselecteerdeFilm.Release}");
        Console.WriteLine($"Regisseur: {geselecteerdeFilm.Director}");
        Console.WriteLine($"Acteurs: {String.Join(", ", geselecteerdeFilm.Actors)}");
        Console.WriteLine($"Duur: {geselecteerdeFilm.Duration}");
        Console.WriteLine($"Genre: {String.Join(", ", geselecteerdeFilm.Genre)}");
        Console.WriteLine($"Leeftijdsclassificatie: {geselecteerdeFilm.AgeRating}");
        Console.WriteLine($"Beschrijving: {geselecteerdeFilm.Description}");
    }
}

public class FilmLoader
{
    public static List<Film> LaadFilmsVanuitJson(string bestandspad)
    {
        using (StreamReader r = new StreamReader(bestandspad))
        {
            string json = r.ReadToEnd();
            // Parse de JSON string naar een JObject.
            var jObject = JObject.Parse(json);
            // Selecteer het deel van de JSON dat de films bevat.
            var moviesToken = jObject.SelectToken("movies");
            if (moviesToken != null)
            {
                // Converteer de JSON array naar een lijst van Film objecten.
                List<Film> films = moviesToken.ToObject<List<Film>>();
                return films;
            }
        }
        return new List<Film>();
    }
}