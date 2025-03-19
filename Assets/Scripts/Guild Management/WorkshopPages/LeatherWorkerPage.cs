using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeatherWorkerPage : WorkshopPage
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
            ),
            new CraftingOption(
                "Leather Top",
                new List<(List<string>, string, int)> {
                    (new List<string> { "leather" }, "leather", 8),
                    (new List<string> { "cloth" }, "cloth", 4)
                },
                (materials) => Armor.Leather.CreateTop(materials[0], materials[1])
            ),
            new CraftingOption(
                "Leather Coif",
                new List<(List<string>, string, int)> {
                    (new List<string> { "leather" }, "leather", 6),
                    (new List<string> { "cloth" }, "cloth", 3)
                },
                (materials) => Armor.Leather.CreateCoif(materials[0], materials[1])
            ),
            new CraftingOption(
                "Leather Boots",
                new List<(List<string>, string, int)> {
                    (new List<string> { "leather" }, "leather", 4),
                    (new List<string> { "cloth" }, "cloth", 2)
                },
                (materials) => Armor.Leather.CreateBoots(materials[0], materials[1])
            ),
            new CraftingOption(
                "Leather Vambraces",
                new List<(List<string>, string, int)> {
                    (new List<string> { "leather" }, "leather", 2),
                    (new List<string> { "cloth" }, "cloth", 1)
                },
                (materials) => Armor.Leather.CreateVambraces(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.leatherWorkers;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.LeatherWorking);
    }
}
