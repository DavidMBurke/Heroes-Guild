using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Fabrics
{
    public static List<Item> Cloths = new List<Item>
    {
        new Item("Basic Cloth", 5, tags: new List<string> {"cloth"}),
        new Item("Gigantopillar Silk Cloth", 25, tags: new List<string> {"cloth"})
    };

    public enum ClothEnum
    {
        BasicCloth = 0,
        GigantopillarCloth = 1
    }

    public static List<Item> BowStrings = new List<Item>
    {
        new Item("Basic Bowstring", 5, tags: new List<string> {"bowstring"}),
        new Item("Gigantopillar Silk Bowstring", 25, tags: new List<string> {"bowstring"}),

    };

    public enum BowstringEnum
    {
        BasicCloth = 0,
        GigantopillarCloth = 1
    }

    public static List<Item> Threads = new List<Item>
    {
        new Item("Basic Thread", 10, multiplier: 1f, tags: new List<string> { "thread", "basic thread" }),
        new Item("Brass-Imbued Thread", 50, multiplier: 1.25f, tags: new List<string> { "thread" }),
        new Item("Silver-Imbued Thread", 500, multiplier: 1.75f, tags: new List<string> { "thread" }),
        new Item("Gold-Imbued Thread", 5000, multiplier: 2.5f, tags: new List<string> { "thread" }),
        new Item("Platinum-Imbued Thread", 50000, multiplier: 3.5f, tags: new List<string> { "thread" }),
        new Item("Osmium-Imbued Thread", 500000, multiplier: 5f, tags: new List<string> { "thread" }),
    };

    public enum ThreadEnum
    {
        BasicThread = 0,
        BrassImbuedThread = 1,
        SilverImbuuedThread = 2,
        GoldImbuedThread = 3,
        PlatinumImbuedThread = 4,
        OsmiumImbuedThread = 5
    }

    public static Item CreateThread(Item plantFiber)
    {
        return new Item("Basic Thread", 10, multiplier: 1f, tags: new List<string> { "thread", "basic thread" });

    }
    public static Item? CreateImbuedThread(Item thread, Item metal)
    {
        string metalName = metal.itemName.Replace(" Ingot", "");
        string threadName = $"{metalName}-Imbued Thread";

        Item existingThread = Threads.FirstOrDefault(t => t.itemName == threadName);
        if (existingThread != null)
        {
            return new Item(threadName, existingThread.cost, multiplier: existingThread.multiplier, tags: new List<string> { "thread" });
        }

        return null;
    }
}

