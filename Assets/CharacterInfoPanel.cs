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
        ResetListItems();
        SetNewListItems(character.inventory);
    }

    public void guildInventoryButtonClickHandler()
    {
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
        for (int i = 0; i < 18; i++)
        {
            inventoryGridItems.Add(Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItem>());
        }
    }

    void SetNewListItems(List<Item> itemList)
    {
        ResetListItems();
        while (character.inventory.Count > inventoryGridItems.Count)
        {
            for (int i = 0; i < 6; i++)
            {
                inventoryGridItems.Add(Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItem>());
            }
        }
        for (int i = 0; i < itemList.Count; i++)
        {
            inventoryGridItems[i].SetItem(itemList[i]);
        }
    }
}
