using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public string description;
    public int cost;
    public int quantity = 1;
    public bool canBePickedUp;
    public float weight = 0;

    [System.NonSerialized]
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

    // tags
    public List<string> tags;


    public Item(string itemName, int cost, float multiplier = 1f, bool equippable = false, List<EquipmentSlots.Enum> equipSlots = null, ItemBaseStats baseStats = null, List<Effect> effects = null, List<string> tags = null)
    {
        this.itemName = itemName;
        this.cost = cost;
        this.multiplier = multiplier;
        this.equippable = equippable;
        this.equipSlots = equipSlots ?? new List<EquipmentSlots.Enum>();
        this.baseStats = baseStats ?? new ItemBaseStats();
        this.effects = effects ?? new List<Effect>();
        this.tags = tags ?? new List<string>();
    }

    public Item Clone()
    {
        return new Item(
            itemName,
            cost,
            multiplier,
            equippable,
            new List<EquipmentSlots.Enum>(equipSlots),
            baseStats.Clone(),
            new List<Effect>(effects),
            new List<string>(tags)
            );
    }

    public void AddToInventory(List<Item> inventory, int amount)
    {
        Item inventoryItem = inventory.FirstOrDefault(i => i.itemName == itemName);
        if (inventoryItem != null)
        {
            inventoryItem.quantity += amount;
            return;
        }
        Item newItem = Clone();
        newItem.quantity = amount;
        inventory.Add(newItem);
    }

    public static List<List<Item>> itemLists = new()
    {
        Jewelry.Gems,
        Metals.MetalIngots,
        Metals.MetalOres,
        MonsterParts.Corpses,
        MonsterParts.Hides,
        MonsterParts.Leathers,
        MonsterParts.MiscellaneousParts,
        MonsterParts.ProcessedParts
    };

}
