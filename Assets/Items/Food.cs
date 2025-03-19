using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class Food
{
    public class Meat
    {
        public static List<Item> RawMeat = new List<Item>
        {
            new Item("Raw Gigantopillar Meat", 10, multiplier: 1f, tags: new List<string> {"raw meat"}),
            new Item("Raw Bloodfang Meat", 25, multiplier: 1.1f, tags: new List<string> {"raw meat"}),
        };

        public static Item CreateGrilledMeat(Item rawMeat)
        {
            string name = rawMeat.itemName.Replace("Raw ", "Grilled ");
            int cost = (int)(rawMeat.cost * 1.25f);
            Item cookedMeat = new Item(name, cost);
            cookedMeat.craftingIngredients = new List<Item>
            {
                rawMeat
            };
            cookedMeat.multiplier = rawMeat.multiplier;
            cookedMeat.tags = new List<string> { "food" };
            return cookedMeat;
        }
    }
}

