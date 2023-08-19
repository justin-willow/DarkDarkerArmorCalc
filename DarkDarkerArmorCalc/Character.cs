using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Character
{
    public Race Race { get; set; }
    public CharClass CharClass { get; set; }
    public int BaseMoveSpeed { get; } = 270;
    public Stats BaseStats { get; set; }

    public Character()
    {

    }
    public Character(Character character, Race race)
    {
        Race = race;
        CharClass = character.CharClass;
        BaseMoveSpeed = character.BaseMoveSpeed;
        BaseStats = character.BaseStats + Race.StatModifier;
    }
    public Character(Race race, Stats stats)
    {
        Race = race;
        BaseStats = stats + Race.StatModifier;
    }
}
