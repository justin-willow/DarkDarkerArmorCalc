using System.Collections.Generic;
using System.Linq;

namespace DarkDarkerArmorCalc;

public class ArmorCombo
{
    public Armor Armor { get; }
    public Character Character { get; }
    public int TotalArmorRating { get; private set; }
    public int TotalMagicResistance { get; private set; }
    public int TotalStrength { get; private set; }
    public int TotalAgility { get; private set; }
    public int TotalWill { get; private set; }
    public int TotalKnowledge { get; private set; }
    public int TotalResourcefulness { get; private set; }
    public int TotalMoveSpeedPenalty { get; private set; }

    public List<Armor> Armors { get; } = new List<Armor>();

    public ArmorCombo(IEnumerable<Armor> armors, Character character)
    {
        this.Character = character;
        foreach (var armor in armors)
            AddArmor(armor);
    }

    public double CalculateFinalMoveSpeed(IEnumerable<Armor> armorCombos)
    {
        int lowAgilityThreshold = 15;
        int mediumAgilityThreshold = 45;
        int highAgilityThreshold = 65;

        double moveSpeed = Character.BaseMoveSpeed; // Default move speed is 270

        int totalAgility = TotalAgility;


        if (totalAgility >= 0 && totalAgility <= lowAgilityThreshold)
        {
            moveSpeed += totalAgility * 2; // 0 to 15: 2 per point
        }
        else if (totalAgility >= lowAgilityThreshold && totalAgility <= mediumAgilityThreshold)
        {
            moveSpeed += lowAgilityThreshold * 2 + (totalAgility - lowAgilityThreshold); // 15 to 45: 1 per point
        }
        else if (totalAgility <= highAgilityThreshold)
        {
            moveSpeed += lowAgilityThreshold * 2 + (mediumAgilityThreshold - lowAgilityThreshold) + ((totalAgility - mediumAgilityThreshold) * 0.5); // 45 to 65: 0.5 per point
        }
        else
        {
            moveSpeed += lowAgilityThreshold * 2 + (mediumAgilityThreshold - lowAgilityThreshold) + ((highAgilityThreshold - mediumAgilityThreshold) * 0.5) + ((totalAgility - highAgilityThreshold) * 0.33); // 65 to 100: 0.33 per point
        }

        moveSpeed += TotalMoveSpeedPenalty;

        return moveSpeed;
    }

    public string CalculateFinalActionSpeed(IEnumerable<Armor> armorCombos)
    {
        int lowAgilityThreshold = 10;
        int mediumAgilityThreshold1 = 13;
        int mediumAgilityThreshold2 = 25;
        int highAgilityThreshold1 = 41;
        int highAgilityThreshold2 = 50;

        double actionSpeed = -0.38; // Default action speed penalty at 0 agility

        int totalAgility = TotalAgility;

        if (totalAgility <= lowAgilityThreshold)
        {
            actionSpeed += (totalAgility + 38) * 0.03; // 0 to 10: 3% per point, adjusted for threshold
        }
        else if (totalAgility <= mediumAgilityThreshold1)
        {
            actionSpeed += lowAgilityThreshold * 0.03 + (totalAgility - lowAgilityThreshold) * 0.02; // 10 to 13: 2% per point
        }
        else if (totalAgility <= mediumAgilityThreshold2)
        {
            actionSpeed += lowAgilityThreshold * 0.03 + (mediumAgilityThreshold1 - lowAgilityThreshold) * 0.02 + (totalAgility - mediumAgilityThreshold1) * 0.01; // 13 to 25: 1% per point
        }
        else if (totalAgility <= highAgilityThreshold1)
        {
            actionSpeed += lowAgilityThreshold * 0.03 + (mediumAgilityThreshold1 - lowAgilityThreshold) * 0.02 + (mediumAgilityThreshold2 - mediumAgilityThreshold1) * 0.01 + (totalAgility - mediumAgilityThreshold2) * 0.015; // 25 to 41: 1.5% per point
        }
        else if (totalAgility <= highAgilityThreshold2)
        {
            actionSpeed += lowAgilityThreshold * 0.03 + (mediumAgilityThreshold1 - lowAgilityThreshold) * 0.02 + (mediumAgilityThreshold2 - mediumAgilityThreshold1) * 0.01 + (highAgilityThreshold1 - mediumAgilityThreshold2) * 0.015 + (totalAgility - highAgilityThreshold1) * 0.01; // 41 to 50: 1% per point
        }
        else
        {
            actionSpeed += lowAgilityThreshold * 0.03 + (mediumAgilityThreshold1 - lowAgilityThreshold) * 0.02 + (mediumAgilityThreshold2 - mediumAgilityThreshold1) * 0.01 + (highAgilityThreshold1 - mediumAgilityThreshold2) * 0.015 + (highAgilityThreshold2 - highAgilityThreshold1) * 0.01 + (totalAgility - highAgilityThreshold2) * 0.005; // 50 to 100: 0.5% per point
        }

        return $"{(actionSpeed * 100):0.##}%"; // Format as percentage string
    }


    public void AddArmor(Armor armor)
    {
        Armors.Add(armor);
        UpdateTotalProperties();
    }

    private void UpdateTotalProperties()
    {
        TotalArmorRating = Armors.Sum(armor => armor.JunkStats.ArmorRating);
        TotalMagicResistance = Armors.Sum(armor => armor.JunkStats.MagicResistance);
        TotalStrength = Armors.Sum(armor => armor.JunkStats.Strength) + Character.Strength;
        TotalAgility = Armors.Sum(armor => armor.JunkStats.Agility) + Character.Agility;
        TotalWill = Armors.Sum(armor => armor.JunkStats.Will) + Character.Will;
        TotalKnowledge = Armors.Sum(armor => armor.JunkStats.Knowledge) + Character.Knowledge;
        TotalResourcefulness = Armors.Sum(armor => armor.JunkStats.Resourcefulness) + Character.Resourcefulness;
        TotalMoveSpeedPenalty = Armors.Sum(armor => armor.JunkStats.MovementSpeed);
    }
}