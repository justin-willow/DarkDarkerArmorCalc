using Newtonsoft.Json;
using Spectre.Console;

using DarkDarkerArmorCalc;


var armorJson = File.ReadAllText(@"D:\CODE\DarkDarkerArmorCalc\DarkDarkerArmorCalc\armors.json");
var armorList = JsonConvert.DeserializeObject<List<Armor>>(armorJson);

var characterJson = File.ReadAllText(@"D:\CODE\DarkDarkerArmorCalc\DarkDarkerArmorCalc\characters.json");
var characterList = JsonConvert.DeserializeObject<List<Character>>(characterJson);


List<ArmorCombo> validCombos = new List<ArmorCombo>();

var permutations =  from head in armorList.Where(armor => armor.Slot == ArmorSlot.HEAD)
                    from chest in armorList.Where(armor => armor.Slot == ArmorSlot.CHEST)
                    from hand in armorList.Where(armor => armor.Slot == ArmorSlot.HANDS)
                    from legs in armorList.Where(armor => armor.Slot == ArmorSlot.LEGS)
                    from feet in armorList.Where(armor => armor.Slot == ArmorSlot.FEET)
                    select new Tuple<Armor, Armor, Armor, Armor, Armor>(head, chest, hand, legs, feet);

foreach (var character in characterList)
{
    foreach (var (head, chest, hand, legs, feet) in permutations)
    {
        var set = new List<Armor>() { head, chest, hand, legs, feet };
        ArmorCombo combo = new ArmorCombo(set, character);
        validCombos.Add(combo);
    }
}

bool shouldContinue = true;

while (shouldContinue)
{
    Console.Write("Enter minimum move speed: ");
    double.TryParse(Console.ReadLine(), out double minimumMoveSpeed);

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

        List<string> armorNames = combo.Armors.Select(armor => armor.Name).ToList();
        string armorNamesString = string.Join(", ", armorNames);

        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("[bold]Armor Combo[/]");
        table.AddColumn("[bold]Character[/]");
        table.AddColumn("[bold]Total Agility[/]");
        table.AddColumn("[bold]Total Strength[/]");
        table.AddColumn("[bold]Total Will[/]");
        table.AddColumn("[bold]Total Knowledge[/]");
        table.AddColumn("[bold]Total Resourcefulness[/]");
        table.AddColumn("[bold]Total Armor Rating[/]");
        table.AddColumn("[bold]Total Magic Resistance[/]");
        table.AddColumn("[bold]Final Action Speed[/]");
        table.AddColumn("[bold]Final Move Speed[/]");

        table.AddRow(
            new Markup($"[deepskyblue2]{armorNamesString}[/]"),
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