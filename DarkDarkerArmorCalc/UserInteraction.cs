using Spectre.Console;

namespace DarkDarkerArmorCalc;

public class UserInteraction
{
    public static bool ContinueChecker()
    {
        AnsiConsole.Markup("[bold]Would you like to continue? (Y/N): [/]");
        string? userInput;

        while (true)
        {
            userInput = Console.ReadLine()?.Trim()?.ToLower();

            if (userInput == "y")
            {
                Console.Clear();
                var rule = new Rule("[wheat1]Previous results[/]");
                rule.Style = Style.Parse("red dim");
                AnsiConsole.Write(rule);
                rule.Title = "";
                AnsiConsole.Write(rule);
                return true;
            }
            else if (userInput == "n")
            {
                AnsiConsole.MarkupLine("[cyan]Goodbye![/]");
                return false;
            }
            AnsiConsole.Markup("[red]Invalid input.[/] Please answer with either '[bold]Y[/]' or '[bold]N[/]': ");
        }
    }
    public static CharClass GetValidCharClass()
    {
        var classes = Enum.GetValues(typeof(CharClass)).Cast<CharClass>().ToList();
        var panel = new Panel(string.Join("\n", classes));
        panel.Header = new PanelHeader("DnD Classes");
        panel.Border = BoxBorder.Ascii;
        AnsiConsole.Write(panel);

        AnsiConsole.Markup("Enter your character class: ");
        while (true)
        {
            if (Enum.TryParse(Console.ReadLine()?.Trim(), true, out CharClass userCharClass))
            {
                return userCharClass;
            }
            else
            {
                AnsiConsole.Markup("[red]Invalid input.[/] Please answer with a valid [bold]character class[/]: ");
            }
        }
    }

    public static double GetValidMinimumMoveSpeed()
    {
        AnsiConsole.Markup("Enter minimum move speed: ");
        while (true)
        {
            if (double.TryParse(Console.ReadLine(), out double minimumMoveSpeed))
            {
                return minimumMoveSpeed;
            }
            else
            {
                AnsiConsole.Markup("[red]Invalid input.[/] Please answer with a valid [bold]minimum move speed[/]: ");
            }
        }
    }
}