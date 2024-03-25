using Spectre.Console;

public static class Program
{
    public static void Main(string[] args)
    {
        // AnsiConsole.Markup("[underline red]Hello[/] World!");
        System.Console.OutputEncoding = System.Text.Encoding.UTF8;

        AnsiConsole.Write(
            new FigletText("TurboCinema")
                .LeftJustified()
                .Color(Color.Red));

        var naar = AnsiConsole.Prompt(new ConfirmationPrompt("Naar betaalscherm?"));
        if (naar)
        {
            AnsiConsole.Clear();
            var betaalscherm = new Betaalscherm();
        }

        Console.ReadLine();
    }
}
