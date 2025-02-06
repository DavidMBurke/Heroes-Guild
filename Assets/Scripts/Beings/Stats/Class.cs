using System.Collections.Generic;

public class Class
{
    public string name;
    public string description;
    public CombatSkills combatSkillMods;

    Class(string name, string description, Dictionary<string, int> combatSkillModifiers = null)
    {
        this.name = name;
        this.description = description;
        this.combatSkillMods = new CombatSkills();

        if (combatSkillModifiers != null)
        {
            foreach (var mod in combatSkillModifiers)
            {
                if (combatSkillMods.skills.ContainsKey(mod.Key))
                {
                    combatSkillMods.skills[mod.Key].level += mod.Value;
                } else
                {
                    combatSkillMods.skills[mod.Key] = new Skill(mod.Key, mod.Value);
                }
            }
        }
    }

    public static List<Class> classes = new List<Class>
    {
        new Class("Unassigned", ""),
        new Class("Fighter", "", new Dictionary<string, int>
        {
            { "Melee", 2 }, { "Block", 2 }, { "Dodge", 1 }, { "Ranged", 1 }
        }),
        new Class("Paladin", "", new Dictionary<string, int>
        {
            { "Block", 2 }, { "Healing", 2 }, { "Melee", 1 }, { "Auras", 1 }
        }),
        new Class("Cleric", "", new Dictionary<string, int>
        {
            { "Healing", 2 }, { "Auras", 2 }, { "Block", 1 }, { "Evocation", 1 }
        }),
        new Class("Sorcerer", "", new Dictionary<string, int>
        {
            { "Evocation", 3 }, { "Melee", 2 }, { "Dodge", 1 }
        }),
        new Class("Rogue", "", new Dictionary<string, int>
        {
            { "Stealth", 2 }, { "Dodge", 2 }, { "Melee", 1 }, { "Ranged", 1 }
        }),
        new Class("Pack Mule", "", new Dictionary<string, int>
        {
            { "Block", 3 }, { "Stealth", 2 }
        }),
        new Class("Wizard", "", new Dictionary<string, int>
        {
            { "Evocation", 3 }, { "Auras", 3 }
        }),
        new Class("Druid", "", new Dictionary<string, int>
        {
            { "Healing", 2 }, { "Auras", 2 }, { "Evocation", 2 }
        }),
        new Class("Barbarian", "", new Dictionary<string, int>
        {
            { "Melee", 3 }, { "Dodge", 2 }, { "Ranged", 1 }
        }),
        new Class("Bard", "", new Dictionary<string, int>
        {
            { "Auras", 3 }, { "Stealth", 1 }, { "Dodge", 1 }, { "Ranged", 1 }
        }),
        new Class("Monk", "", new Dictionary<string, int>
        {
            { "Melee", 3 }, { "Dodge", 3 }
        })
    };
}
    public enum ClassEnum
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
