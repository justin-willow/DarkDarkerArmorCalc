using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Character
{
    public string Name { get; set; }
    public CharClass CharClass { get; set; }
    public int BaseMoveSpeed { get; } = 270;
    public Stats BaseStats { get; set; }
    public Race Race { get; set; }

    public Character()
    {

    }

    public Character(string name, Race race, Stats stats)
    {
        Name = name;
        Race = race;
        BaseStats = stats + Race.StatModifier;
    }
}
