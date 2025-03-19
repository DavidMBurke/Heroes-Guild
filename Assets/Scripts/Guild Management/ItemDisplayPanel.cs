using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayPanel : MonoBehaviour
{
    Item item = null!;
    public TextMeshProUGUI itemName = null!;
    public TextMeshProUGUI description = null!;
    public TextMeshProUGUI quantity = null!;
    public Image image = null!;

    private void Start()
    {
        itemName.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        quantity.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
    }
    public void AssignItem(Item item)
    {
        this.item = item;
        UpdateItem();
    }

    public void UpdateItem()
    {
        itemName.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        quantity.gameObject.SetActive(true);
        image.gameObject.SetActive(true);
        itemName.text = item.itemName;
        description.text = item.description;
        quantity.text = item.quantity.ToString();
        image.sprite = item.sprite;
    }
}
