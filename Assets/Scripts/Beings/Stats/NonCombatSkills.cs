using System.Collections.Generic;

[System.Serializable]
public class NonCombatSkills
{
    public Dictionary<string, Skill> skills = new();

    public NonCombatSkills() {
        string[] skillNames = {
            GetName(Enum.Cooking),
            GetName(Enum.Sentry),
            GetName(Enum.Fletching),
            GetName(Enum.Trapping),
            GetName(Enum.Herbalism),
            GetName(Enum.Medicine),
            GetName(Enum.LeatherWorking),
            GetName(Enum.Tailoring),
            GetName(Enum.Alchemy),
            GetName(Enum.ArmorSmithing),
            GetName(Enum.WeaponSmithing),
            GetName(Enum.Enchanting),
            GetName(Enum.Mechanisms),
            GetName(Enum.JewelryCrafting),
            GetName(Enum.Mining),
            GetName(Enum.MonsterWrangling),
            GetName(Enum.Cartography),
            GetName(Enum.Barter)
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
        { Enum.LeatherWorking, "Leather Working" },
        { Enum.Tailoring, "Tailoring" },
        { Enum.Alchemy, "Alchemy" },
        { Enum.ArmorSmithing, "Armor Smithing" },
        { Enum.Enchanting, "Enchanting" },
        { Enum.Mechanisms, "Mechanisms" },
        { Enum.JewelryCrafting, "JewelryCrafting" },
        { Enum.Mining, "Mining" },
        { Enum.MonsterWrangling, "MonsterWrangling" },
        { Enum.Cartography, "Cartography" },
        { Enum.Barter, "Barter" }
    };
}

