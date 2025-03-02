using System;
using System.Collections.Generic;

public class CraftingOption
{
    public string itemName;
    public List<(List<string> tags, string tagName)> requiredMaterialTags;
    public int requiredMaterialCount;
    public Func<List<Item>, Item> craftingFunction;

    public CraftingOption(string name, List<(List<string>, string)> tags, int count, Func<List<Item>, Item> function)
    {
        itemName = name;
        requiredMaterialTags = tags;
        requiredMaterialCount = count;
        craftingFunction = function;
    }
}
