using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanistPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Staff",
                new List<(List<string>, string, int)> {
                    (new List<string> { "wood" }, "wood", 8),
                    (new List<string> { "essence" }, "essence", 1)
                },
                (materials) => Weapons.Magic.CreateStaff(materials[0], materials[1])
            ),
             new CraftingOption(
                "Wand",
                new List<(List<string>, string, int)> {
                    (new List<string> { "wood" }, "wood", 3),
                    (new List<string> { "essence" }, "essence", 1)
                },
                (materials) => Weapons.Magic.CreateWand(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.arcanists;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.Arcana);
    }
}
