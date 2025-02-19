using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    PlayerCharacter character;
    List<InventoryGridItem> inventoryGridItems = new();
    public Button playerInventoryButton;
    public Button guildInventoryButton;
    public GameObject inventoryGridItemPrefab;
    public Transform inventoryGrid;



    private void Start()
    {
        playerInventoryButton.onClick.AddListener(() => playerInventoryButtonClickHandler());
        guildInventoryButton.onClick.AddListener(() => guildInventoryButtonClickHandler());
        Close();

    }

    public void SetCharacter(PlayerCharacter character)
    {
        this.character = character;
        playerInventoryButtonClickHandler();
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void playerInventoryButtonClickHandler()
    {
        Debug.Log("playerInventoryButtonClickHandler()");
        ResetListItems();
        SetNewListItems(character.inventory);
    }

    public void guildInventoryButtonClickHandler()
    {
        Debug.Log("guildInventoryButtonClickHandler()");
        ResetListItems();
        SetNewListItems(GuildManager.instance.stockpile);
    }

    void ResetListItems()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }
        inventoryGridItems.Clear();
    }

    void SetNewListItems(List<Item> itemList)
    {
        ResetListItems();
        while (itemList.Count > inventoryGridItems.Count || 18 > inventoryGridItems.Count)
        {
            for (int i = 0; i < 6; i++)
            {
                inventoryGridItems.Add(Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItem>());
            }
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            if (i < itemList.Count)
            {
                inventoryGridItems[i].SetItem(itemList[i]);
            }
            else
            {
                inventoryGridItems[i].SetItem(null);
            }
        }
    }
}
