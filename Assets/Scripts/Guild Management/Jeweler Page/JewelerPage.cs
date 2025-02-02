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
    public List<Item> itemsQueue;
    public GameObject itemListItemPrefab;
    public GameObject itemList;

    public ItemSlot itemSlot1;
    public ItemSlot itemSlot2;

    public GameObject materialList;
    public GameObject craftPanel;
    public List<ItemInQueue> jewelryQueue;
    public Button addToQueueButton;
    public TextMeshProUGUI craftingMenuHeader;


    private void Start()
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
            string text3 = "JewelryCrafting: " + character.nonCombatSkills.jewelryCrafting.ToString();
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
            string text3 = "JewelryCrafting: " + character.nonCombatSkills.jewelryCrafting.ToString();
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
        }
    }

    public void ToggleCraftPanel()
    {
        craftPanel.SetActive(!craftPanel.activeInHierarchy);
    }

    public void SelectItemSlot(List<string> tags, ItemSlot itemSlot)
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
    }

    public void UpdateItemList(List<Item> items, ItemSlot itemSlot)
    {
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
        Item necklace = (Jewelry.CreateNecklace(itemSlot1.item, itemSlot2.item));
        GameObject necklaceObject = new GameObject();
        ItemInQueue necklaceInQueue = necklaceObject.AddComponent<ItemInQueue>();
        necklaceInQueue.item = necklace;
        jewelryQueue.Add(necklaceInQueue);
    }
}
