using System.Collections.Generic;

[System.Serializable]
public class NonCombatSkills
{
    public Dictionary<string, Skill> skills = new();

    public NonCombatSkills() {
        string[] skillNames = {
            "Cooking", "Sentry", "Fletching", "Trapping", "Herbalism",
            "Medicine", "Leather Working", "Tailoring", "Alchemy", "Armor Smithing",
            "Weapon Smithing", "Enchanting", "Mechanisms", "Jewelry Crafting", "Mining",
            "MonsterWrangling", "Cartography", "Barter"
        };

        foreach (string skillName in skillNames)
        {
            skills[skillName] = new Skill(skillName);
        }
    }

    public static NonCombatSkills RollBaseSkills(NonCombatSkills mod)
    {
        NonCombatSkills rolledSkills = new NonCombatSkills();

        foreach (var skill in rolledSkills.skills)
        {
            int modValue = mod.skills.ContainsKey(skill.Key) ? mod.skills[skill.Key].level : 0;
            PlayerCharacter.RollStat(ref skill.Value.level, 2 + modValue, -1, 2);
        }

        return rolledSkills;
    }
}
