using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInQueue : MonoBehaviour
{
    public Item item;
    public float percentComplete;
    public float craftingDifficulty;
    public PlayerCharacter? assignedCrafter;
    public Action actionOnComplete;

    public ItemInQueue(Item item, Action actionOnComplete, PlayerCharacter? assignedCrafter = null)
    {
        this.item = item;
        this.actionOnComplete = actionOnComplete;
        this.assignedCrafter = assignedCrafter;
        percentComplete = 0f;
        craftingDifficulty = SetCraftingDifficulty(item);
    }

    private float SetCraftingDifficulty(Item item)
    {
        float difficulty = 1f;
        foreach (Item ingredient in item.craftingIngredients)
        {
            difficulty *= ingredient.multiplier;
        }
        return difficulty;
    }

}
