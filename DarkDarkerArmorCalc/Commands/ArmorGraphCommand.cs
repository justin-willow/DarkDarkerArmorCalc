using DarkDarkerArmorCalc.Repositories;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.ImageSharp;
using OxyPlot.Series;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace DarkDarkerArmorCalc.Commands;

internal class ArmorGraphCommand : Command<ArmorGraphCommand.Settings>
{
    private readonly CharacterRepository characterRepository;
    private readonly RaceRepository raceRepository;
    private readonly ArmorRepository armorRepository;

    public class Settings : CommandSettings
    {
    }
    public ArmorGraphCommand(CharacterRepository characterRepository,
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
        IEnumerable<ArmorCombo> sortedCombos = validCombos
            .OrderByDescending(combo => combo.CalculateEffectiveHitPoints());

        var chartFilePath = CreateAndSaveArmorChart(sortedCombos, userCharClass);
        Console.WriteLine($"Chart saved at: {chartFilePath}");

        static string CreateAndSaveArmorChart(IEnumerable<ArmorCombo> combos, CharClass userCharClass)
        {
            var model = new PlotModel
            {
                Title = $"{userCharClass} Armor Sets",
                Background = OxyColor.FromRgb(255, 255, 255)
            };

            // Configure x-axis with more tick marks and labels
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "EHP",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.DarkGray,
                MajorStep = 1, // Adjust this value as needed
                MinorStep = 0.5,  // Adjust this value as needed
                TickStyle = TickStyle.Outside,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                StringFormat = "0", // Format for tick labels
                Angle = 90
            });

            // Configure y-axis with more tick marks and labels
            model.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Move Speed",
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = OxyColors.DarkGray,
                MajorStep = 1, // Adjust this value as needed
                MinorStep = 0.5,  // Adjust this value as needed
                TickStyle = TickStyle.Outside,
                IsPanEnabled = false,
                IsZoomEnabled = false,
                StringFormat = "0.0" // Format for tick labels
            });

            var uniqueCombos = combos
                .GroupBy(combo => combo.CalculateEffectiveHitPoints()) // Group by armor rating
                .Select(group => group.OrderByDescending(combo => combo.CalculateFinalMoveSpeed()).FirstOrDefault()) // Select highest move speed from each group
                .GroupBy(combo => combo.CalculateFinalMoveSpeed()) // Group by move speed
                .Select(group => group.OrderByDescending(combo => combo.CalculateEffectiveHitPoints()).FirstOrDefault()) // Select highest armor rating from each group
                .ToList();

            var lineSeries = new LineSeries
            {
                Title = "Move Speed",
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2,
                MarkerType = MarkerType.Circle,
                MarkerSize = 3.5,
                MarkerStroke = OxyColors.Blue
            };

            foreach (var combo in uniqueCombos)
            {
                lineSeries.Points.Add(new DataPoint(combo.CalculateEffectiveHitPoints(), combo.CalculateFinalMoveSpeed()));
            }
            model.Series.Add(lineSeries);

            string fileName = $"{userCharClass}plot.png";
            int fileNumber = 1;

            while (File.Exists(fileName))
            {
                fileName = $"{userCharClass}plot({fileNumber++}).png";
            }
            PngExporter.Export(model, fileName, 1920, 1080);

            return fileName;
        }
        return 0;
    }
}