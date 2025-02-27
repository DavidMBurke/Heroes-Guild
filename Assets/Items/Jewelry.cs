using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewelry
{
    public static List<Item> Gems = new List<Item>
    {
        new Item("Red Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Orange Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Yellow Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Green Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Blue Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Purple Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("Black Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),
        new Item("White Fluorite", 10, multiplier: 1, tags: new List<string> { "gem" }),

        new Item("Red Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Orange Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Yellow Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Green Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Blue Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Purple Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("Black Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),
        new Item("White Garnet", 50, multiplier: 1.5f, tags: new List<string> { "gem" }),

        new Item("Red Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Orange Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Yellow Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Green Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Blue Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Purple Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("Black Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),
        new Item("White Topaz", 250, multiplier: 2f, tags: new List<string> { "gem" }),

        new Item("Red Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Orange Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Yellow Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Green Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Blue Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Purple Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("Black Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),
        new Item("White Zircon", 1250, multiplier: 2.5f, tags: new List<string> { "gem" }),

        new Item("Red Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Orange Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Yellow Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Green Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Blue Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Purple Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("Black Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),
        new Item("White Spinel", 6000, multiplier: 3f, tags: new List<string> { "gem" }),

        new Item("Red Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Orange Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Yellow Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Green Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Blue Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Purple Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("Black Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),
        new Item("White Tourmaline", 30000, multiplier: 3.5f, tags: new List<string> { "gem" }),

        new Item("Red Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Orange Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Yellow Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Green Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Blue Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Purple Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("Black Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),
        new Item("White Opal", 150000, multiplier: 4f, tags: new List<string> { "gem" }),

        new Item("Red Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Orange Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Yellow Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Green Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Blue Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Purple Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("Black Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),
        new Item("White Sapphire", 750000, multiplier: 4.5f, tags: new List<string> { "gem" }),

        new Item("Red Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Orange Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Yellow Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Green Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Blue Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Purple Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("Black Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),
        new Item("White Diamond", 5000000, multiplier: 5f, tags: new List<string> { "gem" }),

    };

    public static Item CreateNecklace(Item metalIngot, Item gem)
    {
        string name = $"{metalIngot.itemName} {gem.itemName} Necklace".Replace(" Ingot", "");
        float multiplier = metalIngot.multiplier * gem.multiplier;
        int cost = metalIngot.cost + gem.cost;
        Item necklace = new Item(name, cost, multiplier);
        necklace.craftingIngredients = new List<Item>
        {
            metalIngot, gem
        };
        necklace.tags = new List<string> { "necklace" };
        necklace.equipSlots.Add(EquipmentSlots.Enum.Neck);

        return necklace;
    }
}
