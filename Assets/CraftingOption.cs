using System;
using System.Collections.Generic;

public class CraftingOption
{
    public string itemName;
    public List<(List<string> tags, string itemTypeName, int quantity)> requiredMaterials;
    public Func<List<Item>, Item> craftingFunction;

    public CraftingOption(string name, List<(List<string>, string, int)> materials, Func<List<Item>, Item> function)
    {
        itemName = name;
        requiredMaterials = materials;
        craftingFunction = function;
    }
}
