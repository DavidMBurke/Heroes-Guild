using System.Collections.Generic;

public class Attributes
{
    public Dictionary<string, Attribute> attributes = new();

    public Attributes()
    {        
        foreach(var attributeEnum in Names)
        {
            string name = attributeEnum.Value;
            attributes[name] = new Attribute(name);
        }
    }

    public static Attributes RollBaseAttributes(Attributes mods)
    {
        Attributes rolledAttributes = new Attributes();

        foreach (var attribute in rolledAttributes.attributes)
        {
            int modValue = mods.attributes.ContainsKey(attribute.Key) ? mods.attributes[attribute.Key].level : 0;
            PlayerCharacter.RollStat(ref attribute.Value.level, 1, 3 + modValue, 1, 2);
        }

        return rolledAttributes;
    }

    public static string GetName(Enum attributeEnum)
    {
        return Names.TryGetValue(attributeEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Strength = 1,
        Agility = 2,
        Charisma = 3,
        Intelligence = 4,
        Will = 5,
        Fortitude = 6
    }


    private static readonly Dictionary<Enum, string> Names = new Dictionary<Enum, string>
    {
        { Enum.Strength, "Strength" },
        { Enum.Agility, "Agility" },
        { Enum.Charisma, "Charisma" },
        { Enum.Intelligence, "Intelligence" },
        { Enum.Will, "Will" },
        { Enum.Fortitude, "Fortitude" }
    };
}