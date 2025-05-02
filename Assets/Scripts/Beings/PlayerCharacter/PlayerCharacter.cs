using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Player-controlled character with race, class, attributes, inventory, equipment, and combat logic.
/// </summary>
public class PlayerCharacter : Being
{
    // ========== Character Meta ==========
    public Race race = Race.races[(int)Race.Enum.Unassigned];
    public Class playerClass = Class.classes[(int)Class.Enum.Unassigned];
    public int levelAttained;
    public int salary = 1;

    // ========== Inventory ==========
    private List<Item> _inventory = new List<Item>();
    public new List<Item> inventory
    {
        get => _inventory;
        set
        {
            _inventory = value;
            OnInventoryUpdated?.Invoke();
        }
    }

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated = null!;

    // ========== Unity Lifecycle Methods ==========

    new void Start()
    {
        base.Start();
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogWarning($"{name} could not snap to NavMesh. Check if terrain is baked");
        }
    }

    private new void Update() => base.Update();
    new private void FixedUpdate() => base.FixedUpdate();

    // ========== Turn Logic ==========

    /// <summary>
    /// Initializes values at the start of a character's turn.
    /// </summary>
    public void StartTurn()
    {
        isTurn = true;
        hasMovement = true;
        startingPosition = transform.position;
        actionPoints = maxActionPoints;
    }

    /// <summary>
    /// Ends the character's turn and any active action.
    /// </summary>
    public void EndTurn()
    {
        endMove = true;
        isTurn = false;
        hasMovement = false;
        if (isInCharacterAction) EndCharacterAction();
    }

    /// <summary>
    /// Begins a coroutine-based character action.
    /// </summary>
    public void StartCharacterAction(CharacterAction action)
    {
        if (isInCharacterAction) EndCharacterAction();
        action.endSignal = false;
        currentAction = action;
        StartCoroutine(action.action(action.character, action));
    }

    /// <summary>
    /// Ends the currently executing action.
    /// </summary>
    public void EndCharacterAction()
    {
        isInCharacterAction = false;
        currentAction.EndAction();
    }

    // ========== Inventory Management ==========

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    public void AddToInventory(Item item)
    {
        if (item == null) return;
        item.AddToInventory(inventory, item.quantity);
        OnInventoryUpdated?.Invoke();
    }

    /// <summary>
    /// Removes an item or quantity of item from the inventory.
    /// </summary>
    public void RemoveFromInventory(Item item, int quantity = 1)
    {
        Item inventoryItem = inventory.First(i => i.itemName == item.itemName && i.description == item.description);
        if (inventoryItem == null)
        {
            Debug.LogWarning("Attempted to remove item not in inventory");
            return;
        }

        inventoryItem.quantity -= quantity;
        if (inventoryItem.quantity <= 0)
        {
            inventory.Remove(inventoryItem);
        }

        OnInventoryUpdated?.Invoke();
    }

    // ========== Character Generation & Stats ==========

    /// <summary>
    /// Rolls a stat using dice and enforces a minimum.
    /// </summary>
    public static void RollStat(ref int stat, int minVal, int diceCount, int diceMin, int diceMax)
    {
        stat = 0;
        for (int i = 0; i < diceCount; i++)
        {
            stat += Random.Range(diceMin, diceMax + 1);
        }
        if (stat < minVal) stat = minVal;
    }

    /// <summary>
    /// Creates and returns a new randomized PlayerCharacter.
    /// </summary>
    public static PlayerCharacter CreateNewCharacter()
    {
        GameObject gameObject = new GameObject();
        PlayerCharacter character = gameObject.AddComponent<PlayerCharacter>();
        character.RollNewStats();
        character.name = character.characterName;
        return character;
    }

    /// <summary>
    /// Rolls new stats and generates race, class, skills, and actions.
    /// </summary>
    /// <param name="raceNum">Race enumerator number</param>
    public void RollNewStats(int raceNum = -1)
    {
        if (raceNum < 0)
        {
            int raceRoll = Random.Range(1, Race.races.Count);
            race = Race.races[raceRoll];
            characterName = AssignNewName(raceRoll);
        }
        else
        {
            race = Race.races[raceNum];
            characterName = AssignNewName(raceNum);
        }

        int classRoll = Random.Range(0, race.rollableClasses.Count);
        int classNum = race.rollableClasses[classRoll];
        playerClass = Class.classes[classNum];

        attributes = Attributes.RollBaseAttributes(race.attributeMods);
        affinities = Affinities.RollBaseAffinities(race.affinityMods);
        combatSkills = CombatSkills.RollBaseSkills(playerClass.combatSkillMods);
        nonCombatSkills = NonCombatSkills.RollBaseSkills(race.nonCombatSkillMods);

        level = CalculateCharacterLevel();
        salary = level;

        // TEMPORARY: Action setup (should move to class logic later)
        if (classNum == (int)Class.Enum.Paladin)
        {
            actionList.Add(new CharacterAction((a, act) => Attack.BasicAttack(a, 2, 10, act), this, "Melee Attack"));
        }
        if (classNum == (int)Class.Enum.Rogue)
        {
            actionList.Add(new CharacterAction((a, act) => Attack.BasicAttack(a, 2, 10, act), this, "Melee Attack"));
            actionList.Add(new CharacterAction((a, act) => Attack.BasicAttack(a, 15, 10, act), this, "Ranged Attack"));
        }
        if (classNum == (int)Class.Enum.Wizard)
        {
            actionList.Add(new CharacterAction((a, act) => Spells.FireBall(a, act), this, "Fireball"));
        }

        ApplyEquipmentSkillModifiers();
    }

    /// <summary>
    /// Assigns a new name based on race.
    /// </summary>
    private string AssignNewName(int raceRoll)
    {
        return raceRoll switch
        {
            (int)Race.Enum.Felis => NameGenerator.GenerateFelisName(),
            (int)Race.Enum.Canid => NameGenerator.GenerateCanidName(),
            (int)Race.Enum.MouseFolk => NameGenerator.GenerateMouseFolkName(),
            _ => ""
        };
    }

    /// <summary>
    /// Calculates level based on skill values using weighted scaling.
    /// </summary>
    public int CalculateCharacterLevel()
    {
        float totalLevelPoints = 0;
        int pointsPerLevelUp = 4;

        var nonCombatLevels = nonCombatSkills.skills.Values.Select(s => s.level).OrderByDescending(l => l).ToList();
        var combatLevels = combatSkills.skills.Values.Select(s => s.level).OrderByDescending(l => l).ToList();

        for (int i = 0; i < nonCombatLevels.Count; i++)
            totalLevelPoints += (nonCombatLevels[i] - 1f) / (i + 1);

        for (int i = 0; i < combatLevels.Count; i++)
            totalLevelPoints += (combatLevels[i] - 1f) / (i + 1);

        return (int)(totalLevelPoints / pointsPerLevelUp);
    }

    /// <summary>
    /// Applies equipment-based skill bonuses to the character.
    /// </summary>
    public void ApplyEquipmentSkillModifiers()
    {
        Dictionary<string, float> totalFlatBonuses = new();
        Dictionary<string, float> totalMultipliers = new();

        foreach (var slot in equipmentSlots.equipmentSlots)
        {
            Item equippedItem = slot.Value.item;
            if (equippedItem == null) continue;

            foreach (var bonus in equippedItem.skillBonuses)
            {
                if (!totalFlatBonuses.ContainsKey(bonus.Key))
                    totalFlatBonuses[bonus.Key] = 0;
                totalFlatBonuses[bonus.Key] += bonus.Value;
            }

            foreach (var multiplier in equippedItem.skillMultipliers)
            {
                if (!totalMultipliers.ContainsKey(multiplier.Key))
                    totalMultipliers[multiplier.Key] = 0;
                totalMultipliers[multiplier.Key] += multiplier.Value;
            }
        }

        combatSkills.ApplyEquipmentBonuses(totalFlatBonuses, totalMultipliers);
        nonCombatSkills.ApplyEquipmentBonuses(totalFlatBonuses, totalMultipliers);
    }

    /// <summary>
    /// Logs all equipment currently worn by the character.
    /// </summary>
    public void LogEquipment()
    {
        Debug.Log($"Equipment - {name}:");
        foreach (var slot in equipmentSlots.equipmentSlots)
        {
            string itemName = slot.Value.item != null ? slot.Value.item.itemName : "Empty";
            Debug.Log($"Slot: {slot.Key} - Item: {itemName}");
        }
    }
}
