using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JewelerPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Necklace",
                new List<(List<string>, string, int)> { 
                    (new List<string> { "metal", "jewelry" }, "ingot", 1), 
                    (new List<string> { "gem" }, "gem", 1)},
                (materials) => Jewelry.CreateNecklace(materials[0], materials[1])
            ),
            new CraftingOption(
                "Ring",
                new List<(List<string>, string, int)> { 
                    (new List<string> { "metal", "jewelry" }, "ingot", 1), 
                    (new List<string> { "gem" }, "gem", 1)},
                (materials) => Jewelry.CreateRing(materials[0], materials[1])
            ),
            new CraftingOption(
                "Bracelet",
                new List<(List<string>, string, int)> { 
                    (new List<string> { "metal", "jewelry" }, "ingot", 1), 
                    (new List<string> { "gem" }, "gem", 1)},
                (materials) => Jewelry.CreateBracelet(materials[0], materials[1])
            )};
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.jewelers;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting);
    }
}
