using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;

public class ItemInQueue : MonoBehaviour
{
    public Item item;
    public int workDone;
    public int workToComplete;
    public PlayerCharacter? assignedCrafter;
    public List<PlayerCharacter> crafterList;

    public void SetNewItem(Item item, List<PlayerCharacter> crafterList, PlayerCharacter? assignedCrafter = null)
    {
        this.item = item;
        this.assignedCrafter = assignedCrafter;
        this.crafterList = crafterList;
        workDone = 0;
        workToComplete = SetCraftingDifficulty(item);
    }

    private int SetCraftingDifficulty(Item item)
    {
        float workToComplete = 100f;
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
