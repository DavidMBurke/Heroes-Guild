using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooksPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Grilled Meat",
                new List<(List<string>, string, int)> {
                    (new List<string> { "raw meat" }, "raw meat", 1)
                },
                (materials) => Food.Meat.CreateGrilledMeat(materials[0])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.cooks;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.Cooking);
    }
}
