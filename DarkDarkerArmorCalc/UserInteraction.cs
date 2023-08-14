using Spectre.Console;

namespace DarkDarkerArmorCalc;

public class UserInteraction
{
    public static bool ContinueChecker()
    {
        AnsiConsole.Markup("[bold]Would you like to continue? (Y/N): [/]");
        string userInput;

        while (true)
        {
            userInput = Console.ReadLine().Trim().ToLower();

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
}
