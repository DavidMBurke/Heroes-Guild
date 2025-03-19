using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DevTools : MonoBehaviour
{
    MenuPanel panel;
    GuildManager gm;

    private void Start()
    {
        panel = GetComponentInChildren<MenuPanel>();
        panel.gameObject.SetActive(false);
        gm = GuildManager.instance;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            panel.gameObject.SetActive(!panel.gameObject.activeInHierarchy);
        }
    }

    public void AddCoin(int amount)
    {
        gm.AddCoin(amount);
    }

    public void GenerateCharactersForHire(int amount)
    {
        gm.GenerateCharactersForHire(amount);
    }

    public void GenerateEmployees(int amount)
    {
        gm.GenerateEmployees(amount);
    }

    public void GenerateCraftingItems()
    {
        foreach (List<Item> items in Item.allItemLists)
        {
            foreach (Item item in items)
            {
                item.AddToInventory(gm.stockpile, 100);
            }
        }
    }

    public void GenerateRandomEquipment()
    {
        foreach (Item ingot in Metals.MetalIngots.Where(m => m.tags.Any(t => t == "jewelry")))
        {
            foreach (Item gem in Jewelry.Gems)
            {
                float f = Random.Range(0, 10);
                if (f == 0)
                {
                    Item necklace = Jewelry.CreateNecklace(ingot, gem);
                    necklace.AddToInventory(gm.stockpile, 1);
                }
            }
        }
        foreach (Item ingot in Metals.MetalIngots.Where(m => m.tags.Any(t => t == "jewelry")))
        {
            foreach (Item gem in Jewelry.Gems)
            {
                float f = Random.Range(0, 10);
                if (f == 0)
                {
                    Item necklace = Jewelry.CreateBracelet(ingot, gem);
                    necklace.AddToInventory(gm.stockpile, 1);
                }
            }
        }
        foreach (Item ingot in Metals.MetalIngots.Where(m => m.tags.Any(t => t == "jewelry")))
        {
            foreach (Item gem in Jewelry.Gems)
            {
                float f = Random.Range(0, 10);
                if (f == 0)
                {
                    Item necklace = Jewelry.CreateRing(ingot, gem);
                    necklace.AddToInventory(gm.stockpile, 1);
                }
            }
        }
    }
}


