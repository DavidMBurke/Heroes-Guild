using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGridItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;
    Transform originalParent;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemQuantity;
#nullable enable
    Item? item;
#nullable disable


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        canvasGroup.blocksRaycasts = true;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        itemName.text = item == null ? "" : item.itemName;
        itemQuantity.text = item == null ? "" : item.quantity.ToString();
    }

}
