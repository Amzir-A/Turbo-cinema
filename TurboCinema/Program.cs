using Spectre.Console;

public static class Program
{
    public static void Main(string[] args)
    {
        // AnsiConsole.Markup("[underline red]Hello[/] World!");
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;

        var naar = AnsiConsole.Prompt(new ConfirmationPrompt("Naar betaalscherm?"));
        if (naar)
        {
            AnsiConsole.Clear();
            var betaalscherm = new Betaalscherm();
        }

        Console.ReadLine();
    }
}
