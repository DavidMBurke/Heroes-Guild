#nullable enable
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public int cost;
    public int quantity;
    public bool canBePickedUp;
    public float weight = 0;

    public Sprite sprite;

    // crafting
    public float multiplier; // For crafting ingredients with multipliers

    [System.NonSerialized]
    public List<Item> craftingIngredients;

    // equipment
    public bool equippable = false;
    public List<EquipmentSlots.Enum> equipSlots = new List<EquipmentSlots.Enum>();
    public ItemBaseStats baseStats = new ItemBaseStats();
    public List<Effect> effects; // List of effects that are applied in sequence

    // skill modifiers
    public Dictionary<string, float> skillBonuses = new(); // flat added bonus
    public Dictionary<string, float> skillMultipliers = new(); 
    // tags
    public List<string> tags;


    public Item(string itemName = "", int cost = 0, int quantity = 1, float multiplier = 1f, bool equippable = false, List<EquipmentSlots.Enum>? equipSlots = null, ItemBaseStats? baseStats = null, List<Effect>? effects = null, List<string>? tags = null, Dictionary<string, float>? skillBonuses = null, Dictionary<string, float>? skillMultipliers = null, string description = "", List<Item>? craftingIngredients = null, Sprite? sprite = null)
    {
        this.itemName = itemName;
        this.cost = cost;
        this.quantity = quantity;
        this.multiplier = multiplier;
        this.equippable = equippable;
        this.equipSlots = equipSlots ?? new List<EquipmentSlots.Enum>();
        this.baseStats = baseStats ?? new ItemBaseStats();
        this.effects = effects ?? new List<Effect>();
        this.tags = tags ?? new List<string>();
        this.skillBonuses = skillBonuses ?? new();
        this.skillMultipliers = skillMultipliers ?? new();
        this.description = description;
        this.craftingIngredients = craftingIngredients ?? new List<Item>();
        if (sprite != null)
        {
            this.sprite = sprite;
        } else
        {
            this.sprite = Resources.Load<Sprite>("Sprites/GenericItem");
        }
    }

    public Item Clone(int? newQuantity = null)
    {
        return new Item(
            itemName: itemName,
            cost: cost,
            quantity: newQuantity ?? quantity,
            multiplier: multiplier,
            equippable: equippable,
            equipSlots: new List<EquipmentSlots.Enum>(equipSlots),
            baseStats: baseStats.Clone(),
            effects: new List<Effect>(effects),
            tags: new List<string>(tags),
            skillBonuses: new Dictionary<string, float>(skillBonuses),
            skillMultipliers: new Dictionary<string, float>(skillMultipliers),
            description: description,
            craftingIngredients: craftingIngredients,
            sprite: sprite
            );
    }

    public void AddToInventory(List<Item> inventory, int? amount = null)
    {
        Item existingItem = inventory.FirstOrDefault(i => i.itemName == itemName && i.description == description);
        if (existingItem != null)
        {
            existingItem.quantity += amount ?? quantity;
            return;
        }
        else
        {
            Item newItem = Clone();
            newItem.quantity = amount ?? quantity;
            inventory.Add(newItem);
        }
    }

    public void SetModifiers()
    {
        var skillBonusKeys = skillBonuses.Keys.ToList();
        foreach (string key in skillBonusKeys)
        {
            skillBonuses[key] *= multiplier;
        }

        var skillMultiplierKeys = skillMultipliers.Keys.ToList();
        foreach (string key in skillMultiplierKeys)
        {
            skillMultipliers[key] *= multiplier;
        }
    }

    // For generation of every item for debugging
    public static List<List<Item>> allItemLists = new()
    {
        Jewelry.Gems,
        Metals.MetalIngots,
        Metals.MetalOres,
        Fabrics.Cloths,
        Fabrics.Threads,
        Fabrics.BowStrings,
        Food.Meat.RawMeat,
        MonsterParts.Corpses,
        MonsterParts.Essences,
        MonsterParts.Hides,
        MonsterParts.Leathers,
        MonsterParts.Bones,
        MonsterParts.Misc,
        MonsterParts.ProcessedParts,
        PlantParts.Woods,
        PlantParts.Misc
    };

}
