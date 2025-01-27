using System.Collections.Generic;

public class Class
{
    public string name;
    public string description;
    public CombatSkills combatSkillMods;

    Class(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public static List<Class> classes = new List<Class>
    {
        new Class("Unassigned", "")
        {

        },
        new Class("Fighter", "")
        {
            combatSkillMods = new CombatSkills(melee: 2, block: 2, dodge: 1, ranged: 1)
        },
        new Class("Paladin", "")
        {
            combatSkillMods = new CombatSkills(block: 2, healing: 2, melee: 1, auras: 1)
        },
        new Class("Cleric", "")
        {
            combatSkillMods = new CombatSkills(healing: 2, auras: 2, block: 1, evocation: 1)
        },
        new Class("Sorcerer", "")
        {
            combatSkillMods = new CombatSkills(evocation: 3, melee: 2, dodge: 1)
        },
        new Class("Rogue", "")
        {
            combatSkillMods = new CombatSkills(stealth: 2, dodge: 2, melee: 1, ranged: 1)
        },
        new Class("Pack Mule", "")
        {
            combatSkillMods = new CombatSkills(block: 3, stealth: 2)
        },
        new Class("Wizard", "")
        {
            combatSkillMods = new CombatSkills(evocation: 3, auras: 3)
        },
        new Class("Druid", "")
        {
            combatSkillMods = new CombatSkills(healing: 2, auras: 2, evocation: 2)
        },
        new Class("Barbarian", "")
        {
            combatSkillMods = new CombatSkills(melee: 3, dodge: 2, ranged: 1)
        },
        new Class("Bard", "")
        {
            combatSkillMods = new CombatSkills(auras: 3, stealth: 1, dodge: 1, ranged: 1)
        },
        new Class("Monk", "")
        {
            combatSkillMods = new CombatSkills(melee: 3, dodge: 3)
        },
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
