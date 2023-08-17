using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DarkDarkerArmorCalc;

[JsonConverter(typeof(StringEnumConverter))]
public enum CharClasses
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