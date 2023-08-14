using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Character
{
    public string Name { get; set; }
    public int Strength { get; set; }
    public int Agility { get; set; }
    public int Will { get; set; }
    public int Knowledge { get; set; }
    public int Resourcefulness { get; set; }
    public int BaseMoveSpeed { get; } = 270;

    public Character(string name, int strength, int agility, int will, int knowledge, int resourcefulness)
    {
        this.Name = name;
        this.Strength = strength;
        this.Agility = agility;
        this.Will = will;
        this.Knowledge = knowledge;
        this.Resourcefulness = resourcefulness;
    }
}