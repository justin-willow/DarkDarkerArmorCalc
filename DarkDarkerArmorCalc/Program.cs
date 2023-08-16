using Newtonsoft.Json;
using System.Reflection;
using Spectre.Console;
using System.Text;

using DarkDarkerArmorCalc;

string assemblyLocation = Assembly.GetExecutingAssembly().Location;
string? assemblyDirectory = Path.GetDirectoryName(assemblyLocation);

if (string.IsNullOrEmpty(assemblyDirectory))
    throw new ApplicationException("unable to detemine assembly directory");

var armorJson = File.ReadAllText(Path.Join(assemblyDirectory, "armors.json"));
var armorList = JsonConvert.DeserializeObject<List<Armor>>(armorJson);

var characterJson = File.ReadAllText(Path.Join(assemblyDirectory, "characters.json"));
var characterList = JsonConvert.DeserializeObject<List<Character>>(characterJson);

if (armorList is null)
    throw new ApplicationException("unable to find armor.json source");

if (characterList is null)
    throw new ApplicationException("unable to find characters.json source");

List<ArmorCombo> validCombos = new();



Console.Write("Enter your character class: ");
string userInput = Console.ReadLine().Trim().ToUpper();
Enum.TryParse(userInput, out CharClasses userCharClass);


var permutations = from head in armorList.Where(armor => armor.Slot == ArmorSlot.HEAD && armor.AllowedClasses.Contains(userCharClass))
                   from chest in armorList.Where(armor => armor.Slot == ArmorSlot.CHEST && armor.AllowedClasses.Contains(userCharClass))
                   from hand in armorList.Where(armor => armor.Slot == ArmorSlot.HANDS && armor.AllowedClasses.Contains(userCharClass))
                   from legs in armorList.Where(armor => armor.Slot == ArmorSlot.LEGS && armor.AllowedClasses.Contains(userCharClass))
                   from feet in armorList.Where(armor => armor.Slot == ArmorSlot.FEET && armor.AllowedClasses.Contains(userCharClass))
                   select new Tuple<Armor, Armor, Armor, Armor, Armor>(head, chest, hand, legs, feet);


foreach (var character in characterList)
{
    foreach (var (head, chest, hand, legs, feet) in permutations)
    {
        var set = new List<Armor>() { head, chest, hand, legs, feet };
        ArmorCombo combo = new(set, character);
        validCombos.Add(combo);
    }
}

bool shouldContinue = true;

while (shouldContinue)
{
    Console.Write("Enter minimum move speed: ");
    _ = double.TryParse(Console.ReadLine(), out double minimumMoveSpeed);

    IEnumerable<Armor> distinctArmorList = validCombos
        .Select(combo => combo.Armor)
        .Distinct();

    IEnumerable<ArmorCombo> filteredCombos = validCombos
        .Where(combo => combo.CalculateFinalMoveSpeed(distinctArmorList) >= minimumMoveSpeed);

    IEnumerable<ArmorCombo> sortedCombos = filteredCombos
        .OrderByDescending(combo => combo.TotalArmorRating)
        .ThenByDescending(combo => combo.TotalStrength)
        .Take(20);

    foreach (var combo in sortedCombos)
    {
        double finalMoveSpeed = combo.CalculateFinalMoveSpeed(distinctArmorList);
        string finalActionSpeed = combo.CalculateFinalActionSpeed(distinctArmorList);

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
            .AddColumn("[bold]Final Action Speed[/]")
            .AddColumn("[bold]Final Move Speed[/]");

        table.AddRow(
            new Markup($"[deepskyblue2]{BuildArmorComboString(combo)}[/]"),
            new Markup($"[wheat1]{combo.Character.Name}[/]"),
            new Markup($"[green]{combo.TotalAgility}[/]"),
            new Markup($"[green]{combo.TotalStrength}[/]"),
            new Markup($"[magenta]{combo.TotalWill}[/]"),
            new Markup($"[yellow]{combo.TotalKnowledge}[/]"),
            new Markup($"[blue]{combo.TotalResourcefulness}[/]"),
            new Markup($"[white]{combo.TotalArmorRating}[/]"),
            new Markup($"[red]{combo.TotalMagicResistance}[/]"),
            new Markup($"[magenta]{finalActionSpeed}[/]"),
            new Markup($"[green]{finalMoveSpeed}[/]")
        );

        AnsiConsole.Write(table);
    }
    shouldContinue = UserInteraction.ContinueChecker();
}

static string BuildArmorComboString(ArmorCombo combo)
{
    StringBuilder sb = new(500);
    var armorNames = combo.Armors.Select(a => $"{a.Slot} - {a.Name}");
    foreach (var name in armorNames)
        sb.AppendLine(name);
    return sb.ToString();
}