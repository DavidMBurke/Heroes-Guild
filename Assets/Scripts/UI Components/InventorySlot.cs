using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Interactable interactable;
    public Image image;

    private void Start()
    {
        image = GetComponentsInChildren<Image>().ToList().FirstOrDefault(i => i.name == "Icon");
    }

    public void UpdateItem(Interactable i)
    {
        if (i != null && i.sprite != null)
        {
            interactable = i;
            image.sprite = i.sprite;
            image.color = Color.white;
            return;
        }        
        image.sprite = null;
        image.color = Color.clear;
    }
}
