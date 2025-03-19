using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FletcherPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Shortbow",
                new List<(List<string>, string, int)> {
                    (new List<string> { "wood" }, "wood", 3),
                    (new List<string> { "bowstring" }, "bowstring", 2)
                },
                (materials) => Weapons.Ranged.CreateShortbow(materials[0], materials[1])
            ),
            new CraftingOption(
                "Longbow",
                new List<(List<string>, string, int)> {
                    (new List<string> { "wood" }, "wood", 5),
                    (new List<string> { "bowstring" }, "bowstring", 3)
                },
                (materials) => Weapons.Ranged.CreateLongbow(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.fletchers;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.Fletching);
    }
}
