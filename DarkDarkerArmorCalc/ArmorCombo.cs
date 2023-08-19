namespace DarkDarkerArmorCalc;

public class ArmorCombo
{
    public Character Character { get; }
    public Stats TotalStats { get; private set; } = new Stats();
    public List<Armor> Armors { get; } = new List<Armor>();

    public ArmorCombo(IEnumerable<Armor> armors, Character character)
    {
        Character = character;
        foreach (var armor in armors)
            AddArmor(armor);
    }
    public double CalculateFinalMoveSpeed()
    {
        int lowAgilityThreshold = 15;
        int mediumAgilityThreshold = 45;
        int highAgilityThreshold = 65;

        double moveSpeed = Character.BaseMoveSpeed; // Default move speed is 270

        int totalAgility = TotalStats.Agility;

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
        moveSpeed += TotalStats.MovementSpeed;

        return moveSpeed;
    }
    public string CalculateFinalActionSpeed()
    {
        int lowAgilityThreshold = 10;
        int mediumAgilityThreshold1 = 13;
        int mediumAgilityThreshold2 = 25;
        int highAgilityThreshold1 = 41;
        int highAgilityThreshold2 = 50;

        double actionSpeed = -0.38; // Default action speed penalty at 0 agility

        int totalAgility = TotalStats.Agility;

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
        return $"{actionSpeed * 100:0.##}%"; // Format as percentage string
    }
    public double CalculateEffectiveHitPoints()
    {
        int armorRating = TotalStats.ArmorRating;
        double damageReduction = 0.0;

        if (armorRating >= 0 && armorRating <= 10)
        {
            damageReduction = armorRating * 0.01; // 1% damage reduction per ArmorRating
        }
        else if (armorRating <= 19)
        {
            damageReduction = 0.1 + (armorRating - 10) * 0.005; // 0.5% damage reduction per ArmorRating
        }
        else if (armorRating <= 30)
        {
            damageReduction = 0.15 + (armorRating - 19) * 0.004; // 0.4% damage reduction per ArmorRating
        }
        else if (armorRating <= 40)
        {
            damageReduction = 0.23 + (armorRating - 30) * 0.003; // 0.3% damage reduction per ArmorRating
        }
        else if (armorRating <= 50)
        {
            damageReduction = 0.32 + (armorRating - 40) * 0.002; // 0.2% damage reduction per ArmorRating
        }
        else if (armorRating <= 100)
        {
            damageReduction = 0.42 + (armorRating - 50) * 0.001; // 0.1% damage reduction per ArmorRating
        }
        else if (armorRating <= 150)
        {
            damageReduction = 0.52 + (armorRating - 100) * 0.002; // 0.2% damage reduction per ArmorRating
        }
        else if (armorRating <= 250)
        {
            damageReduction = 0.62 + (armorRating - 150) * 0.003; // 0.3% damage reduction per ArmorRating
        }
        else if (armorRating <= 350)
        {
            damageReduction = 0.92 + (armorRating - 250) * 0.002; // 0.2% damage reduction per ArmorRating
        }
        else if (armorRating <= 500)
        {
            damageReduction = 1.22 + (armorRating - 350) * 0.001; // 0.1% damage reduction per ArmorRating
        }
        double maxHealth = 60.0; // Base health for 0 strength

        if (TotalStats.Strength <= 10)
        {
            maxHealth += TotalStats.Strength * 3.0;
        }
        else if (TotalStats.Strength <= 50)
        {
            maxHealth += 10.0 * 3.0 + (TotalStats.Strength - 10) * 2.0;
        }
        else if (TotalStats.Strength <= 75)
        {
            maxHealth += 10.0 * 3.0 + 40.0 * 2.0 + (TotalStats.Strength - 50) * 1.0;
        }
        else if (TotalStats.Strength <= 100)
        {
            maxHealth += 10.0 * 3.0 + 40.0 * 2.0 + 25.0 * 1.0 + (TotalStats.Strength - 75) * 0.5;
        }
        double effectiveHitPoints = (1.0 / (1.0 - damageReduction)) * maxHealth;

        return effectiveHitPoints;
    }
    public void AddArmor(Armor armor)
    {
        Armors.Add(armor);
        UpdateTotalProperties();
    }
    private void UpdateTotalProperties()
    {
        var newTotal = new Stats();
        foreach (var armors in Armors)
            newTotal += armors.JunkStats;
        TotalStats = Character.BaseStats + newTotal;
    }
}