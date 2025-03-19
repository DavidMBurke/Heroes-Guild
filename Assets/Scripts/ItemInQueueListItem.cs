using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemInQueueListItem : MonoBehaviour
{
    public TMP_Text itemName;
    public TMP_Text itemPercentComplete;
    public ItemInQueue itemInQueue;
    public TMP_Dropdown crafterDropdown;
    public List<PlayerCharacter> crafterList;
    public Button collectButton;
    public float percentComplete = 0f;

    public const string NEXT_AVAILABLE_TEXT = "Next Available";

    private void Awake()
    {
        collectButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateDropdown();
    }

    public void SetItem(ItemInQueue itemInQueue)
    {
        this.itemInQueue = itemInQueue;
        itemName.text = itemInQueue.item.itemName;
        crafterList = itemInQueue.crafterList;
        UpdatePercentComplete();
        UpdateDropdown();
    }

    public void UpdatePercentComplete()
    {
        percentComplete = 100f * itemInQueue.workDone / itemInQueue.workToComplete;
        itemPercentComplete.text = $"{(int)percentComplete} ({itemInQueue.workDone}/{itemInQueue.workToComplete})";
    }

    public void UpdateDropdown()
    {
        if (!crafterDropdown.gameObject.activeInHierarchy)
        {
            return;
        }
        crafterDropdown.ClearOptions();
        List<string> characters = crafterList.Select(x => x.characterName).ToList();
        crafterDropdown.AddOptions(new List<string> {NEXT_AVAILABLE_TEXT});
        crafterDropdown.AddOptions(characters);
        if (itemInQueue.assignedCrafter != null)
        {
            int index = characters.IndexOf(itemInQueue.assignedCrafter.characterName);
            if (index != -1)
            {
                crafterDropdown.value = index + 1;
            }
        }
        if (percentComplete >= 100)
        {
            crafterDropdown.gameObject.SetActive(false);
            collectButton.gameObject.SetActive(true);
        }
    }

    public void CrafterSelectionHandler()
    {
        string name = crafterDropdown.options[crafterDropdown.value].text;
        if (name == NEXT_AVAILABLE_TEXT)
        {
            itemInQueue.assignedCrafter = null;
            return;
        }
        PlayerCharacter character = crafterList.FirstOrDefault(c => c.characterName == name);
        if (character == null)
        {
            Debug.LogError("Selected character not found in crafter list");
            return;
        }
        itemInQueue.assignedCrafter = character;
    }

    public void CollectItem()
    {
        itemInQueue.item.AddToInventory(GuildManager.instance.stockpile, itemInQueue.item.quantity);
        itemInQueue.queue.Remove(itemInQueue);
        Destroy(gameObject);
    }
}
