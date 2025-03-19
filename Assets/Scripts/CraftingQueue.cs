using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingQueue : MonoBehaviour
{
    public List<ItemInQueue> itemQueue = null!;
    public List<ItemInQueue> completedItems = null!;
    public GameObject itemInQueueList = null!;
    public GameObject itemInQueueListItemPrefab = null!;

    public void UpdateQueueList()
    {
        foreach (Transform child in itemInQueueList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemInQueue item in completedItems)
        {
            ItemInQueueListItem itemInQueue = Instantiate(itemInQueueListItemPrefab, itemInQueueList.transform).GetComponent<ItemInQueueListItem>();
            itemInQueue.SetItem(item);
            itemInQueue.crafterDropdown.gameObject.SetActive(false);
        }
        foreach (ItemInQueue item in itemQueue)
        {
            ItemInQueueListItem itemInQueue = Instantiate(itemInQueueListItemPrefab, itemInQueueList.transform).GetComponent<ItemInQueueListItem>();
            itemInQueue.SetItem(item);
        }
    }

    public void UpdateItemValues()
    {
        foreach (Transform child in itemInQueueList.transform)
        {
            ItemInQueueListItem item = child.gameObject.GetComponent<ItemInQueueListItem>();
            if (item != null)
            {
                item.UpdatePercentComplete();
                string dropdownText = item.crafterDropdown.options[item.crafterDropdown.value].text;

                if (item.itemInQueue.assignedCrafter != null && dropdownText != item.itemInQueue.assignedCrafter.characterName)
                {
                    item.UpdateDropdown();
                }
                if (item.itemInQueue.assignedCrafter == null && dropdownText != ItemInQueueListItem.NEXT_AVAILABLE_TEXT)
                {
                    item.UpdateDropdown();
                }
            }
        }
    }

}
