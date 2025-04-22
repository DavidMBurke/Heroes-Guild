using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Playable character
/// </summary>
public class PlayerCharacter : Being
{
    public Race race = Race.races[(int)Race.Enum.Unassigned];
    public Class playerClass = Class.classes[(int)Class.Enum.Unassigned];
    public Attributes attributes = new Attributes();
    public Affinities affinities = new Affinities();
    [SerializeField]
    public CombatSkills combatSkills = new CombatSkills();
    [SerializeField]
    public NonCombatSkills nonCombatSkills = new NonCombatSkills();
    public List<CharacterAction> actionList = new List<CharacterAction>();
    [SerializeField]
    public EquipmentSlots equipmentSlots = new EquipmentSlots();

    public int maxActionPoints = 1;
    public int actionPoints = 0;
    public bool hasMovement = true;
    public bool endMove = false; // Used to signal move action to end mid-movement.

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated = null!; 
    private List<Item> _inventory = new List<Item>();

    public int levelAttained; // checked against being.level to prompt leveling up.

    public int salary = 1; //coin per day

    public new List<Item> inventory
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
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        } else
        {
            Debug.LogWarning($"{name} could not snap to NavMesh. Check if terrain is baked");
        }
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

    public void AddToInventory(Item item)
    {
        if (item == null)
        {
            return;
        }
        item.AddToInventory(inventory, item.quantity);
        OnInventoryUpdated?.Invoke();
    }

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
        level = CalculateCharacterLevel();
        salary = level;

        /// vvv PLACEHOLDER GARBAGE TO MAKE THIS WORK ON FRONT END vvv
        /// This will be dynamically generated and stored in the future

        if (classNum == (int)Class.Enum.Paladin)
        {
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), this, "Melee Attack"));
        }
        if (classNum == (int)Class.Enum.Rogue)
        {
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), this, "Melee Attack"));
            actionList.Add(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 15, 10, action), this, "Ranged Attack"));
        }
        if (classNum == (int)Class.Enum.Wizard)
        {
            actionList.Add(new CharacterAction((caster, action) => Spells.FireBall(caster, action), this, "Fireball"));
        }
        ApplyEquipmentSkillModifiers();
    }

    private string AssignNewName(int raceRoll)
    {
        if (raceRoll == (int)Race.Enum.Felis)
        {
            return NameGenerator.GenerateFelisName();
        }
        if (raceRoll == (int)Race.Enum.Canid)
        {
            return NameGenerator.GenerateCanidName();
        }
        if (raceRoll == (int)Race.Enum.MouseFolk)
        {
            return NameGenerator.GenerateMouseFolkName();
        }
        return "";
    }

    public int CalculateCharacterLevel()
    {
        float totalLevelPoints = 0;
        int pointsPerLevelUp = 4;
        List<int> nonCombatSkillLevels = new();
        List<int> combatSkillLevels = new();
        foreach (var skill in nonCombatSkills.skills)
        {
            nonCombatSkillLevels.Add(skill.Value.level);
        }
        foreach (var skill in combatSkills.skills)
        {
            combatSkillLevels.Add(skill.Value.level);
        }
        nonCombatSkillLevels.Sort((a,b) => b.CompareTo(a));
        combatSkillLevels.Sort((a,b) => b.CompareTo(a));
        string logText = "Noncombat Skills: ";
        for(int i = 0; i < nonCombatSkillLevels.Count; i++)
        {
            totalLevelPoints += ((float)nonCombatSkillLevels[i] - 1) / (i + 1);
            logText += " ";
            logText += nonCombatSkillLevels[i].ToString();
        }
        logText += "Combat Skills: ";
        for(int i = 0; i < combatSkillLevels.Count; i++)
        {
            totalLevelPoints += ((float)combatSkillLevels[i] - 1) / (i + 1);
            logText += " ";
            logText += combatSkillLevels[i];
        }

        logText += $" totalLevelPoints: {totalLevelPoints}";
        int level = (int)(totalLevelPoints / pointsPerLevelUp);
        logText += $" level: {level}";
        
        return level;
    }

    public void ApplyEquipmentSkillModifiers()
    {
        Dictionary<string, float> totalFlatBonuses = new();
        Dictionary<string, float> totalMultipliers = new();

        foreach (var slot in equipmentSlots.equipmentSlots)
        {
            Item equippedItem = slot.Value.item;
            if (equippedItem != null)
            {
                foreach (var bonus in equippedItem.skillBonuses)
                {
                    if (!totalFlatBonuses.ContainsKey(bonus.Key))
                    {
                        totalFlatBonuses[bonus.Key] = 0;
                    }
                    totalFlatBonuses[bonus.Key] += bonus.Value;
                }

                foreach (var multiplier in equippedItem.skillMultipliers)
                {
                    if (!totalMultipliers.ContainsKey(multiplier.Key))
                    {
                        totalMultipliers[multiplier.Key] = 0;
                    }
                    totalMultipliers[multiplier.Key] += multiplier.Value;
                }
            }
        }
        combatSkills.ApplyEquipmentBonuses(totalFlatBonuses, totalMultipliers);
        nonCombatSkills.ApplyEquipmentBonuses(totalFlatBonuses, totalMultipliers);
    }
    
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
