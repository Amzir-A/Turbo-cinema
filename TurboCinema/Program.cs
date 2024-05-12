using Newtonsoft.Json;
using System.Text;

static class Program
{
    class ScreenState
    {
        public required Delegate ScreenDelegate { get; set; }
        public object[]? Arguments { get; set; }
    }

    static Stack<ScreenState> screenHistory = new();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ShowScreen(MainScreen.MainMenu);
    }

    public static void ShowScreen(Action screen)
    {
        screenHistory.Push(new ScreenState { ScreenDelegate = screen });
        screen.Invoke();
    }
    public static void ShowScreen<T>(Action<T> screen, T arg)
    {
        screenHistory.Push(new ScreenState { ScreenDelegate = screen, Arguments = [arg] });
        screen.Invoke(arg);
    }

    public static void PreviousScreen()
    {
        if (screenHistory.Count > 1)
        {
            screenHistory.Pop();
            ScreenState previousScreen = screenHistory.Pop();
            if (previousScreen.Arguments != null)
                previousScreen.ScreenDelegate.DynamicInvoke(previousScreen.Arguments);
            else
                previousScreen.ScreenDelegate.DynamicInvoke();
        }
    }
}
