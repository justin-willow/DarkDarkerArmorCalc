using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Armor
{
    public string Name { get; set; }
    public ArmorSlot Slot { get; set; }
    public CharClasses[] AllowedClasses { get; set; }
    public ArmorStats JunkStats { get; set; }

    public Armor(string name, ArmorSlot slot, CharClasses[] allowedClasses, ArmorStats junkStats)
    {
        this.Name = name;
        this.Slot = slot;
        this.AllowedClasses = allowedClasses;
        this.JunkStats = junkStats;
    }
}