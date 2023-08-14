using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ArmorStats
{
    public int ArmorRating { get; set; }
    public int MovementSpeed { get; set; }
    public int MagicResistance { get; set; }
    public int Agility { get; set; }
    public int Will { get; set; }
    public int Knowledge { get; set; }
    public int Strength { get; set; }
    public int Resourcefulness { get; set; }
    public double HeadshotReduction { get; set; }
}