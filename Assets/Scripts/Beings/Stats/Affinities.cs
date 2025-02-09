using System.Collections.Generic;

public class Affinities
{
    public Dictionary<string, Affinity> affinities = new();

    public Affinities()
    {
        string[] affinityNames = { 
            GetName(Enum.Nature), 
            GetName(Enum.Arcana), 
            GetName(Enum.Celestial), 
            GetName(Enum.Spiritual), 
            GetName(Enum.Qi) 
        };

        foreach (string affinityName in affinityNames)
        {
            affinities[affinityName] = new Affinity(affinityName);
        }
    }

    public static Affinities RollBaseAffinities(Affinities mods)
    {
        Affinities rolledAffinities = new Affinities();

        foreach (var affinity in rolledAffinities.affinities)
        {
            int modValue = mods.affinities.ContainsKey(affinity.Key) ? mods.affinities[affinity.Key].level : 0;
            PlayerCharacter.RollStat(ref affinity.Value.level, 2 + modValue, 0, 2);
        }
        return rolledAffinities;
    }

    public static string GetName(Enum affinityEnum)
    {
        return Names.TryGetValue(affinityEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Nature = 1,
        Arcana = 2,
        Celestial = 3,
        Spiritual = 4,
        Qi = 5
    }


    private static readonly Dictionary<Enum, string> Names = new Dictionary<Enum, string>
    {
        { Enum.Nature, "Nature" },
        { Enum.Arcana, "Arcana" },
        { Enum.Celestial, "Celestial" },
        { Enum.Spiritual, "Spiritual" },
        { Enum.Qi, "Qi" }
    };
}