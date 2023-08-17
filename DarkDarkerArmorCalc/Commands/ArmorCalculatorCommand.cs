using DarkDarkerArmorCalc.Repositories;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DarkDarkerArmorCalc.Command;

internal class ArmorCalculatorCommand : Command<ArmorCalculatorCommand.Settings>
{
    private readonly CharacterRepository characterRepository;
    private readonly RaceRepository raceRepository;
    private readonly ArmorRepository armorRepository;

    public class Settings : CommandSettings
    {
    }

    public ArmorCalculatorCommand(CharacterRepository characterRepository,
        RaceRepository raceRepository,
        ArmorRepository armorRepository)
    {
        this.characterRepository = characterRepository
            ?? throw new ArgumentNullException(nameof(characterRepository));
        this.raceRepository = raceRepository
            ?? throw new ArgumentNullException(nameof(raceRepository));
        this.armorRepository = armorRepository
            ?? throw new ArgumentNullException(nameof(armorRepository));
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        AnsiConsole.Write(new FigletText("Dark and Darker Armor Calc").LeftJustified().Color(Color.Red));

        var validCombos = new List<ArmorCombo>();
        CharClass userCharClass = UserInteraction.GetValidCharClass();
        var classSelection = characterRepository.Get().FirstOrDefault(c => c.CharClass == userCharClass)
            ?? throw new ArgumentNullException("class does not exist");

        var classArmorList = armorRepository.Get().Where(armor => armor.AllowedClasses.Contains(userCharClass));
        var permutations = from head in classArmorList.Where(armor => armor.Slot == ArmorSlot.HEAD)
                           from chest in classArmorList.Where(armor => armor.Slot == ArmorSlot.CHEST)
                           from hand in classArmorList.Where(armor => armor.Slot == ArmorSlot.HANDS)
                           from legs in classArmorList.Where(armor => armor.Slot == ArmorSlot.LEGS)
                           from feet in classArmorList.Where(armor => armor.Slot == ArmorSlot.FEET)
                           select new Tuple<Armor, Armor, Armor, Armor, Armor>(head, chest, hand, legs, feet);

        foreach (var race in raceRepository.Get())
        {
            foreach (var (head, chest, hand, legs, feet) in permutations)
            {
                var set = new List<Armor>() { head, chest, hand, legs, feet };
                validCombos.Add(new(set, new(classSelection, race)));
            }
        }

        bool shouldContinue = true;
        while (shouldContinue)
        {
            double minimumMoveSpeed = UserInteraction.GetValidMinimumMoveSpeed();

            IEnumerable<Armor> distinctArmorList = validCombos
                .SelectMany(combo => combo.Armors)
                .Distinct();

            IEnumerable<ArmorCombo> filteredCombos = validCombos
                .Where(combo => combo.CalculateFinalMoveSpeed() >= minimumMoveSpeed);

            IEnumerable<ArmorCombo> sortedCombos = filteredCombos
                .OrderByDescending(combo => combo.TotalStats.ArmorRating)
                .ThenByDescending(combo => combo.TotalStats.Strength)
                .Take(20);

            foreach (var combo in sortedCombos)
            {
                double finalMoveSpeed = combo.CalculateFinalMoveSpeed();
                string finalActionSpeed = combo.CalculateFinalActionSpeed();

                var table = new Table().Border(TableBorder.AsciiDoubleHead)
                    .AddColumn("[bold]Armor Combo[/]", (config) => config.NoWrap = true)
                    .AddColumn("[bold]Character[/]")
                    .AddColumn("[bold]Total Agility[/]")
                    .AddColumn("[bold]Total Strength[/]")
                    .AddColumn("[bold]Total Will[/]")
                    .AddColumn("[bold]Total Knowledge[/]")
                    .AddColumn("[bold]Total Resourcefulness[/]")
                    .AddColumn("[bold]Total Armor Rating[/]")
                    .AddColumn("[bold]Total Magic Resistance[/]")
                    .AddColumn("[bold]Headshot Reduction[/]")
                    .AddColumn("[bold]Projectile Reduction[/]")
                    .AddColumn("[bold]Final Action Speed[/]")
                    .AddColumn("[bold]Final Move Speed[/]");

                table.AddRow(
                    new Markup($"[deepskyblue2]{BuildArmorComboString(combo)}[/]"),
                    new Markup($"[wheat1]{combo.Character.CharClass} - {combo.Character.Race}[/]"),
                    new Markup($"[green]{combo.TotalStats.Agility}[/]"),
                    new Markup($"[green]{combo.TotalStats.Strength}[/]"),
                    new Markup($"[magenta]{combo.TotalStats.Will}[/]"),
                    new Markup($"[yellow]{combo.TotalStats.Knowledge}[/]"),
                    new Markup($"[blue]{combo.TotalStats.Resourcefulness}[/]"),
                    new Markup($"[white]{combo.TotalStats.ArmorRating}[/]"),
                    new Markup($"[red]{combo.TotalStats.MagicResistance}[/]"),
                    new Markup($"[white]{combo.TotalStats.HeadshotReduction:0.#}%[/]"),
                    new Markup($"[white]{combo.TotalStats.ProjectileReduction:0.#}%[/]"),
                    new Markup($"[magenta]{finalActionSpeed}[/]"),
                    new Markup($"[green]{finalMoveSpeed}[/]")
                );

                AnsiConsole.Write(table);
            }
            shouldContinue = UserInteraction.ContinueChecker();
        }

        return 0;
    }

    static string BuildArmorComboString(ArmorCombo combo)
    {
        StringBuilder sb = new(500);
        var armorNames = combo.Armors.Select(a => $"{a.Slot} - {a.Name}");
        foreach (var name in armorNames)
            sb.AppendLine(name);
        return sb.ToString();
    }
}
