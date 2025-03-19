using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorSmithPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Platelegs",
                new List<(List<string>, string, int)> { 
                    (new List<string> { "smithing metal" }, "ingot", 8), 
                    (new List<string> { "leather" }, "leather", 4) 
                },
                (materials) => Armor.Plate.CreatePlatelegs(materials[0], materials[1])
            ),
            new CraftingOption(
                "Platebody",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 6),
                    (new List<string> { "leather" }, "leather", 3)
                },
                (materials) => Armor.Plate.CreatePlatebody(materials[0], materials[1])
            ),
            new CraftingOption(
                "Helmet",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 4),
                    (new List<string> { "leather" }, "leather", 2)
                },
                (materials) => Armor.Plate.CreateHelmet(materials[0], materials[1])
            ),
            new CraftingOption(
                "Plated Boots",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 3),
                    (new List<string> { "leather" }, "leather", 3)
                },
                (materials) => Armor.Plate.CreateBoots(materials[0], materials[1])
            ),
            new CraftingOption(
                "Gauntlets",
                new List<(List<string>, string, int)> {
                    (new List<string> { "smithing metal" }, "ingot", 2),
                    (new List<string> { "leather" }, "leather", 2)
                },
                (materials) => Armor.Plate.CreateGauntlets(materials[0], materials[1])
            )
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.armorSmiths;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.ArmorSmithing);
    }
}
