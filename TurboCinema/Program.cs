using Newtonsoft.Json;
using Spectre.Console;
using System.Text;

static class Program
{
    public class ScreenState
    {
        public required Delegate ScreenDelegate { get; set; }
        public object[]? Arguments { get; set; }
    }
  
    public static List<ScreenState> screenHistory = new();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        ShowScreen(MainScreen.MainMenu);
    }

    public static void ShowScreen(object screen)
    {
        if (screen is Delegate scrDelegate)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrDelegate });
            scrDelegate.DynamicInvoke();
        } else if (screen is Action scrAction)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrAction });
            scrAction.Invoke();
        }
    }
    public static void ShowScreen(object screen, object[] arg)
    {
        if (screen is Delegate scrDelegate)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrDelegate, Arguments = arg });
            scrDelegate.DynamicInvoke(arg);
        } else if (screen is Action scrAction)
        {
            screenHistory.Add(new ScreenState { ScreenDelegate = scrAction, Arguments = arg });
            scrAction.DynamicInvoke(arg);
        }
    }

    public static void PreviousScreen()
    {
        if (screenHistory.Count > 1)
        {
            screenHistory.RemoveAt(screenHistory.Count - 1);
            ScreenState previousScreen = screenHistory[^1];
            screenHistory.RemoveAt(screenHistory.Count - 1);
            

            if (previousScreen.Arguments != null)
                ShowScreen(previousScreen.ScreenDelegate, previousScreen.Arguments);
            else
                ShowScreen(previousScreen.ScreenDelegate);
        }
    }
}
