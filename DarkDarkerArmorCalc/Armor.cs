﻿using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DarkDarkerArmorCalc;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class Armor
{
    public string Name { get; set; }
    public ArmorSlot Slot { get; set; }
    public Stats JunkStats { get; set; }
    public CharClass[] AllowedClasses { get; set; }

    public Armor()
    {
        
    }

    public Armor(string name, ArmorSlot slot, CharClass[] allowedClasses, Stats junkStats)
    {
        Name = name;
        Slot = slot;
        AllowedClasses = allowedClasses;
        JunkStats = junkStats;
    }
}