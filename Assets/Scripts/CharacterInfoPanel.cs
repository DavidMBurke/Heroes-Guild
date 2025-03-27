using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    List<InventoryGridItemUIElement> inventoryGridItems = new();
    public PlayerCharacter character = null!;
    public Button playerInventoryButton = null!;
    public Button guildInventoryButton = null!;
    public GameObject inventoryGridItemPrefab = null!;
    public Transform inventoryGrid = null!;
    public List<EquipmentSlotUIElement> equipmentSlotUIElements = null!;
    public static CharacterInfoPanel instance = null!;
    public InventorySource currentSource;
    EquipmentSlots.Enum? activeFilter = null;

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
        currentSource = InventorySource.Player;
        UpdateInventoryList();
    }

    public void guildInventoryButtonClickHandler()
    {
        currentSource = InventorySource.Guild;
        UpdateInventoryList();
    }

    public void equipmentSlotClickHandler(EquipmentSlots.Enum slotEnum)
    {
        activeFilter = slotEnum;
        UpdateInventoryList();
    }


    void ResetListItems()
    {
        foreach (Transform child in inventoryGrid.transform)
        {
            Destroy(child.gameObject);
        }
        inventoryGridItems.Clear();
    }

    void UpdateInventoryList()
    {
        List<Item> items = new();
        if (currentSource == InventorySource.Player)
        {
            items = character.inventory;
        } 
        else
        {
            items = GuildManager.instance.stockpile;
        }

        if (activeFilter.HasValue)
        {
            items = items.Where(i => i.equipSlots.Any(s => s == activeFilter.Value)).ToList();
        }

        SetNewListItems(items, currentSource);
    }

    void SetNewListItems(List<Item> itemList, InventorySource source)
    {
        ResetListItems();
        foreach (Item item in itemList)
        {
            InventoryGridItemUIElement gridItem = Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItemUIElement>();
            inventoryGridItems.Add(gridItem);
            gridItem.SetItem(item, source);
        }
        int slotsToFill = 6 - inventoryGridItems.Count() % 6;
        for (int i = 0; i < slotsToFill + 12; i++)
        {
            InventoryGridItemUIElement gridItem = Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItemUIElement>();
            inventoryGridItems.Add(gridItem);
            gridItem.SetItem(null);
        }
    }

    void MapEquipmentSlots()
    {
        foreach (EquipmentSlotUIElement slotUI in equipmentSlotUIElements)
        {
            var slot = character.equipmentSlots.equipmentSlots[slotUI.equipmentSlotEnum];
            slotUI.UpdateSlotItem(slot.item);
            Button slotButton = slotUI.gameObject.GetComponentInChildren<Button>();
            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => { equipmentSlotClickHandler(slotUI.equipmentSlotEnum); });
        }
    }
}
