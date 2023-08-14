using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Armor
{
    public string Name { get; set; }
    public ArmorSlot Slot { get; set; }
    public ArmorStats JunkStats { get; set; }

    public Armor(string name, ArmorSlot slot, ArmorStats junkStats)
    {
        this.Name = name;
        this.Slot = slot;
        this.JunkStats = junkStats;
    }
}