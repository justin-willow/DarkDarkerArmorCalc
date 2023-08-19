using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public struct Stats
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
    public double ProjectileReduction { get; set; }

    public Stats(int armorRating,
        int movementSpeed,
        int magicResistence,
        int agility,
        int will,
        int knowledge,
        int strength,
        int resourcefulness,
        double headshotReduction,
        double projectileReduction)
    {
        ArmorRating = armorRating;
        MovementSpeed = movementSpeed;
        MagicResistance = magicResistence;
        Agility = agility;
        Will = will;
        Knowledge = knowledge;
        Strength = strength;
        Resourcefulness = resourcefulness;
        HeadshotReduction = headshotReduction;
        ProjectileReduction = projectileReduction;
    }
    public static Stats operator +(Stats a, Stats b)
    {
        return new(
            a.ArmorRating + b.ArmorRating,
            a.MovementSpeed + b.MovementSpeed,
            a.MagicResistance + b.MagicResistance,
            a.Agility + b.Agility,
            a.Will + b.Will,
            a.Knowledge + b.Knowledge,
            a.Strength + b.Strength,
            a.Resourcefulness + b.Resourcefulness,
            a.HeadshotReduction + b.HeadshotReduction,
            a.ProjectileReduction + b.ProjectileReduction
        );
    }
    public static Stats operator -(Stats a, Stats b)
    {
        return new(
            a.ArmorRating - b.ArmorRating,
            a.MovementSpeed - b.MovementSpeed,
            a.MagicResistance - b.MagicResistance,
            a.Agility - b.Agility,
            a.Will - b.Will,
            a.Knowledge - b.Knowledge,
            a.Strength - b.Strength,
            a.Resourcefulness - b.Resourcefulness,
            a.HeadshotReduction - b.HeadshotReduction,
            a.ProjectileReduction - b.ProjectileReduction
        );
    }
}