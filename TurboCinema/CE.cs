// Console extension, extra functionaliteit voor het vragen om input in de console

using Spectre.Console;

public class CE
{
    public static int ConfirmR(string message)
    {
        while (true)
        {
            Console.Write(message + " ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"ja, nee of terug (j/n/t): ");
            Console.ResetColor();

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.J)
            {
                return 0;
            }
            else if (key.Key == ConsoleKey.N)
            {
                return 1;
            } else if (key.Key == ConsoleKey.T)
            {
                return 2;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ongeldige invoer. Voer 'j', 'n' of 't' in.");
                Console.ResetColor();
            }
        }
    }

    public static bool Confirm(string message)
    {
        while (true)
        {
            Console.Write(message + " ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("ja of nee (j/n): ");
            Console.ResetColor();

            ConsoleKeyInfo key = Console.ReadKey();
            Console.WriteLine();
            if (key.Key == ConsoleKey.J)
            {
                return true;
            }
            else if (key.Key == ConsoleKey.N)
            {
                return false;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Ongeldige invoer. Voer 'j' of 'n' in.");
                Console.ResetColor();
            }
        }
    }

    public static bool Confirm2(string message)
    {
        Color color = Color.Grey;
        Color color2 = Color.Black;
        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine(message);

            WL();
            AnsiConsole.Write(new Text("Ja", new Style(Color.Yellow, color)).Centered());
            AnsiConsole.Write(new Text("Nee", new Style(Color.Yellow, color2)).Centered());

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    color = Color.Grey;
                    color2 = Color.Black;
                    break;
                case ConsoleKey.DownArrow:
                    color = Color.Black;
                    color2 = Color.Grey;
                    break;
                case ConsoleKey.Enter:
                    return color == Color.Grey;
            }
        }
    }

    public static void Wait(string message)
    {
        Console.Write(message);
        for (int i = 0; i < 3; i++)
        {
            System.Threading.Thread.Sleep(500);
            Console.Write(".");
        }
        Console.WriteLine();
    }

    public static void Clear()
    {
        Console.Clear();
    }
    public static void WL()
    {
        Console.WriteLine();
    }

    public static void WL(string message)
    {
        Console.WriteLine(message);
    }
    public static void PressAnyKey()
    {
        Console.WriteLine("Druk op een knop om door te gaan...");
        Console.ReadKey();
    }

}
