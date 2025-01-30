using System.Collections.Generic;

public class Metals
{
    public static List<Item> MetalOres = new List<Item>
    {
        new Item("Copper Ore", 5, tags: new List<string> {"smithing", "ore"}),
        new Item("Brass Ore", 25, tags: new List<string> {"smithing", "ore"}),
        new Item("Iron Ore", 50, tags: new List<string> {"smithing", "ore"}),
        new Item("Silver Ore", 250, tags: new List<string> {"smithing", "ore"}),
        new Item("Gold Ore", 2500, tags: new List<string> {"smithing", "ore"}),
        new Item("Mythril Ore", 5000, tags: new List<string> {"smithing", "ore"}),
        new Item("Platinum Ore", 25000, tags: new List<string> {"smithing", "ore"}),
        new Item("Adamantium Ore", 50000, tags: new List<string> {"smithing", "ore"}),
        new Item("Osmium Ore", 250000, tags: new List<string> {"smithing", "ore"}),
        new Item("Meteorum Ore", 500000, tags: new List<string> {"smithing", "ore"})
    };

    public enum MetalOreEnum
    {
        CopperOre = 0,
        BrassOre = 1,
        IronOre = 2,
        SilverOre = 3,
        GoldOre = 4,
        MythrilOre = 5,
        PlatinumOre = 6,
        AdamantiumOre = 7,
        OsmiumOre = 8,
        MeteorumOre = 9
    }

    public static List<Item> MetalIngots = new List<Item>
    {
        new Item("Copper Ingot", 10, multiplier: 1f, tags: new List<string> {"smithing", "metal"}),
        new Item("Brass Ingot", 50, multiplier: 1f, tags: new List<string> {"jewelry", "metal"}),
        new Item("Iron Ingot", 100, multiplier: 1.5f, tags: new List<string> {"smithing", "metal"}),
        new Item("Silver Ingot", 500, multiplier: 1.5f, tags: new List<string> {"jewelry", "metal"}),
        new Item("Steel Ingot", 1000, multiplier: 2.25f, tags: new List<string> {"smithing", "metal"}),
        new Item("Gold Ingot", 5000, multiplier: 2.25f, tags: new List<string> {"jewelry", "metal"}),
        new Item("Mythril Ingot", 10000, multiplier: 3.375f, tags: new List<string> {"smithing", "metal"}),
        new Item("Platinum Ingot", 50000, multiplier: 3.75f, tags: new List<string> {"jewelry", "metal"}),
        new Item("Adamantium Ingot", 100000, multiplier: 5f, tags: new List<string> {"smithing", "metal"}),
        new Item("Osmium Ingot", 500000, multiplier: 5f, tags: new List<string> {"jewelry", "metal"}),
        new Item("Meteorum Ingot", 1000000, multiplier: 7.5f, tags: new List<string> {"smithing", "metal"})
    };

    public enum MetalIngotEnum
    {
        CopperIngot = 0,
        BrassIngot = 1,
        IronIngot = 2,
        SilverIngot = 3,
        SteelIngot = 4,
        GoldIngot = 5,
        MythrilIngot = 6,
        PlatinumIngot = 7,
        AdamantiumIngot = 8,
        OsmiumIngot = 9,
        MetorumIngot = 10
    }
}
