using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DarkDarkerArmorCalc;

[JsonConverter(typeof(StringEnumConverter))]
public enum ArmorSlot
{
    HEAD,
    CHEST,
    HANDS,
    LEGS,
    FEET
}