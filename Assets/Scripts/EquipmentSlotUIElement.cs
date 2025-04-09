#nullable enable
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentSlotUIElement : MonoBehaviour, IDropHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public EquipmentSlots.Enum equipmentSlotEnum;
    public Item? slotItem;
    public TextMeshProUGUI itemName;
    private CanvasGroup canvasGroup = null!;
    private RectTransform rectTransform = null!;
    private GameObject draggedItemObject = null!;
    private Transform originalParent = null!;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItemUIElement draggedItem))
        {
            Item newItem = draggedItem.GetItem().Clone();
            if (newItem == null || !newItem.equipSlots.Any(es => es == equipmentSlotEnum))
            {
                return;
            }
            newItem.quantity = 1;
            PlayerCharacter character = CharacterInfoPage.instance.character;
            EquipmentSlot slot = character.equipmentSlots.equipmentSlots[equipmentSlotEnum];
            Item? replacedItem = slot.item;
            if (draggedItem.source == InventorySource.Player)
            {
                character.RemoveFromInventory(newItem);
            }
            else if (draggedItem.source == InventorySource.Guild)
            {
                GuildManager.instance.RemoveFromStockpile(newItem);
            }
            else
            {
                Debug.LogError("draggedItem has no source");
            }
            
            if (replacedItem != null)
            {
                if (draggedItem.source == InventorySource.Player)
                {
                    character.AddToInventory(replacedItem);
                }
                if (draggedItem.source == InventorySource.Guild)
                {
                    replacedItem.AddToInventory(GuildManager.instance.stockpile);
                }
            }

            draggedItem.transform.SetParent(null);
            Destroy(draggedItem.gameObject);
            character.equipmentSlots.EquipItem(equipmentSlotEnum, newItem);
            UpdateSlotItem(newItem);
            character.ApplyEquipmentSkillModifiers();

            if (draggedItem.source == InventorySource.Player)
            {
                CharacterInfoPage.instance.playerInventoryButtonClickHandler();
            }
            if (draggedItem.source == InventorySource.Guild)
            {
                CharacterInfoPage.instance.guildInventoryButtonClickHandler();
            }

        }
    }

    public void UpdateSlotItem(Item? item)
    {
        slotItem = item;
        itemName.text = item?.itemName ?? "";
        gameObject.SetActive(true);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotItem == null) return;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        draggedItemObject = new GameObject("DraggedItem");
        draggedItemObject.transform.SetParent(transform.root);
        draggedItemObject.transform.SetAsLastSibling();

        RectTransform draggedRect = draggedItemObject.AddComponent<RectTransform>();
        draggedRect.sizeDelta = rectTransform.sizeDelta;
        draggedRect.anchorMin = new Vector2(0.5f, 0.5f);
        draggedRect.anchorMax = new Vector2(0.5f, 0.5f);
        draggedRect.pivot = new Vector2(0.5f, 0.5f);

        Image draggedImage = draggedItemObject.AddComponent<Image>();
        draggedImage.sprite = GetComponent<Image>().sprite;
        draggedImage.raycastTarget = false;

        draggedItemObject.transform.position = Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItemObject == null) return;
        draggedItemObject.transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (draggedItemObject != null)
        {
            Destroy(draggedItemObject);
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            UpdateSlotItem(slotItem);
        }
    }

}
