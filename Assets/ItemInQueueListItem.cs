using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInQueueListItem : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemPercentComplete;
    public ItemInQueue itemInQueue;
    public TMP_Dropdown crafterDropdown;
    public List<PlayerCharacter> crafterList;
    public PlayerCharacter assignedCrafter;

    const string NEXT_AVAILABLE_TEXT = "Next Available"; //just in case it changes later

    private void Update()
    {
        UpdateDropdown();
    }

    public void SetItem(ItemInQueue itemInQueue)
    {
        this.itemInQueue = itemInQueue;
        itemName.text = itemInQueue.item.itemName;
        crafterList = itemInQueue.crafterList;
        assignedCrafter = itemInQueue.assignedCrafter;
        UpdatePercentComplete();
        UpdateDropdown();
    }

    public void UpdatePercentComplete()
    {
        float percentComplete = 100f * itemInQueue.workDone / itemInQueue.workToComplete;
        itemPercentComplete.text = $"{(int)percentComplete} ({itemInQueue.workDone}/{itemInQueue.workToComplete})";
    }

    public void UpdateDropdown()
    {
        crafterDropdown.ClearOptions();
        List<string> characters = crafterList.Select(x => x.characterName).ToList();
        crafterDropdown.AddOptions(new List<string> {NEXT_AVAILABLE_TEXT});
        crafterDropdown.AddOptions(characters);
        if (assignedCrafter != null)
        {
            int index = characters.IndexOf(assignedCrafter.characterName);
            if (index != -1)
            {
                crafterDropdown.value = index + 1;
            }
        }
    }

    public void CrafterSelectionHandler()
    {
        string name = crafterDropdown.options[crafterDropdown.value].text;
        if (name == NEXT_AVAILABLE_TEXT)
        {
            assignedCrafter = null;
            return;
        }
        PlayerCharacter character = crafterList.FirstOrDefault(c => c.characterName == name);
        if (character == null)
        {
            Debug.LogError("Selected character not found in crafter list");
            return;
        }
        assignedCrafter = character;
    }
}
