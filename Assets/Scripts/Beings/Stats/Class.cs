using System.Collections.Generic;

public class Class
{
    public string name;
    public string description;
    public Attributes attributeMods;
    public Affinities affinityMods;
    public CombatSkills combatSkillMods;
    public NonCombatSkills nonCombatSkillMods;


    private static readonly Dictionary<Enum, string> Names = new Dictionary<Enum, string>
    {
        { Enum.Unassigned, "Unassigned" },
        { Enum.Fighter, "Fighter" },
        { Enum.Paladin, "Paladin" },
        { Enum.Cleric, "Cleric" },
        { Enum.Sorcerer, "Sorcerer" },
        { Enum.Rogue, "Rogue" },
        { Enum.PackMule, "Pack Mule" },
        { Enum.Wizard, "Wizard" },
        { Enum.Druid, "Druid" },
        { Enum.Barbarian, "Barbarian" },
        { Enum.Bard, "Bard" },
        { Enum.Monk, "Monk" }
    };

    Class(
        string name, 
        string description, 
        Dictionary<string, int> combatSkillModifiers = null,
        Dictionary<string, int> nonCombatSkillModifiers = null,
        Dictionary<string, int> attributeModifiers = null,
        Dictionary<string, int> affinityModifiers = null)
    {
        this.name = name;
        this.description = description;
        combatSkillMods = new CombatSkills();
        nonCombatSkillMods = new NonCombatSkills();
        attributeMods = new Attributes();
        affinityMods = new Affinities();



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

    public static List<Class> classes = new List<Class>
{
    new Class(GetName(Enum.Unassigned), ""),
    new Class(GetName(Enum.Fighter), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Block), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1 }
    }),
    new Class(GetName(Enum.Paladin), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Block), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Healing), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Auras), 1 }
    }),
    new Class(GetName(Enum.Cleric), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Healing), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Auras), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Block), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1 }
    }),
    new Class(GetName(Enum.Sorcerer), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Evocation), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1 }
    }),
    new Class(GetName(Enum.Rogue), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Stealth), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1 }
    }),
    new Class(GetName(Enum.PackMule), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Block), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Stealth), 2 }
    }),
    new Class(GetName(Enum.Wizard), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Evocation), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Auras), 3 }
    }),
    new Class(GetName(Enum.Druid), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Healing), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Auras), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Evocation), 2 }
    }),
    new Class(GetName(Enum.Barbarian), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 2 },
        { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1 }
    }),
    new Class(GetName(Enum.Bard), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Auras), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1 },
        { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1 }
    }),
    new Class(GetName(Enum.Monk), "", new Dictionary<string, int>
    {
        { CombatSkills.GetName(CombatSkills.Enum.Melee), 3 },
        { CombatSkills.GetName(CombatSkills.Enum.Dodge), 3 }
    })
};

    public static string GetName(Enum classEnum)
    {
        return Names.TryGetValue(classEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Unassigned = 0,
        Fighter = 1,
        Paladin = 2,
        Cleric = 3,
        Sorcerer = 4,
        Rogue = 5,
        PackMule = 6,
        Wizard = 7,
        Druid = 8,
        Barbarian = 9,
        Bard = 10,
        Monk = 11
    }

}

        