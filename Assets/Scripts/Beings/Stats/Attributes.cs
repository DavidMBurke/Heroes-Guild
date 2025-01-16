/// <summary>
/// Most overarching stats of the player, affect almost everything a character does
/// </summary>
public class Attributes
{
    public int strength;
    public int agility;
    public int charisma;
    public int intelligence;
    public int will;
    public int fortitude;

    public Attributes(int strength = 0, int agility = 0, int charisma = 0, int intelligence = 0, int will = 0, int fortitude = 0)
    {
        this.strength = strength;
        this.agility = agility;
        this.charisma = charisma;
        this.intelligence = intelligence;
        this.will = will;
        this.fortitude = fortitude;
    }

    /// <summary>
    /// Roll 6d2 for each stat. Mods add or subtract dice.
    /// </summary>

    public static Attributes RollBaseAttributes(Attributes mods)
    {
        Attributes attributes = new Attributes();
        PlayerCharacter.RollStat(ref attributes.strength, 3 + mods.strength, 1, 2);
        PlayerCharacter.RollStat(ref attributes.agility, 3 + mods.agility, 1, 2);
        PlayerCharacter.RollStat(ref attributes.charisma, 3 + mods.charisma, 1, 2);
        PlayerCharacter.RollStat(ref attributes.intelligence, 3 + mods.intelligence, 1, 2);
        PlayerCharacter.RollStat(ref attributes.will, 3 + mods.will, 1, 2);
        PlayerCharacter.RollStat(ref attributes.fortitude, 3 + mods.fortitude, 1, 2);
        return attributes;
    }
}