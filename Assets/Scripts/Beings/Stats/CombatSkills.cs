using System.Collections.Generic;

[System.Serializable]
public class CombatSkills
{
    public Dictionary<string, Skill> skills = new();

    public CombatSkills()
    {
        string[] skillNames = { "Dodge", "Block", "Stealth", "Melee", "Ranged", "Healing", "Auras", "Evocation" };

        foreach (string skillName in skillNames)
        {
            skills[skillName] = new Skill(skillName);
        }
    }

    public static CombatSkills RollBaseSkills(CombatSkills mod)
    {
        CombatSkills rolledSkills = new CombatSkills();

        foreach (var skill in rolledSkills.skills)
        {
            int modValue = mod.skills.ContainsKey(skill.Key) ? mod.skills[skill.Key].level : 0;
            PlayerCharacter.RollStat(ref skill.Value.level, 2 + modValue, 0, 2);
        }

        return rolledSkills;
    }
}
