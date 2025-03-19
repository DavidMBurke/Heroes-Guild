using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailorPage : WorkshopPage
{
    public override List<CraftingOption> GetCraftingOptions()
    {
        return new List<CraftingOption>
        {
            new CraftingOption(
                "Cloth Pants",
                new List<(List<string>, string, int)> {
                    (new List<string> { "cloth" }, "cloth", 8),
                    (new List<string> { "thread" }, "thread", 12)
                },
                (materials) => Armor.Cloth.CreatePants(materials[0], materials[1])
            ),
            new CraftingOption(
                "Cloth Robe",
                new List<(List<string>, string, int)> {
                    (new List<string> { "cloth" }, "cloth", 8),
                    (new List<string> { "thread" }, "thread", 12)
                },
                (materials) => Armor.Cloth.CreateRobe(materials[0], materials[1])
            ),
            new CraftingOption(
                "Cloth Hat",
                new List<(List<string>, string, int)> {
                    (new List<string> { "cloth" }, "cloth", 6),
                    (new List<string> { "thread" }, "thread", 9)
                },
                (materials) => Armor.Cloth.CreateHat(materials[0], materials[1])
            ),
            new CraftingOption(
                "Cloth Shoes",
                new List<(List<string>, string, int)> {
                    (new List<string> { "cloth" }, "cloth", 4),
                    (new List<string> { "thread" }, "thread", 6)
                },
                (materials) => Armor.Cloth.CreateShoes(materials[0], materials[1])
            ),
            new CraftingOption(
                "Cloth Gloves",
                new List<(List<string>, string, int)> {
                    (new List<string> { "cloth" }, "cloth", 2),
                    (new List<string> { "thread" }, "thread", 3)
                },
                (materials) => Armor.Cloth.CreateGloves(materials[0], materials[1])
            ),
            new CraftingOption(
                "Thread",
                new List<(List<string>, string, int)>
                {
                    (new List<string> {"plant fiber"}, "plant fiber", 2)
                },
                (materials) => Fabrics.CreateThread(materials[0])
            ),
            new CraftingOption(
                "Imbued Thread",
                new List<(List<string>, string, int)> {
                    (new List<string> { "basic thread" }, "thread", 5),
                    (new List<string> { "fine metal" }, "ingot", 1)
                },
                (materials) => Fabrics.CreateImbuedThread(materials[0], materials[1])
            ),
        };
    }

    public override List<PlayerCharacter> GetCrafters()
    {
        return GuildManager.instance.tailors;
    }

    public override string GetCraftingSkillName()
    {
        return NonCombatSkills.GetName(NonCombatSkills.Enum.Tailoring);
    }
}
