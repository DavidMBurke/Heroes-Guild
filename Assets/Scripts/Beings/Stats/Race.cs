using System.Collections.Generic;

public class Race
{
    public string name;
    public string description;

    public Attributes? attributeMods;
    public Affinities? affinityMods;
    public CombatSkills? combatSkillMods;
    public NonCombatSkills? nonCombatSkillMods;

    public List<int>? rollableClasses;

    private static readonly Dictionary<Enum, string> Names = new Dictionary<Enum, string>
    {
        { Enum.Unassigned, "Unassigned" },
        { Enum.Canid, "Canid" },
        { Enum.Felis, "Felis" },
        { Enum.MouseFolk, "MouseFolk" }
    };


    public Race(
        string name,
        string description,
        Dictionary<string, int>? combatSkillModifiers = null,
        Dictionary<string, int>? nonCombatSkillModifiers = null,
        Dictionary<string, int>? attributeModifiers = null,
        Dictionary<string, int>? affinityModifiers = null,
        List<int>? rollableClasses = null)
    {
        this.name = name;
        this.description = description;

        combatSkillMods ??= new CombatSkills();
        nonCombatSkillMods ??= new NonCombatSkills();
        attributeMods ??= new Attributes();
        affinityMods ??= new Affinities();
        this.rollableClasses = rollableClasses ?? new List<int>();

        if (attributeModifiers != null)
        {
            foreach (var mod in attributeModifiers)
            {
                if (attributeMods.attributes.ContainsKey(mod.Key))
                {
                    attributeMods.attributes[mod.Key].level += mod.Value;
                }
                else
                {
                    attributeMods.attributes[mod.Key] = new Attribute(mod.Key, mod.Value);
                }
            }
        }

        if (affinityModifiers != null)
        {
            foreach (var mod in affinityModifiers)
            {
                if (affinityMods.affinities.ContainsKey(mod.Key))
                {
                    affinityMods.affinities[mod.Key].level += mod.Value;
                }
                else
                {
                    affinityMods.affinities[mod.Key] = new Affinity(mod.Key, mod.Value);
                }
            }
        }

        if (combatSkillModifiers != null)
        {
            foreach (var mod in combatSkillModifiers)
            {
                if (combatSkillMods.skills.ContainsKey(mod.Key))
                {
                    combatSkillMods.skills[mod.Key].level += mod.Value;
                }
                else
                {
                    combatSkillMods.skills[mod.Key] = new Skill(mod.Key, mod.Value);
                }
            }
        }

        if (nonCombatSkillModifiers != null)
        {
            foreach (var mod in nonCombatSkillModifiers)
            {
                if (nonCombatSkillMods.skills.ContainsKey(mod.Key))
                {
                    nonCombatSkillMods.skills[mod.Key].level += mod.Value;
                }
                else
                {
                    nonCombatSkillMods.skills[mod.Key] = new Skill(mod.Key, mod.Value);
                }
            }
        }

    }

    public static List<Race> races = new List<Race>
    {
        new Race(GetName(Enum.Unassigned), ""),

        new Race(GetName(Enum.Canid), "Dog-folk known for strength, resilience and loyalty.",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Sentry), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Medicine), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.ArmorSmithing), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.WeaponSmithing), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Mining), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Cartography), 2 }
            },
            attributeModifiers: new Dictionary<string, int>
            {
                { Attributes.GetName(Attributes.Enum.Fortitude), 3 },
                { Attributes.GetName(Attributes.Enum.Strength), 2 },
                { Attributes.GetName(Attributes.Enum.Charisma), 1 }
            },
            affinityModifiers: new Dictionary<string, int>
            {
                { Affinities.GetName(Affinities.Enum.Celestial), 3 }
            },
            rollableClasses: new List<int> { (int)Class.Enum.Paladin }
        ),

        new Race(GetName(Enum.Felis), "Cat people known for intelligence and magical abilities",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Trapping), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Tailoring), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Alchemy), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Enchanting), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.MonsterWrangling), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Arcana), 2 }
            },
            attributeModifiers: new Dictionary<string, int>
            {
                { Attributes.GetName(Attributes.Enum.Intelligence), 3 },
                { Attributes.GetName(Attributes.Enum.Will), 2 },
                { Attributes.GetName(Attributes.Enum.Fortitude), 1 }
            },
            affinityModifiers: new Dictionary<string, int>
            {
                { Affinities.GetName(Affinities.Enum.Arcana), 3 }
            },
            rollableClasses: new List<int> { (int)Class.Enum.Wizard }
        ),

        new Race(GetName(Enum.MouseFolk), "Mouse people known for stealth, agility and perseverance",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Cooking), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Fletching), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Herbalism), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.LeatherWorking), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Mechanisms), 2 },
                { NonCombatSkills.GetName(NonCombatSkills.Enum.Barter), 2 }
            },
            attributeModifiers: new Dictionary<string, int>
            {
                { Attributes.GetName(Attributes.Enum.Agility), 3 },
                { Attributes.GetName(Attributes.Enum.Charisma), 2 },
                { Attributes.GetName(Attributes.Enum.Fortitude), 2 }
            },
            affinityModifiers: new Dictionary<string, int>
            {
                { Affinities.GetName(Affinities.Enum.Nature), 1 },
                { Affinities.GetName(Affinities.Enum.Spiritual), 1 },
                { Affinities.GetName(Affinities.Enum.Qi), 1 }
            },
            rollableClasses: new List<int> { (int)Class.Enum.Rogue }
        )
    };

    public static string GetName(Enum raceEnum)
    {
        return Names.TryGetValue(raceEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Unassigned = 0,
        Canid = 1,
        Felis = 2,
        MouseFolk = 3
    }

}

