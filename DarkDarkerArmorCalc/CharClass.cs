using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DarkDarkerArmorCalc;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharClass
{
    BARBARIAN,
    BARD,
    CLERIC,
    DRUID,
    FIGHTER,
    RANGER,
    ROGUE,
    WARLOCK,
    WIZARD
}