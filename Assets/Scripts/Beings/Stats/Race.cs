using System.Collections.Generic;

public class Race
{
    public string name;
    public string description;

    public Attributes attributeMods;
    public Affinities affinityMods;
    public CombatSkills combatSkillMods;
    public NonCombatSkills nonCombatSkillMods;

    public List<int> rollableClasses;

    public Race(string name, string description,
        Dictionary<string, int> combatSkillModifiers = null,
        Dictionary<string, int> nonCombatSkillModifiers = null,
        Attributes attributeMods = null,
        Affinities affinityMods = null,
        List<int> rollableClasses = null)
    {
        this.name = name;
        this.description = description;
        this.attributeMods = attributeMods ?? new Attributes();
        this.affinityMods = affinityMods ?? new Affinities();
        combatSkillMods = new CombatSkills();
        nonCombatSkillMods = new NonCombatSkills();
        this.rollableClasses = rollableClasses ?? new List<int>();

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
        new Race("Unassigned", ""),

        new Race("Canid", "Dog-folk known for strength, resilience and loyalty.",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { "Sentry", 2 }, { "Medicine", 2 }, { "ArmorSmithing", 2 },
                { "WeaponSmithing", 2 }, { "Mining", 2 }, { "Cartography", 2 }
            },
            attributeMods: new Attributes(fortitude: 3, strength: 2, charisma: 1),
            affinityMods: new Affinities(celestial: 3),
            rollableClasses: new List<int> { (int)ClassEnum.Paladin }
        ),

        new Race("Felis", "Cat people known for intelligence and magical abilities",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { "Trapping", 2 }, { "Tailoring", 2 }, { "Alchemy", 2 },
                { "Enchanting", 2 }, { "JewelryCrafting", 2 }, { "MonsterWrangling", 2 }
            },
            attributeMods: new Attributes(intelligence: 3, will: 2, fortitude: 1),
            affinityMods: new Affinities(arcana: 3),
            rollableClasses: new List<int> { (int)ClassEnum.Wizard }
        ),

        new Race("Mousefolk", "Mouse people known for stealth, agility and perseverance",
            nonCombatSkillModifiers: new Dictionary<string, int>
            {
                { "Cooking", 2 }, { "Fletching", 2 }, { "Herbalism", 2 },
                { "LeatherWorking", 2 }, { "Mechanisms", 2 }, { "Barter", 2 }
            },
            attributeMods: new Attributes(agility: 3, charisma: 2, fortitude: 2),
            affinityMods: new Affinities(nature: 1, spiritual: 1, qi: 1),
            rollableClasses: new List<int> { (int)ClassEnum.Rogue }
        )
    };
}

public enum RaceEnum
{
    Unassigned = 0,
    Canid = 1,
    Felis = 2,
    MouseFolk = 3
}
