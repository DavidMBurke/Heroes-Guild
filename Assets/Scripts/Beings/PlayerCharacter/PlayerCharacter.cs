using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Playable character
/// </summary>
public class PlayerCharacter : Being
{
    const int skillLevelsPerCharacterLevel = 4;
    public Race race = Race.races[(int)RaceEnum.Unassigned];
    public Class playerClass = Class.classes[(int)ClassEnum.Unassigned];
    public Attributes attributes = new Attributes();
    public Affinities affinities = new Affinities();
    public CombatSkills combatSkills = new CombatSkills();
    public NonCombatSkills nonCombatSkills = new NonCombatSkills();
    public List<CharacterAction> actionList = new List<CharacterAction>();

    public int maxActionPoints = 1;
    public int actionPoints = 0;
    public bool hasMovement = true;
    public bool endMove = false; // Used to signal move action to end mid-movement.

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated;
    private List<Interactable> _inventory = new List<Interactable>();

    public int levelAttained; // checked against being.level to prompt leveling up.

    public int salary = 10; //coin per week

    public new List<Interactable> inventory
    {
        get => _inventory;
        set
        {
            _inventory = value;
            OnInventoryUpdated?.Invoke();
        }
    }

    new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }

    public void StartTurn()
    {
        isTurn = true;
        hasMovement = true;
        startingPosition = transform.position;
        actionPoints = maxActionPoints;
    }

    public void EndTurn()
    {
        endMove = true;
        isTurn = false;
        hasMovement = false;
        if (isInCharacterAction)
        {
            EndCharacterAction();
        }
    }

    public void StartCharacterAction(CharacterAction action)
    {
        if (isInCharacterAction)
        {
            EndCharacterAction();
        }
        currentAction = action;
        StartCoroutine(action.action(action.character, action));
    }

    public void EndCharacterAction()
    {
        isInCharacterAction = false;
        currentAction.EndAction();
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void AddToInventory(Interactable item)
    {
        if (item == null)
        {
            return;
        }
        inventory.Add(item);
        OnInventoryUpdated?.Invoke();
    }

    public void RemoveFromInventory(Interactable item)
    {
        if (inventory.Remove(item))
        {
            OnInventoryUpdated?.Invoke();
            return;
        }
    }

    public static void RollStat(ref int stat, int diceCount, int diceMin, int diceMax)
    {
        stat = 0;
        for (int i = 0; i < diceCount; i++)
        {
            stat += Random.Range(diceMin, diceMax + 1);
        }
        if (stat < 0) stat = 0;
    }

    /// <summary>
    /// Generate a new random character with random race, class and stats rolled
    /// </summary>
    /// <returns></returns>
    public static PlayerCharacter CreateNewCharacter()
    {
        GameObject gameObject = new GameObject();
        PlayerCharacter character = gameObject.AddComponent<PlayerCharacter>();
        character.RollNewStats();
        character.name = character.characterName;
        return character;
    }

    /// <summary>
    /// Roll race, class, attributes, affinities, combat and non-combat skills
    /// TODO - Move this somewhere else for static character generation methods
    /// </summary>
    public void RollNewStats()
    {
        int raceRoll = Random.Range(1, Race.races.Count);
        race = Race.races[raceRoll];
        characterName = AssignNewName(raceRoll);
        int classRoll = Random.Range(0, race.rollableClasses.Count);
        int classNum = race.rollableClasses[classRoll];
        playerClass = Class.classes[classNum];
        attributes = Attributes.RollBaseAttributes(race.attributeMods);
        affinities = Affinities.RollBaseAffinities(race.affinityMods);
        combatSkills = CombatSkills.RollBaseSkills(playerClass.combatSkillMods);
        nonCombatSkills = NonCombatSkills.RollBaseSkills(race.nonCombatSkillMods);

        /// vvv PLACEHOLDER GARBAGE TO MAKE THIS WORK ON FRONT END vvv
        /// This will be dynamically generated and stored in the future

        if (classNum == (int)ClassEnum.Paladin)
        {
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), this, "Melee Attack"));
        }
        if (classNum == (int)ClassEnum.Rogue)
        {
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), this, "Melee Attack"));
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 15, 10, action), this, "Ranged Attack"));
        }
        if (classNum == (int)ClassEnum.Wizard)
        {
            actionList.Add(new CharacterAction((caster, action) => Spells.FireBall(caster, action), this, "Fireball"));
        }
    }

    private string AssignNewName(int raceRoll)
    {
        if (raceRoll == (int)RaceEnum.Felis)
        {
            return NameGenerator.GenerateFelisName();
        }
        if (raceRoll == (int)RaceEnum.Canid)
        {
            return NameGenerator.GenerateCanidName();
        }
        if (raceRoll == (int)RaceEnum.MouseFolk)
        {
            return NameGenerator.GenerateMouseFolkName();
        }
        return "";
    }

    private int CalculateLevel()
    {
        List<int> skillLevels = new List<int>();
        foreach (FieldInfo field in typeof(CombatSkills).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (field.FieldType == typeof(int)) // Ensure we're only iterating through integer fields
            {
                skillLevels.Add((int)field.GetValue(combatSkills));
            }
        }
        foreach (FieldInfo field in typeof(NonCombatSkills).GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (field.FieldType == typeof(int)) // Ensure we're only iterating through integer fields
            {
                skillLevels.Add((int)field.GetValue(nonCombatSkills));
            }
        }
        List<int> orderedLevels = skillLevels.OrderBy(x => x).ToList();
        float level = 0;
        for (int i = 1; i <= orderedLevels.Count; i++)
        {
            level += (float)orderedLevels[i] * 1f/i;
        }
        return (int)Math.Floor(level / skillLevelsPerCharacterLevel);
    }
}
