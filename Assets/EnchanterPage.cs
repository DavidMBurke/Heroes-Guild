using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchanterPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Leather Leggings",
                new List<(List<string>, string, int)> {
                    (new List<string> { "leather" }, "leather", 8),
                    (new List<string> { "cloth" }, "cloth", 4)
                },
                (materials) => Armor.Leather.CreateLeggings(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.enchanters;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.Enchanting);
    }
}
