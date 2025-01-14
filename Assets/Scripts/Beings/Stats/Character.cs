using UnityEngine;

public class Character
{
    public string name;
    public Race race = Race.races[(int)RaceEnum.Unassigned];
    public Class playerClass = Class.classes[(int)ClassEnum.Unassigned];
    public Attributes attributes = new Attributes();
    public Affinities affinities = new Affinities();
    public CombatSkills combatSkills = new CombatSkills();
    public NonCombatSkills nonCombatSkills = new NonCombatSkills();

    public static void RollStat(ref int stat, int diceCount, int diceMin, int diceMax)
    {
        stat = 0;
        for (int i = 0; i < diceCount; i++)
        {
            stat += Random.Range(diceMin, diceMax + 1);
        }
        if (stat < 0) stat = 0;
    }
    
    public static Character CreateNewCharacter()
    {
        Character character = new Character();
        character.RollNewStats();
        return character;
    }

    public void RollNewStats()
    {
        int raceRoll = Random.Range(1, Race.races.Count);
        race = Race.races[raceRoll];
        int classRoll = Random.Range(0, race.rollableClasses.Count);
        int classNum = race.rollableClasses[classRoll];
        playerClass = Class.classes[classNum];
        attributes = Attributes.RollBaseAttributes(race.attributeMods);
        affinities = Affinities.RollBaseAffinities(race.affinityMods);
        combatSkills = CombatSkills.RollBaseSkills(playerClass.combatSkillMods);
        nonCombatSkills = NonCombatSkills.RollBaseSkills(race.nonCombatSkillMods);
    }
}



