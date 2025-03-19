using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Weapons
{
    public class Melee
    {
        public static Item CreateDagger(Item gripMaterial, Item metalIngot)
        {
            string name = $"{metalIngot.itemName} Dagger".Replace(" Ingot", "");
            string description = $"{name} with {gripMaterial.itemName} Grip".Replace(" Ingot", "");
            float multiplier = metalIngot.multiplier * gripMaterial.multiplier;
            int cost = metalIngot.cost + gripMaterial.cost;
            Item dagger = new Item(name, cost, multiplier, description: description);
            dagger.craftingIngredients = new List<Item>
                {
                    metalIngot, gripMaterial
                };
            dagger.multiplier = multiplier;
            dagger.skillBonuses = metalIngot.skillBonuses.Concat(gripMaterial.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            dagger.skillMultipliers = metalIngot.skillMultipliers.Concat(gripMaterial.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            dagger.tags = new List<string> { "melee", "weapon", "dagger" };
            dagger.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            dagger.SetModifiers();

            return dagger;
        }
        public static Item CreateShortsword(Item gripMaterial, Item metalIngot)
        {
            string name = $"{metalIngot.itemName} Shortsword".Replace(" Ingot", "");
            string description = $"{name} with {gripMaterial.itemName} Grip".Replace(" Ingot", "");
            float multiplier = metalIngot.multiplier * gripMaterial.multiplier;
            int cost = metalIngot.cost + gripMaterial.cost;
            Item dagger = new Item(name, cost, multiplier, description: description);
            dagger.craftingIngredients = new List<Item>
                {
                    metalIngot, gripMaterial
                };
            dagger.multiplier = multiplier;
            dagger.skillBonuses = metalIngot.skillBonuses.Concat(gripMaterial.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            dagger.skillMultipliers = metalIngot.skillMultipliers.Concat(gripMaterial.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            dagger.tags = new List<string> { "melee", "weapon", "sword" };
            dagger.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            dagger.SetModifiers();

            return dagger;
        }
    }
    public class Ranged
    {
        public static Item CreateShortbow(Item wood, Item bowString)
        {
            string name = $"{wood.itemName} Shortbow";
            string description = $"{name} strung with {bowString.itemName}";
            float multiplier = wood.multiplier * bowString.multiplier;
            int cost = wood.cost + bowString.cost;
            Item bow = new Item(name, cost, multiplier, description: description);
            bow.craftingIngredients = new List<Item>
                {
                    wood, bowString
                };
            bow.multiplier = multiplier;
            bow.skillBonuses = wood.skillBonuses.Concat(bowString.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            bow.skillMultipliers = wood.skillMultipliers.Concat(bowString.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            bow.tags = new List<string> { "ranged", "weapon", "bow" };
            bow.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            bow.SetModifiers();

            return bow;
        }

        public static Item CreateLongbow(Item wood, Item bowString)
        {
            string name = $"{wood.itemName} Shortbow";
            string description = $"{name} strung with {bowString.itemName}";
            float multiplier = wood.multiplier * bowString.multiplier;
            int cost = wood.cost + bowString.cost;
            Item bow = new Item(name, cost, multiplier, description: description);
            bow.craftingIngredients = new List<Item>
                {
                    wood, bowString
                };
            bow.multiplier = multiplier;
            bow.skillBonuses = wood.skillBonuses.Concat(bowString.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            bow.skillMultipliers = wood.skillMultipliers.Concat(bowString.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            bow.tags = new List<string> { "ranged", "weapon", "bow" };
            bow.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            bow.SetModifiers();

            return bow;
        }
    }

    public class Magic
    {
        public static Item CreateStaff(Item wood, Item essence)
        {
            string name = $"{wood.itemName} Staff";
            string description = $"{name} staff bound with {essence.itemName}";
            float multiplier = wood.multiplier * essence.multiplier;
            int cost = wood.cost + essence.cost;
            Item staff = new Item(name, cost, multiplier, description: description);
            staff.craftingIngredients = new List<Item>
                {
                    wood, essence
                };
            staff.multiplier = multiplier;
            staff.skillBonuses = wood.skillBonuses.Concat(essence.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            staff.skillMultipliers = wood.skillMultipliers.Concat(essence.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            staff.tags = new List<string> { "magic", "weapon", "staff" };
            staff.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            staff.SetModifiers();

            return staff;
        }

        public static Item CreateWand(Item wood, Item essence)
        {
            string name = $"{wood.itemName} Wand";
            string description = $"{name} bound with {essence.itemName}";
            float multiplier = wood.multiplier * essence.multiplier;
            int cost = wood.cost + essence.cost;
            Item staff = new Item(name, cost, multiplier, description: description);
            staff.craftingIngredients = new List<Item>
                {
                    wood, essence
                };
            staff.multiplier = multiplier;
            staff.skillBonuses = wood.skillBonuses.Concat(essence.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            staff.skillMultipliers = wood.skillMultipliers.Concat(essence.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            staff.tags = new List<string> { "magic", "weapon", "wand" };
            staff.equipSlots.Add(EquipmentSlots.Enum.RightHand);
            staff.SetModifiers();

            return staff;
        }
    }
}
