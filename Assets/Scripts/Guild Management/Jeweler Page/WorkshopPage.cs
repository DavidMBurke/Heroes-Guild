using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPage : MonoBehaviour
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

    public CraftingPanel craftingPanel;

    public List<ItemInQueue> itemQueue;
    public List<ItemInQueue> completedItems;
    public GameObject itemInQueueList;
    public GameObject itemInQueueListItemPrefab;
    public int workstationsCount = 3;
    public TextMeshProUGUI workstationsAssignedText;


    private void Awake()
    {
        gm = GuildManager.instance;
        craftingPanel = GetComponentInChildren<CraftingPanel>();
        craftingPanel.workshopPage = this;
        SetPopupsInactive();
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
    }

    public void SetPopupsInactive()
    {
        characterListPanel.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
        assignCharacterButton.SetActive(false);
        unassignCharacterButton.SetActive(false);
    }

    public void AssignCrafter()
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
        if (gm.jewelers.Count >= workstationsCount)
        {
            Debug.Log("More workstations needed to assign more crafters.");
            return;
        }
        gm.jewelers.Add(selectedCharacter);
        gm.unassignedEmployees.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
        UpdateWorkStationsAvailabilityText();
    }
    public void UnassignCrafter()
    {
        if (selectedCharacter == null)
        {
            return;
        }
        if (!gm.jewelers.Contains(selectedCharacter))
        {
            Debug.LogWarning("Character not currently assigned as crafter.");
            return;
        }
        gm.unassignedEmployees.Add(selectedCharacter);
        gm.jewelers.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
        UpdateWorkStationsAvailabilityText();
    }

    private void UpdateWorkStationsAvailabilityText()
    {
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
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
            string skillName = NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting);
            string text3 = skillName + ": " + character.nonCombatSkills.skills[skillName].level.ToString();
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
            string skillName = NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting);
            string text3 = skillName + ": " + character.nonCombatSkills.skills[skillName].level.ToString();
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
            craftingPanel.gameObject.SetActive(false);
        }

    }

    public void ToggleCraftPanel()
    {
        craftingPanel.gameObject.SetActive(!craftingPanel.gameObject.activeInHierarchy);
        if (craftingPanel.gameObject.activeInHierarchy)
        {
            characterListPanel.SetActive(false);
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
