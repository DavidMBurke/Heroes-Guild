using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Armor
{
    public static Item CreatePlatelegs(Item metalIngot, Item leather)
    {
        string name = $"{leather.itemName}-lined {metalIngot.itemName} Platelegs".Replace(" Ingot", "").Replace(" Leather", "");
        float multiplier = metalIngot.multiplier * leather.multiplier;
        int cost = metalIngot.cost + leather.cost;
        Item platelegs = new Item(name, cost, multiplier);
        platelegs.craftingIngredients = new List<Item>
        {
            metalIngot, leather
        };
        platelegs.multiplier = multiplier;
        platelegs.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        platelegs.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        platelegs.tags = new List<string> { "legs", "armor" };
        platelegs.equipSlots.Add(EquipmentSlots.Enum.Legs);

        platelegs.SetModifiers();

        return platelegs;
    }

    public static Item CreatePlatebody(Item metalIngot, Item leather)
    {
        string name = $"{leather.itemName}-lined {metalIngot.itemName} Platebody".Replace(" Ingot", "").Replace(" Leather", "");
        float multiplier = metalIngot.multiplier * leather.multiplier;
        int cost = metalIngot.cost + leather.cost;
        Item platebody = new Item(name, cost, multiplier);
        platebody.craftingIngredients = new List<Item>
        {
            metalIngot, leather
        };
        platebody.multiplier = multiplier;
        platebody.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        platebody.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        platebody.tags = new List<string> { "body", "armor" };
        platebody.equipSlots.Add(EquipmentSlots.Enum.Legs);

        platebody.SetModifiers();

        return platebody;
    }
}
