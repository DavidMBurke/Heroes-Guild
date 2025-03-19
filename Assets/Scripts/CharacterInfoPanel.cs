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
    InventorySource currentSource;

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
        currentSource = InventorySource.Player;
        SetNewListItems(character.inventory, InventorySource.Player);
    }

    public void guildInventoryButtonClickHandler()
    {
        ResetListItems();
        currentSource = InventorySource.Guild;
        SetNewListItems(GuildManager.instance.stockpile, InventorySource.Guild);
    }

    public void equipmentSlotClickHandler(EquipmentSlots.Enum slotEnum)
    {
        List<Item> items = new List<Item>();
        if (currentSource == InventorySource.Player)
        {
            items = character.inventory;
        }
        if (currentSource == InventorySource.Guild) {
            items = GuildManager.instance.stockpile;
        }
        SetNewListItems(items.Where(i => i.equipSlots.Any(s => s == slotEnum)).ToList(), currentSource);
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
            InventoryGridItemUIElement gridItem = Instantiate(inventoryGridItemPrefab, inventoryGrid).GetComponent<InventoryGridItemUIElement>();
            inventoryGridItems.Add(gridItem);
            gridItem.SetItem(item, source);
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
