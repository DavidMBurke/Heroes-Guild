using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Jewelry
{
    public static List<Item> Gems = new List<Item>
    {
        new Item("Red Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),

        new Item("Red Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Evocation), 1f } }),
        new Item("Orange Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Dodge), 1f } }),
        new Item("Yellow Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Ranged), 1f } }),
        new Item("Green Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Block), 1f } }),
        new Item("Blue Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Melee), 1f } }),
        new Item("Purple Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Auras), 1f } }),
        new Item("Black Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Stealth), 1f } }),
        new Item("White Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }, skillBonuses: new Dictionary<string, float> { { CombatSkills.GetName(CombatSkills.Enum.Healing), 1f } }),
    };

    public static Item CreateNecklace(Item metalIngot, Item gem)
    {
        string name = $"{gem.itemName} Necklace";
        string description = $"{metalIngot.itemName} Necklace with an embedded {gem.itemName}".Replace(" Ingot", "");
        float multiplier = metalIngot.multiplier * gem.multiplier;
        int cost = metalIngot.cost + gem.cost;
        Item necklace = new Item(name, cost, multiplier:multiplier, description:description);
        necklace.craftingIngredients = new List<Item>
        {
            metalIngot, gem
        };
        necklace.skillBonuses = metalIngot.skillBonuses.Concat(gem.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        necklace.skillMultipliers = metalIngot.skillMultipliers.Concat(gem.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        necklace.tags = new List<string> { "necklace" };
        necklace.equipSlots.Add(EquipmentSlots.Enum.Neck);

        necklace.SetModifiers();

        return necklace;
    }

    public static Item CreateRing(Item metalIngot, Item gem)
    {
        string name = $"{metalIngot.itemName} {gem.itemName} Ring".Replace(" Ingot", "");
        string description = $"{metalIngot.itemName} Ring with an embedded {gem.itemName}".Replace(" Ingot", "");
        float multiplier = metalIngot.multiplier * gem.multiplier;
        int cost = metalIngot.cost + gem.cost;
        Item ring = new Item(name, cost, multiplier: multiplier, description: description);
        ring.craftingIngredients = new List<Item>
        {
            metalIngot, gem
        };
        ring.skillBonuses = metalIngot.skillBonuses.Concat(gem.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        ring.skillMultipliers = metalIngot.skillMultipliers.Concat(gem.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        ring.tags = new List<string> { "ring" };
        ring.equipSlots.Add(EquipmentSlots.Enum.Finger1);
        ring.equipSlots.Add(EquipmentSlots.Enum.Finger2);

        ring.SetModifiers();

        return ring;
    }

    public static Item CreateBracelet(Item metalIngot, Item gem)
    {
        string name = $"{metalIngot.itemName} {gem.itemName} Bracelet".Replace(" Ingot", "");
        string description = $"{metalIngot.itemName} Ring with an embedded {gem.itemName}".Replace(" Ingot", "");
        float multiplier = metalIngot.multiplier * gem.multiplier;
        int cost = metalIngot.cost + gem.cost;
        Item bracelet = new Item(name, cost, multiplier: multiplier, description: description);
        bracelet.craftingIngredients = new List<Item>
        {
            metalIngot, gem
        };
        bracelet.skillBonuses = metalIngot.skillBonuses.Concat(gem.skillBonuses).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        bracelet.skillMultipliers = metalIngot.skillMultipliers.Concat(gem.skillMultipliers).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        bracelet.tags = new List<string> { "bracelet" };
        bracelet.equipSlots.Add(EquipmentSlots.Enum.Wrist);

        bracelet.SetModifiers();

        return bracelet;
    }



}
