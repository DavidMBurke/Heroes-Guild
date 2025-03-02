using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelerPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        (List<string> tags, string itemTypeName) ingot = (new List<string> { "metal", "jewelry" }, "ingot");
        (List<string> tags, string itemTypeName) gem = (new List<string> { "gem" }, "gem");
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Necklace",
                new List<(List<string>, string)> {ingot, gem},
                2,
                (materials) => Jewelry.CreateNecklace(materials[0], materials[1])
            ),
            new CraftingOption(
                "Ring",
                new List<(List<string>, string)> {ingot, gem},
                2,
                (materials) => Jewelry.CreateRing(materials[0], materials[1])
            ),
            new CraftingOption(
                "Bracelet",
                new List<(List<string>, string)> {ingot, gem},
                2,
                (materials) => Jewelry.CreateBracelet(materials[0], materials[1])
            )};
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.jewelers;
    }
}
