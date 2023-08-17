using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Race
{
    public string Name { get; set; }
    public Stats StatModifier { get; set; }

    public Race(string name, Stats statMod)
    {
        Name = name;
        StatModifier = statMod;
    }
}
