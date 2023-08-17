using Spectre.Console;

namespace DarkDarkerArmorCalc;

public class UserInteraction
{
    public static bool ContinueChecker()
    {
        AnsiConsole.Markup("[bold]Would you like to continue? (Y/N): [/]");
        while (true)
        {
            string userInput = Console.ReadLine().Trim().ToLower();

            if (userInput == "y")
            {
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
    public static CharClasses GetValidCharClass()
    {
        AnsiConsole.Markup("Enter your character class: ");
        while (true)
        {
            if (Enum.TryParse(Console.ReadLine().Trim(), true, out CharClasses userCharClass))
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