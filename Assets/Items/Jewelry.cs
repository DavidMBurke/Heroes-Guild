using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jewelry
{
    public static List<Item> Gems = new List<Item>
    {
        new Item("Red Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Orange Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Yellow Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Green Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Blue Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Purple Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("Black Fluorite", 10, tags: new List<string> { "Gem" }),
        new Item("White Fluorite", 10, tags: new List<string> { "Gem" }),

        new Item("Red Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Orange Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Yellow Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Green Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Blue Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Purple Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("Black Garnet", 50, tags: new List<string> { "Gem" }),
        new Item("White Garnet", 50, tags: new List<string> { "Gem" }),

        new Item("Red Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Orange Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Yellow Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Green Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Blue Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Purple Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("Black Topaz", 250, tags: new List<string> { "Gem" }),
        new Item("White Topaz", 250, tags: new List<string> { "Gem" }),

        new Item("Red Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Orange Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Yellow Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Green Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Blue Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Purple Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("Black Zircon", 1250, tags: new List<string> { "Gem" }),
        new Item("White Zircon", 1250, tags: new List<string> { "Gem" }),

        new Item("Red Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Orange Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Yellow Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Green Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Blue Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Purple Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("Black Spinel", 6000, tags: new List<string> { "Gem" }),
        new Item("White Spinel", 6000, tags: new List<string> { "Gem" }),

        new Item("Red Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Orange Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Yellow Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Green Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Blue Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Purple Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("Black Tourmaline", 30000, tags: new List<string> { "Gem" }),
        new Item("White Tourmaline", 30000, tags: new List<string> { "Gem" }),

        new Item("Red Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Orange Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Yellow Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Green Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Blue Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Purple Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("Black Opal", 150000, tags: new List<string> { "Gem" }),
        new Item("White Opal", 150000, tags: new List<string> { "Gem" }),

        new Item("Red Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Orange Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Yellow Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Green Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Blue Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Purple Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("Black Sapphire", 750000, tags: new List<string> { "Gem" }),
        new Item("White Sapphire", 750000, tags: new List<string> { "Gem" }),

        new Item("Red Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Orange Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Yellow Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Green Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Blue Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Purple Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("Black Diamond", 5000000, tags: new List<string> { "Gem" }),
        new Item("White Diamond", 5000000, tags: new List<string> { "Gem" }),

    };

    public Item CreateNecklace(Item metal, Item gem)
    {
        string name = $"{metal.itemName} {gem.itemName} Necklace";
        float multiplier = metal.multiplier;
        int cost = metal.cost;

        Item necklace = new Item(name, cost, multiplier);

        return necklace;
    }
}
