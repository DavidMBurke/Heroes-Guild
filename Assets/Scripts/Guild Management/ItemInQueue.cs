using System.Collections.Generic;
using UnityEngine;

public class ItemInQueue : MonoBehaviour
{
    public Item item;
    public int workDone;
    public int workToComplete;
    #nullable enable
    public PlayerCharacter? assignedCrafter;
    #nullable disable
    public List<PlayerCharacter> crafterList;

    #nullable enable
    public void SetNewItem(Item item, List<PlayerCharacter> crafterList, PlayerCharacter? assignedCrafter = null)
    {
        this.item = item;
        this.assignedCrafter = assignedCrafter;
        this.crafterList = crafterList;
        workDone = 0;
        workToComplete = SetCraftingDifficulty(item);
    }
    #nullable disable

    private int SetCraftingDifficulty(Item item)
    {
        float workToComplete = 1000f;
        foreach (Item ingredient in item.craftingIngredients)
        {
            workToComplete *= ingredient.multiplier;
        }
        return (int)workToComplete;
    }

    public void SetCrafter(PlayerCharacter character)
    {
        assignedCrafter = character;
    }

}
