using UnityEngine;

public class Attribute
{
    public string name;
    public int level;
    public float modifiedLevel;

    public Attribute(string name, int level = 0)
    {
        this.name = name;
        this.level = level;
        this.modifiedLevel = level;
    }

    public void ApplyModifiers(float additiveBonus, float multiplier)
    {
        modifiedLevel = (level + additiveBonus) * multiplier;
    }

}
