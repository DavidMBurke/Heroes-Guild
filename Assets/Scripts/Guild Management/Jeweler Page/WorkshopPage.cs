using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPage : MonoBehaviour
{

    public PlayerCharacter selectedCharacter;
    private GuildManager gm;
    public GameObject itemListItemPrefab;
    public GameObject itemList;

    AssignWorkersPanel assignWorkersPanel;
    CraftingPanel craftingPanel;

    public List<ItemInQueue> itemQueue;
    public List<ItemInQueue> completedItems;
    public GameObject itemInQueueList;
    public GameObject itemInQueueListItemPrefab;
    public int workstationsCount = 3;
    public TextMeshProUGUI workstationsAssignedText;


    private void Awake()
    {
        gm = GuildManager.instance;
        assignWorkersPanel = GetComponentInChildren<AssignWorkersPanel>();
        assignWorkersPanel.workshopPage = this;
        craftingPanel = GetComponentInChildren<CraftingPanel>();
        craftingPanel.workshopPage = this;
        SetPopupsInactive();
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
    }

    public void SetPopupsInactive()
    {
        assignWorkersPanel.gameObject.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
    }


    public void UpdateWorkStationsAvailabilityText()
    {
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
    }

    public void ToggleCharacterSelectionPanel()
    {
        assignWorkersPanel.gameObject.SetActive(!assignWorkersPanel.gameObject.activeInHierarchy);
        assignWorkersPanel.ResetCharacterList();
        if (assignWorkersPanel.gameObject.activeInHierarchy)
        {
            craftingPanel.gameObject.SetActive(false);
        }

    }

    public void ToggleCraftPanel()
    {
        craftingPanel.gameObject.SetActive(!craftingPanel.gameObject.activeInHierarchy);
        if (craftingPanel.gameObject.activeInHierarchy)
        {
            assignWorkersPanel.gameObject.SetActive(false);
        }
    }

    public void UpdateItemList(List<Item> items, CraftingMaterialSlot itemSlot)
    {
        items.RemoveAll(i => i.quantity == 0);
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Item item in items)
        {
            ItemListItem itemListItem = Instantiate(itemListItemPrefab, itemList.transform).GetComponent<ItemListItem>();
            itemListItem.SetItem(item);
            itemListItem.selectButton.onClick.AddListener(() => itemSlot.SetItem(item));
        }
    }

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

    public void UpdateQueueItemValues()
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

    public void Tick()
    {
        foreach (PlayerCharacter jeweler in gm.jewelers)
        {
            #nullable enable
            ItemInQueue? queuedItem = itemQueue.FirstOrDefault(i => i.assignedCrafter == jeweler);
            #nullable disable
            if (queuedItem == null)
            {
                queuedItem = itemQueue.FirstOrDefault(i => i.assignedCrafter == null);
            }
            if (queuedItem == null)
            {
                continue;
            }
            if (queuedItem.assignedCrafter == null)
            {
                queuedItem.assignedCrafter = jeweler;
            }
            Skill crafterSkill = jeweler.nonCombatSkills.skills[NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting)];
            int remainingWork = queuedItem.workToComplete - queuedItem.workDone;
            int workAdded = (crafterSkill.level > remainingWork) ? remainingWork : crafterSkill.level;
            queuedItem.workDone += workAdded;
            crafterSkill.AddXP(workAdded);
            if (queuedItem.workDone >= queuedItem.workToComplete)
            {
                itemQueue.Remove(queuedItem);
                completedItems.Add(queuedItem);
            }
        }
        UpdateQueueItemValues();
    }

    public void ToggleNecklaceCrafting()
    {
        ToggleCraftPanel();
        if (!craftingPanel.gameObject.activeInHierarchy)
        {
            return;
        }
        craftingPanel.addToQueueButton.onClick.RemoveAllListeners();
        craftingPanel.addToQueueButton.onClick.AddListener(() => craftingPanel.AddNecklaceToQueue());
    }

}
