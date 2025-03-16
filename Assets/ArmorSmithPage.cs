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
                "Plate Legs",
                new List<(List<string>, string, int)> { 
                    (new List<string> { "smithing", "metal" }, "ingot", 8), 
                    (new List<string> { "leatherworking", "leather" }, "leather", 3) 
                },
                (materials) => Armor.CreatePlatelegs(materials[0], materials[1])
            ) };
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
