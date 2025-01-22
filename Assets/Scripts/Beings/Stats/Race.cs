using System.Collections.Generic;

public class Race
{
    public string name;
    public string description;
    // Attribute Modifiers (uses Attributes class to store extra dice value vs. actual values)
    public Attributes attributeMods;

    // Affinity Modifiers (uses Affinities class to store extra dice value vs. actual values)
    public Affinities affinityMods;

    // Combat Skill Modifiers (uses CombatSkills class to store extra dice value vs. actual values)
    public CombatSkills combatSkillMods;

    // NonCombat Skill Modifiers (uses NonCombatSkills class to store extra dice value vs. actual values)
    public NonCombatSkills nonCombatSkillMods;

    public List<int> rollableClasses;

    

    Race(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public static List<Race> races = new List<Race> {
        new Race("Unassigned", "")
        {

        },
        new Race("Canid", "Dog-folk known for strength, resilience and loyalty.")
        {
            attributeMods = new Attributes(fortitude: 3, strength: 2, charisma: 1),
            affinityMods = new Affinities(celestial: 3),
            combatSkillMods = new CombatSkills(),
            nonCombatSkillMods = new NonCombatSkills(sentry: 2, medicine: 2, armorSmithing: 2, weaponSmithing: 2, mining: 2, cartography: 2),
            rollableClasses = new List<int> {(int)ClassEnum.Paladin}
        },
        new Race("Felis", "Cat people known for intelligence and magical abilities")
        {
            attributeMods = new Attributes(intelligence: 3, will: 2, fortitude: 1),
            affinityMods = new Affinities(arcana: 3),
            combatSkillMods = new CombatSkills(),
            nonCombatSkillMods = new NonCombatSkills(trapping: 2, tailoring: 2, alchemy: 2, enchanting: 2, jewelryCrafting: 2, monsterWrangling: 2),
            rollableClasses = new List<int> {(int)ClassEnum.Wizard}
        },
        new Race("Mousefolk", "Mouse people known for stealth, agility and perserverence")
        {
            attributeMods = new Attributes(agility: 3, charisma: 2, fortitude: 2),
            affinityMods = new Affinities(nature: 1, spiritual: 1, qi: 1),
            combatSkillMods = new CombatSkills(),
            nonCombatSkillMods = new NonCombatSkills(cooking: 2, fletching: 2, herbalism: 2, leatherWorking: 2, mechanisms: 2, barter: 2),
            rollableClasses = new List<int> {(int)ClassEnum.Rogue},
        },
    };
}

public enum RaceEnum
{
    Unassigned = 0,
    Canid = 1,
    Felis = 2,
    MouseFolk = 3
}