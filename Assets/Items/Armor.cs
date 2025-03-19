using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Armor
{
    public class Plate
    {
        public static Item CreatePlatelegs(Item metalIngot, Item leather)
        {
            string name = $"{metalIngot.itemName} Platelegs".Replace(" Ingot", "");
            string description = $"{name} padded with {leather.itemName}";
            float multiplier = metalIngot.multiplier * leather.multiplier;
            int cost = metalIngot.cost + leather.cost;
            Item platelegs = new Item(name, cost, multiplier: multiplier, description: description);
            platelegs.craftingIngredients = new List<Item>
            {
                metalIngot, leather
            };
            platelegs.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            platelegs.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            platelegs.tags = new List<string> { "legs", "armor" };
            platelegs.equipSlots.Add(EquipmentSlots.Enum.Legs);

            platelegs.SetModifiers();

            return platelegs;
        }

        public static Item CreatePlatebody(Item metalIngot, Item leather)
        {
            string name = $"{metalIngot.itemName} Platebody".Replace(" Ingot", "");
            string description = $"{name} padded with {leather.itemName}";
            float multiplier = metalIngot.multiplier * leather.multiplier;
            int cost = metalIngot.cost + leather.cost;
            Item platebody = new Item(name, cost, multiplier: multiplier, description: description);
            platebody.craftingIngredients = new List<Item>
            {
                metalIngot, leather
            };
            platebody.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            platebody.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            platebody.tags = new List<string> { "body", "armor" };
            platebody.equipSlots.Add(EquipmentSlots.Enum.Body);
            platebody.SetModifiers();

            return platebody;
        }

        public static Item CreateHelmet(Item metalIngot, Item leather)
        {
            string name = $"{metalIngot.itemName} Helmet".Replace(" Ingot", "");
            string description = $"{name} padded with {leather.itemName}";
            float multiplier = metalIngot.multiplier * leather.multiplier;
            int cost = metalIngot.cost + leather.cost;
            Item helmet = new Item(name, cost, multiplier: multiplier, description: description);
            helmet.craftingIngredients = new List<Item>
            {
                metalIngot, leather
            };
            helmet.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            helmet.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            helmet.tags = new List<string> { "head", "armor" };
            helmet.equipSlots.Add(EquipmentSlots.Enum.Head);
            helmet.SetModifiers();

            return helmet;
        }

        public static Item CreateGauntlets(Item metalIngot, Item leather)
        {
            string name = $"{metalIngot.itemName} Gauntlets".Replace(" Ingot", "");
            string description = $"{name} padded with {leather.itemName}";
            float multiplier = metalIngot.multiplier * leather.multiplier;
            int cost = metalIngot.cost + leather.cost;
            Item gauntlets = new Item(name, cost, multiplier: multiplier, description: description);
            gauntlets.craftingIngredients = new List<Item>
            {
                metalIngot, leather
            };
            gauntlets.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            gauntlets.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            gauntlets.tags = new List<string> { "hands", "armor" };
            gauntlets.equipSlots.Add(EquipmentSlots.Enum.Hands);
            gauntlets.SetModifiers();

            return gauntlets;
        }

        public static Item CreateBoots(Item metalIngot, Item leather)
        {
            string name = $"{metalIngot.itemName} Boots".Replace(" Ingot", "");
            string description = $"{name} padded with {leather.itemName}";
            float multiplier = metalIngot.multiplier * leather.multiplier;
            int cost = metalIngot.cost + leather.cost;
            Item boots = new Item(name, cost, multiplier: multiplier, description: description);
            boots.craftingIngredients = new List<Item>
            {
                metalIngot, leather
            };
            boots.skillBonuses = metalIngot.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.skillMultipliers = metalIngot.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.tags = new List<string> { "feet", "armor" };
            boots.equipSlots.Add(EquipmentSlots.Enum.Feet);
            boots.SetModifiers();

            return boots;
        }

    }

    public class Leather
    {
        public static Item CreateLeggings(Item leather, Item cloth)
        {
            string name = $"{leather.itemName} Leggings";
            string description = $"{name} lined with {cloth.itemName}";
            float multiplier = cloth.multiplier * leather.multiplier;
            int cost = cloth.cost + leather.cost;
            Item leggings = new Item(name, cost, multiplier: multiplier, description: description);
            leggings.craftingIngredients = new List<Item>
            {
                leather, cloth
            };
            leggings.skillBonuses = cloth.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leggings.skillMultipliers = cloth.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leggings.tags = new List<string> { "legs", "armor" };
            leggings.equipSlots.Add(EquipmentSlots.Enum.Legs);
            leggings.SetModifiers();

            return leggings;
        }

        public static Item CreateTop(Item leather, Item cloth)
        {
            string name = $"{leather.itemName} Top";
            string description = $"{name} lined with {cloth.itemName}";
            float multiplier = cloth.multiplier * leather.multiplier;
            int cost = cloth.cost + leather.cost;
            Item leatherTop = new Item(name, cost, multiplier: multiplier, description: description);
            leatherTop.craftingIngredients = new List<Item>
            {
                leather, cloth
            };
            leatherTop.skillBonuses = cloth.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leatherTop.skillMultipliers = cloth.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leatherTop.tags = new List<string> { "body", "armor" };
            leatherTop.equipSlots.Add(EquipmentSlots.Enum.Body);
            leatherTop.SetModifiers();

            return leatherTop;
        }

        public static Item CreateCoif(Item leather, Item cloth)
        {
            string name = $"{leather.itemName} Coif";
            string description = $"{name} lined with {cloth.itemName}";
            float multiplier = cloth.multiplier * leather.multiplier;
            int cost = cloth.cost + leather.cost;
            Item coif = new Item(name, cost, multiplier: multiplier, description: description);
            coif.craftingIngredients = new List<Item>
            {
                leather, cloth
            };
            coif.skillBonuses = cloth.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            coif.skillMultipliers = cloth.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            coif.tags = new List<string> { "head", "armor" };
            coif.equipSlots.Add(EquipmentSlots.Enum.Head);
            coif.SetModifiers();

            return coif;
        }

        public static Item CreateVambraces(Item leather, Item cloth)
        {
            string name = $"{leather.itemName} Vambraces";
            string description = $"{name} lined with {cloth.itemName}";
            float multiplier = cloth.multiplier * leather.multiplier;
            int cost = cloth.cost + leather.cost;
            Item vambraces = new Item(name, cost, multiplier: multiplier, description: description);
            vambraces.craftingIngredients = new List<Item>
            {
                leather, cloth
            };
            vambraces.skillBonuses = cloth.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            vambraces.skillMultipliers = cloth.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            vambraces.tags = new List<string> { "hands", "armor" };
            vambraces.equipSlots.Add(EquipmentSlots.Enum.Hands);
            vambraces.SetModifiers();

            return vambraces;
        }

        public static Item CreateBoots(Item leather, Item cloth)
        {
            string name = $"{leather.itemName} Boots";
            string description = $"{name} lined with {cloth.itemName}";
            float multiplier = cloth.multiplier * leather.multiplier;
            int cost = cloth.cost + leather.cost;
            Item boots = new Item(name, cost, multiplier: multiplier, description: description);
            boots.craftingIngredients = new List<Item>
            {
                leather, cloth
            };
            boots.skillBonuses = cloth.skillBonuses.Concat(leather.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.skillMultipliers = cloth.skillMultipliers.Concat(leather.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.tags = new List<string> { "feet", "armor" };
            boots.equipSlots.Add(EquipmentSlots.Enum.Feet);
            boots.SetModifiers();

            return boots;
        }
    }

    public class Cloth
    {
        public static Item CreatePants(Item cloth, Item thread)
        {
            string name = $"{cloth.itemName} Pants".Replace(" Cloth", "");
            string description = $"{name} woven with {thread.itemName}";
            float multiplier = thread.multiplier * cloth.multiplier;
            int cost = thread.cost + cloth.cost;
            Item leggings = new Item(name, cost, multiplier: multiplier, description: description);
            leggings.craftingIngredients = new List<Item>
            {
                cloth, thread
            };
            leggings.skillBonuses = thread.skillBonuses.Concat(cloth.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leggings.skillMultipliers = thread.skillMultipliers.Concat(cloth.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leggings.tags = new List<string> { "legs", "armor" };
            leggings.equipSlots.Add(EquipmentSlots.Enum.Legs);
            leggings.SetModifiers();

            return leggings;
        }

        public static Item CreateRobe(Item cloth, Item thread)
        {
            string name = $"{cloth.itemName} Robe".Replace(" Cloth", "");
            string description = $"{name} woven with {thread.itemName}";
            float multiplier = thread.multiplier * cloth.multiplier;
            int cost = thread.cost + cloth.cost;
            Item leatherTop = new Item(name, cost, multiplier: multiplier, description: description);
            leatherTop.craftingIngredients = new List<Item>
            {
                cloth, thread
            };
            leatherTop.skillBonuses = thread.skillBonuses.Concat(cloth.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leatherTop.skillMultipliers = thread.skillMultipliers.Concat(cloth.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            leatherTop.tags = new List<string> { "body", "armor" };
            leatherTop.equipSlots.Add(EquipmentSlots.Enum.Body);
            leatherTop.SetModifiers();

            return leatherTop;
        }

        public static Item CreateHat(Item cloth, Item thread)
        {
            string name = $"{cloth.itemName} Hat".Replace(" Cloth", "");
            string description = $"{name} woven with {thread.itemName}";
            float multiplier = thread.multiplier * cloth.multiplier;
            int cost = thread.cost + cloth.cost;
            Item coif = new Item(name, cost, multiplier: multiplier, description: description);
            coif.craftingIngredients = new List<Item>
            {
                cloth, thread
            };
            coif.skillBonuses = thread.skillBonuses.Concat(cloth.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            coif.skillMultipliers = thread.skillMultipliers.Concat(cloth.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            coif.tags = new List<string> { "head", "armor" };
            coif.equipSlots.Add(EquipmentSlots.Enum.Head);
            coif.SetModifiers();

            return coif;
        }

        public static Item CreateGloves(Item cloth, Item thread)
        {
            string name = $"{cloth.itemName} Gloves".Replace(" Cloth", "");
            string description = $"{name} woven with {thread.itemName}";
            float multiplier = thread.multiplier * cloth.multiplier;
            int cost = thread.cost + cloth.cost;
            Item vambraces = new Item(name, cost, multiplier: multiplier, description: description);
            vambraces.craftingIngredients = new List<Item>
            {
                cloth, thread
            };
            vambraces.skillBonuses = thread.skillBonuses.Concat(cloth.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            vambraces.skillMultipliers = thread.skillMultipliers.Concat(cloth.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            vambraces.tags = new List<string> { "hands", "armor" };
            vambraces.equipSlots.Add(EquipmentSlots.Enum.Hands);
            vambraces.SetModifiers();

            return vambraces;
        }

        public static Item CreateShoes(Item cloth, Item thread)
        {
            string name = $"{cloth.itemName} Shoes".Replace(" Cloth", "");
            string description = $"{name} woven with {thread.itemName}";
            float multiplier = thread.multiplier * cloth.multiplier;
            int cost = thread.cost + cloth.cost;
            Item boots = new Item(name, cost, multiplier: multiplier, description: description);
            boots.craftingIngredients = new List<Item>
            {
                cloth, thread
            };
            boots.skillBonuses = thread.skillBonuses.Concat(cloth.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.skillMultipliers = thread.skillMultipliers.Concat(cloth.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            boots.tags = new List<string> { "feet", "armor" };
            boots.equipSlots.Add(EquipmentSlots.Enum.Feet);
            boots.SetModifiers();

            return boots;
        }
    }

}
