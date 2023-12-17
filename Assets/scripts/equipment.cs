using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Equipment : MonoBehaviour
{
    public float healthMultiplier;
    public float attackMultiplier;
    public float defenseMultiplier;
    public float speedMultiplier;

    protected Equipment(float healthMultiplier = 1, float attackMultiplier = 1, float defenseMultiplier = 1, float speedMultiplier = 1)
    {
        this.healthMultiplier = healthMultiplier;
        this.attackMultiplier = attackMultiplier;
        this.defenseMultiplier = defenseMultiplier;
        this.speedMultiplier = speedMultiplier;
    }
}

public class Helment : Equipment
{
    public Helment(float healthMultiplier = 1, float attackMultiplier = 1, float defenseMultiplier = 2, float speedMultiplier = 1) : base(healthMultiplier, attackMultiplier, defenseMultiplier, speedMultiplier)
    {
    }
}

public class BodyArmor : Equipment
{
    public BodyArmor(float healthMultiplier = 2, float attackMultiplier = 1, float defenseMultiplier = 1, float speedMultiplier = 1) : base(healthMultiplier, attackMultiplier, defenseMultiplier, speedMultiplier)
    {
    }
}

public class Weapon : Equipment
{
    public Weapon(float healthMultiplier = 1, float attackMultiplier = 2, float defenseMultiplier = 1, float speedMultiplier = 1) : base(healthMultiplier, attackMultiplier, defenseMultiplier, speedMultiplier)
    {
    }
}

public class Boots : Equipment
{
    public Boots(float healthMultiplier = 1, float attackMultiplier = 1, float defenseMultiplier = 1, float speedMultiplier = 2) : base(healthMultiplier, attackMultiplier, defenseMultiplier, speedMultiplier)
    {
    }
}

public enum HelmentItems
{
    CardboardHelment,
    BronzeHelment,
    IronHelment,
    PlatinumHelment,
    TitaniumHelment
}

public enum BodyArmorItems
{
    CardboardBodyArmor,
    BronzeBodyArmor,
    IronBodyArmor,
    PlatinumBodyArmor,
    TitaniumBodyArmor
}

public enum WeaponItems
{
    CardboardSword,
    BronzeSword,
    IronSword,
    PlatinumSword,
    TitaniumSword
}

public enum BootsItems
{
    CardboardBoots,
    BronzeBoots,
    IronBoots,
    PlatinumBoots,
    TitaniumBoots
}
