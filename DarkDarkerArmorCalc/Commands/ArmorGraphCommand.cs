using DarkDarkerArmorCalc.Command;
using DarkDarkerArmorCalc.Repositories;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
