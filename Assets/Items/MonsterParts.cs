using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParts
{
    public static List<Item> Hides = new List<Item>
    {
        new Item("Brindlegrazer Hide", 5, tags: new List<string> { "hide" }),
        new Item("Bloodfang Hide", 50, tags: new List<string> { "hide" })
    };

    public enum HidesEnum
    {
        BrindlegrazerHide = 0,
        BloodfangHide = 1
    }

    public static List<Item> Bones = new List<Item>
    {
        new Item("Brindlegrazer Bone", 5, tags: new List<string> { "bone" }),
        new Item("Bloodfang Bone", 50, tags: new List<string> { "bone" })
    };

    public enum BonesEnum
    {
        BrindlegrazerBone = 0,
        BloodfangBone = 1
    }

    public static List<Item> Leathers = new List<Item>
    {
        new Item("Brindlegrazer Leather", 10, multiplier: 1f, tags: new List<string> { "leather" }),
        new Item("Bloodfang Leather", 100, multiplier: 1.25f, tags: new List<string> { "leather"})
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

    public static List<Item> Misc = new List<Item>
    {
        new Item("Mycellium Clump", 2, tags: new List<string> {"component"}),
        new Item("Arthropod Blood", 2, tags: new List<string> {"component"}),
        new Item("Gigantopillar Silk", 2, tags: new List<string> {"component"}),
        new Item("Vorpid Vines", 2, tags: new List<string> {"component"}),
        new Item("Bumbleshroom Cap", 2, tags: new List<string> {"component"}),
        new Item("Bloodfang Saliva Gland", 2, tags: new List<string> {"component"}),

    };

    public enum MiscEnum
    {
        MycelliumClump = 0,
        ArthropodBlood,
        GigantopillarSilk,
        VorpidVines,
        BumbleshroomCap,
        BloodfangSalivaGland
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

    public static List<Item> Essences = new List<Item>
    {
        new Item("Tiny Nature Essence", 10, tags: new List<string> {"essence"}),
        new Item("Tiny Arcana Essence", 10, tags: new List<string> {"essence"}),
        new Item("Tiny Celestial Essence", 10, tags: new List<string> {"essence"}),
        new Item("Tiny Spirit Essence", 10, tags: new List<string> {"essence"}),
        new Item("Tiny Qi Essence", 10, tags: new List<string> {"essence"}),
        new Item("Small Nature Essence", 10, tags: new List<string> {"essence"}),
        new Item("Small Arcana Essence", 10, tags: new List<string> {"essence"}),
        new Item("Small Celestial Essence", 10, tags: new List<string> {"essence"}),
        new Item("Small Spirit Essence", 10, tags: new List<string> {"essence"}),
        new Item("Small Qi Essence", 10, tags: new List<string> {"essence"})
    };

    public enum EssencesEnum
    {
        TinyNatureEssence = 0,
        TinyArcanaEssence = 0,
        TinyCelestialEssence = 0,
        TinySpiritEssence = 0,
        TinyQiEssence = 0,
        SmallNatureEssence = 0,
        SmallArcanaEssence = 0,
        SmallCelestialEssence = 0,
        SmallSpiritEssence = 0,
        SmallQiEssence = 0
    }
}
