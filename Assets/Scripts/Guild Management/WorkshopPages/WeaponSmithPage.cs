using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSmithPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Dagger",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 2),
                    (new List<string> { "bone" }, "bone", 1)
                },
                (materials) => Weapons.Melee.CreateDagger(materials[0], materials[1])
            ),
            new CraftingOption(
                "Shortsword",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 4),
                    (new List<string> { "bone" }, "bone", 2)
                },
                (materials) => Weapons.Melee.CreateDagger(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.weaponSmiths;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.WeaponSmithing);
    }
}
