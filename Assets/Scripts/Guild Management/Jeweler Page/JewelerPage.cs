using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JewelerPage : MonoBehaviour
{
    public GameObject characterListPanel;
    public GameObject characterList;
    public GameObject characterListItemPrefab;
    public GameObject assignCharacterButton;
    public GameObject unassignCharacterButton;
    public PlayerCharacter selectedCharacter;
    private GuildManager gm;
    public GameObject itemListItemPrefab;
    public GameObject itemList;

    public CraftingMaterialSlot itemSlot1;
    public CraftingMaterialSlot itemSlot2;
    public CraftingMaterialSlot selectedSlot;

    public GameObject materialList;
    public GameObject craftPanel;
    public Button addToQueueButton;
    public TextMeshProUGUI craftingMenuHeader;

    public List<ItemInQueue> itemQueue;
    public List<ItemInQueue> completedItems;
    public GameObject itemInQueueList;
    public GameObject itemInQueueListItemPrefab;


    private void Awake()
    {
        gm = GuildManager.instance;
        SetPopupsInactive();
    }

    public void SetPopupsInactive()
    {
        characterListPanel.SetActive(false);
        craftPanel.SetActive(false);
        assignCharacterButton.SetActive(false);
        unassignCharacterButton.SetActive(false);
    }

    public void AssignJeweler()
    {
        if (selectedCharacter == null)
        {
            return;
        }
        if (!gm.unassignedEmployees.Contains(selectedCharacter))
        {
            Debug.LogWarning("Character not in unassignedEmployees");
            return;
        }
        gm.jewelers.Add(selectedCharacter);
        gm.unassignedEmployees.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
    }
    public void UnassignJeweler()
    {
        if (selectedCharacter == null)
        {
            return;
        }
        if (!gm.jewelers.Contains(selectedCharacter))
        {
            Debug.LogWarning("Character not in Jewelers");
            return;
        }
        gm.unassignedEmployees.Add(selectedCharacter);
        gm.jewelers.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
    }

    private void ResetCharacterList()
    {
        gm.unassignedEmployees.RemoveAll(character => character == null);
        foreach (Transform child in characterList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerCharacter character in gm.jewelers)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            JobAssignmentCharacterListItem listItem = characterListItemObject.GetComponent<JobAssignmentCharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            string text1 = character.characterName + (" (Assigned)");
            string text2 = "Lvl: " + character.level.ToString();
            string text3 = "Jewelry Crafting: " + character.nonCombatSkills.skills["Jewelry Crafting"].level.ToString();
            listItem.SetText(text1, text2, text3);
        }
        foreach (PlayerCharacter character in gm.unassignedEmployees)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            JobAssignmentCharacterListItem listItem = characterListItemObject.GetComponent<JobAssignmentCharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            string text1 = character.characterName;
            string text2 = "Lvl: " + character.level.ToString();
            string text3 = "JewelryCrafting: " + character.nonCombatSkills.skills["Jewelry Crafting"].level.ToString();
            listItem.SetText(text1, text2, text3);
        }
    }

    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        assignCharacterButton.SetActive(true);
        UpdateButtons();
        ResetCharacterList();
    }

    private void UpdateButtons()
    {
        if (gm.unassignedEmployees.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(true);
            unassignCharacterButton.SetActive(false);
        }
        if (gm.jewelers.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(true);
        }
        if (selectedCharacter == null)
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(false);
        }
    }

    public void ToggleCharacterSelectionPanel()
    {
        characterListPanel.SetActive(!characterListPanel.activeInHierarchy);
        if (characterListPanel.activeInHierarchy)
        {
            ResetCharacterList();
            craftPanel.SetActive(false);
        }

    }

    public void ToggleCraftPanel()
    {
        craftPanel.SetActive(!craftPanel.activeInHierarchy);
        if (craftPanel.activeInHierarchy)
        {
            characterListPanel.SetActive(false);
        }
    }

    public void SelectItemSlot(List<string> tags, CraftingMaterialSlot itemSlot)
    {
        List<Item> items = new List<Item>();

        foreach (Item item in gm.stockpile)
        {
            if (item.tags.All((t) => tags.Contains(t)))
            {
                items.Add(item);
            };
        }

        UpdateItemList(items, itemSlot);
        selectedSlot = itemSlot;
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

    public void SelectIngotSlot()
    {
        List<string> tags = new List<string>()
        {
            "metal",
            "jewelry"
        };
        SelectItemSlot(tags, itemSlot1);
    }

    public void SelectGemSlot()
    {
        List<string> tags = new List<string>()
        {
            "gem"
        };
        SelectItemSlot(tags, itemSlot2);
    }

    public void ToggleNecklaceCrafting()
    {
        ToggleCraftPanel();
        if (!craftPanel.gameObject.activeInHierarchy)
        {
            return;
        }
        addToQueueButton.onClick.RemoveAllListeners();
        addToQueueButton.onClick.AddListener(() => AddNecklaceToQueue());
    }

    public void AddNecklaceToQueue()
    {
        if (itemSlot1.item == null || itemSlot2.item == null) {
            return;
        }
        List<string> tags1 = new List<string> { "metal", "jewelry" };
        List<string> tags2 = new List<string> { "gem" };
        if (itemSlot1.CheckCorrectItemInSlot(tags1) == false || itemSlot2.CheckCorrectItemInSlot(tags2) == false)
        {
            Debug.Log("Correct Items not in slots");
            return;
        }
        if (itemSlot1.CheckCorrectItemQuantityInSlot(1) == false || itemSlot2.CheckCorrectItemQuantityInSlot(1) == false)
        {
            Debug.Log("Insufficient Quantities in slots");
            return;
        }
        itemSlot1.item.quantity -= 1;
        itemSlot2.item.quantity -= 1;
        Item necklace = Jewelry.CreateNecklace(itemSlot1.item, itemSlot2.item);
        GameObject necklaceObject = new GameObject();
        ItemInQueue necklaceInQueue = necklaceObject.AddComponent<ItemInQueue>();
        necklaceInQueue.SetNewItem(necklace, gm.jewelers);
        necklaceObject.name = necklace.itemName;
        itemQueue.Add(necklaceInQueue);
        UpdateQueueList();
        SelectItemSlot(selectedSlot.item.tags, selectedSlot);
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
            ItemInQueue? queuedItem = itemQueue.FirstOrDefault(i => i.assignedCrafter == jeweler);
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
            queuedItem.workDone += jeweler.nonCombatSkills.skills["Jewelry Crafting"].level;
            if (queuedItem.workDone >= queuedItem.workToComplete)
            {
                queuedItem.workDone = queuedItem.workToComplete;
                itemQueue.Remove(queuedItem);
                completedItems.Add(queuedItem);
            }
        }
        UpdateQueueItemValues();
    }

}
