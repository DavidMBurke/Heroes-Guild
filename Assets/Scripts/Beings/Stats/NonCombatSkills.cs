using System.Collections.Generic;

[System.Serializable]
public class NonCombatSkills
{
    public Dictionary<string, Skill> skills = new();

    public NonCombatSkills() {
        foreach (var skillEnum in Names)
        {
            string name = skillEnum.Value;
            skills[name] = new Skill(name);
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

    public static string GetName(Enum skillEnum)
    {
        return Names.TryGetValue(skillEnum, out var name) ? name : "Unknown";
    }

    public enum Enum
    {
        Cooking = 1,
        Sentry,
        Fletching,
        Trapping,
        Herbalism,
        Medicine,
        LeatherWorking,
        Tailoring,
        Alchemy,
        ArmorSmithing,
        WeaponSmithing,
        Enchanting,
        Mechanisms,
        JewelryCrafting,
        Mining,
        MonsterWrangling,
        Cartography,
        Barter
    }


    private static readonly Dictionary<Enum, string> Names = new Dictionary<Enum, string>
    {
        { Enum.Cooking, "Cooking" },
        { Enum.Sentry, "Sentry" },
        { Enum.Fletching, "Fletching" },
        { Enum.Trapping, "Trapping" },
        { Enum.Herbalism, "Herbalism" },
        { Enum.Medicine, "Medicine" },
        { Enum.LeatherWorking, "Leather-Working" },
        { Enum.Tailoring, "Tailoring" },
        { Enum.Alchemy, "Alchemy" },
        { Enum.ArmorSmithing, "Armor-Smithing" },
        { Enum.Enchanting, "Enchanting" },
        { Enum.Mechanisms, "Mechanisms" },
        { Enum.JewelryCrafting, "Jewelry-Crafting" },
        { Enum.Mining, "Mining" },
        { Enum.MonsterWrangling, "Monster-Wrangling" },
        { Enum.Cartography, "Cartography" },
        { Enum.Barter, "Barter" }
    };
}

