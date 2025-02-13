using System.Collections.Generic;

[System.Serializable]
public class CombatSkills
{
    public Dictionary<string, Skill> skills = new();

    public CombatSkills()
    {
        foreach (var skillEnum in Names)
        {
            string name = skillEnum.Value;
            skills[name] = new Skill(name);
        }
    }

    public static CombatSkills RollBaseSkills(CombatSkills mods)
    {
        CombatSkills rolledSkills = new CombatSkills();

        foreach (var skill in rolledSkills.skills)
        {
            int modValue = mods.skills.ContainsKey(skill.Key) ? mods.skills[skill.Key].level : 1;
            PlayerCharacter.RollStat(ref skill.Value.level, 2 + modValue, 0, 2);
        }

        return rolledSkills;
    }

    public static string GetName(Enum skillEnum)
    {
        return Names.TryGetValue(skillEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Dodge = 1,
        Block = 2,
        Stealth = 3,
        Melee = 4,
        Ranged = 5,
        Healing = 6,
        Auras = 7,
        Evocation = 8
    }


    private static readonly Dictionary<Enum, string> Names = new()
    {
        { Enum.Dodge, "Dodge" },
        { Enum.Block, "Block" },
        { Enum.Stealth, "Stealth" },
        { Enum.Melee, "Melee" },
        { Enum.Ranged, "Ranged" },
        { Enum.Healing, "Healing" },
        { Enum.Auras, "Auras" },
        { Enum.Evocation, "Evocation" }
    };
}
