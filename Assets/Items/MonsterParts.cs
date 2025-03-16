using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParts
{
    public static List<Item> Hides = new List<Item>
    {
        new Item("Brindlegrazer Hide", 5, tags: new List<string> { "leatherworking", "hide" }),
        new Item("Bloodfang Hide", 50, tags: new List<string> { "leatherworking", "hide" })
    };

    public enum HidesEnum
    {
        BrindlegrazerHide = 0,
        BloodfangHide = 1
    }

    public static List<Item> Leathers = new List<Item>
    {
        new Item("Brindlegrazer Leather", 10, multiplier: 1f, tags: new List<string> {"leatherworking", "leather"}),
        new Item("Bloodfang Leather", 100, multiplier: 1.25f, tags: new List<string> {"leatherworking", "leather"})
    };

    public enum LeathersEnum
    {
        BrindlegrazerLeather = 0,
        BloodfangLeather = 1
    }

    public static List<Item> ProcessedParts = new List<Item>
    {
        new Item("Leather Strips", 5, tags: new List<string> {"leatherworking", "component"})
    };

    public enum ProcessedPartsEnum
    {
        LeatherStrips = 0
    }

    public static List<Item> MiscellaneousParts = new List<Item>
    {
        new Item("Monster Meat Chunk", 2, tags: new List<string> {"component"}),
        new Item("Monster Bone", 2, tags: new List<string> {"component"}),
        new Item("Monster Blood", 2, tags: new List<string> {"component"}),
        new Item("Plant Fiber", 2, tags: new List<string> {"component"}),
        new Item("Mycellium Clump", 2, tags: new List<string> {"component"}),
        new Item("Arthropod Blood", 2, tags: new List<string> {"component"}),
        new Item("Gigantopillar Silk", 2, tags: new List<string> {"component"}),
        new Item("Vorpid Vines", 2, tags: new List<string> {"component"}),
        new Item("Bumbleshroom Cap", 2, tags: new List<string> {"component"}),
        new Item("Bloodfang Hide", 2, tags: new List<string> {"component"}),
        new Item("Bloodfang Saliva Gland", 2, tags: new List<string> {"component"}),
    };

    public enum MiscellaneousPartsEnum
    {
        
    }

    public static List<Item> Corpses = new List<Item>
    {
        new Item("Gigantopillar Corpse", 10, tags: new List<string> {"corpse"}),
        new Item("Vorpid Corpse", 10, tags: new List<string> {"corpse"}),
        new Item("Bumbleshroom Corpse", 10, tags: new List<string> {"corpse"}),
        new Item("Bloodfang Corpse", 25, tags: new List<string> {"corpse"}),
    };

    public enum CorpseEnum
    {
        GigantopillarCorpse = 0,
        VorpidCorpse = 1,
        BumbleshroomCorpse = 2,
        BloodfangCorpse = 3
    }
}
