using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    List<InventoryGridItem> inventoryGridItems = new();
    public PlayerCharacter character;
    public Button playerInventoryButton;
    public Button guildInventoryButton;
    public GameObject inventoryGridItemPrefab;
    public Transform inventoryGrid;
    public List<EquipmentSlotUIElement> equipmentSlotUIElements;
    public static CharacterInfoPanel instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        equipmentSlotUIElements = GetComponentsInChildren<EquipmentSlotUIElement>().ToList();
        playerInventoryButton.onClick.AddListener(() => playerInventoryButtonClickHandler());
        guildInventoryButton.onClick.AddListener(() => guildInventoryButtonClickHandler());
        Close();
    }

    public void SetCharacter(PlayerCharacter character)
    {
        this.character = character;
        MapEquipmentSlots();
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
        SetNewListItems(character.inventory, InventorySource.Player);
    }

    public void guildInventoryButtonClickHandler()
    {
        ResetListItems();
        SetNewListItems(GuildManager.instance.stockpile, InventorySource.Guild);
    }

    void ResetListItems()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }
        inventoryGridItems.Clear();
    }

    void SetNewListItems(List<Item> itemList, InventorySource source)
    {
        ResetListItems();
        foreach (Item item in itemList)
        {
            InventoryGridItem gridItem = Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItem>();
            inventoryGridItems.Add(gridItem);
            gridItem.SetItem(item);
        }
    }

    void MapEquipmentSlots()
    {
        foreach (EquipmentSlotUIElement slotUI in equipmentSlotUIElements)
        {
            var slot = character.equipmentSlots.equipmentSlots[slotUI.equipmentSlotEnum];
            slotUI.UpdateSlotItem(slot.item);
        }
    }
}
