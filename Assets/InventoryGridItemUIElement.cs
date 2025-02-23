using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridItemUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Transform originalParent;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
    int originalSiblingIndex;
#nullable enable
    Item? item;
    public InventorySource? source;
    public GameObject? copy;
#nullable disable

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        if (transform.parent == transform.root)
        {
            transform.SetParent(originalParent);
            transform.SetSiblingIndex(originalSiblingIndex);
        }
    }

#nullable enable
    public void SetItem(Item? item, InventorySource? sourceInventory = null)
#nullable disable
    {
        this.item = item;
        source = sourceInventory;
        itemName.text = item?.itemName ?? "";
        itemQuantity.text = item?.quantity.ToString() ?? "";
    }

    public Item GetItem() => item;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent(out InventoryGridItemUIElement draggedItem))
        {
            int targetIndex = transform.GetSiblingIndex();
            draggedItem.transform.SetParent(transform.parent);
            draggedItem.transform.SetSiblingIndex(targetIndex);
        }
    }
}

public enum InventorySource
{
    Player,
    Guild
}
