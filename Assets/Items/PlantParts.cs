using System.Collections.Generic;
using UnityEngine.UI;

public class PlantParts
{
    public static List<Item> Woods = new List<Item>
    {
        new Item("Ash Wood", 25, tags: new List<string> {"wood"}),
    };

    public static List<Item> Misc = new List<Item>
    {
        new Item("Plant Fiber", 2, tags: new List<string> {"plant fiber"}),
    };
}

