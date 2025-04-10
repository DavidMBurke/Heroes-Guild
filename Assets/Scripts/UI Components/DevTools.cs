using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DevTools : MonoBehaviour
{
    MenuPanel panel = null!;
    GuildManager gm = null!;

    private void Start()
    {
        panel = GetComponentInChildren<MenuPanel>();
        panel.gameObject.SetActive(false);
        gm = GuildManager.instance;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
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
        foreach (List<Item> items in Item.GenerateAllItemLists)
        {
            if (items == null)
            {
                Debug.LogError($"List {Item.GenerateAllItemLists.IndexOf(items)} cannot be found");
            }
            foreach (Item item in items)
            {
                if (item == null)
                {
                    Debug.LogError($"Item {items.IndexOf(item)} of {Item.GenerateAllItemLists.IndexOf(items)} cannot be found");
                }
                item.AddToInventory(gm.stockpile, 100);
            }
        }
    }

    public void GenerateRandomEquipment()
    {
        List<Item> jewelryMetals = Metals.MetalIngots.Where(m => m.tags.Any(t => t == "jewelry")).ToList();
        List<Item> smithingMetals = Metals.MetalIngots.Where(m => m.tags.Any(t => t == "smithing")).ToList();
        CreateRandomItems(jewelryMetals, Jewelry.Gems, 10, Jewelry.CreateNecklace);
        CreateRandomItems(jewelryMetals, Jewelry.Gems, 10, Jewelry.CreateBracelet);
        CreateRandomItems(jewelryMetals, Jewelry.Gems, 10, Jewelry.CreateRing);
        CreateRandomItems(smithingMetals, MonsterParts.Leathers, 3, Armor.Plate.CreatePlatebody);
        CreateRandomItems(smithingMetals, MonsterParts.Leathers, 3, Armor.Plate.CreatePlatelegs);
        CreateRandomItems(smithingMetals, MonsterParts.Leathers, 3, Armor.Plate.CreateBoots);
        CreateRandomItems(smithingMetals, MonsterParts.Leathers, 3, Armor.Plate.CreateHelmet);
        CreateRandomItems(smithingMetals, MonsterParts.Leathers, 3, Armor.Plate.CreateGauntlets);
        CreateRandomItems(MonsterParts.Leathers, Fabrics.Cloths, 2, Armor.Leather.CreateLeggings);
        CreateRandomItems(MonsterParts.Leathers, Fabrics.Cloths, 2, Armor.Leather.CreateVambraces);
        CreateRandomItems(MonsterParts.Leathers, Fabrics.Cloths, 2, Armor.Leather.CreateCoif);
        CreateRandomItems(MonsterParts.Leathers, Fabrics.Cloths, 2, Armor.Leather.CreateBoots);
        CreateRandomItems(MonsterParts.Leathers, Fabrics.Cloths, 2, Armor.Leather.CreateTop);
        CreateRandomItems(Fabrics.Cloths, Fabrics.Threads, 2, Armor.Cloth.CreateShoes);
        CreateRandomItems(Fabrics.Cloths, Fabrics.Threads, 2, Armor.Cloth.CreateGloves);
        CreateRandomItems(Fabrics.Cloths, Fabrics.Threads, 2, Armor.Cloth.CreateHat);
        CreateRandomItems(Fabrics.Cloths, Fabrics.Threads, 2, Armor.Cloth.CreatePants);
        CreateRandomItems(Fabrics.Cloths, Fabrics.Threads, 2, Armor.Cloth.CreateRobe);
        CreateRandomItems(smithingMetals, MonsterParts.Bones, 2, Weapons.Melee.CreateDagger);
        CreateRandomItems(smithingMetals, MonsterParts.Bones, 2, Weapons.Melee.CreateShortsword);
        CreateRandomItems(PlantParts.Woods, Fabrics.BowStrings, 2, Weapons.Ranged.CreateLongbow);
        CreateRandomItems(PlantParts.Woods, Fabrics.BowStrings, 2, Weapons.Ranged.CreateShortbow);
        CreateRandomItems(PlantParts.Woods, MonsterParts.Essences, 2, Weapons.Magic.CreateStaff);
        CreateRandomItems(PlantParts.Woods, MonsterParts.Essences, 2, Weapons.Magic.CreateWand);
    }

    public void CreateRandomItems(List<Item> itemList1, List<Item> itemList2, int chanceDenom, Func<Item, Item, Item> craftingFunction)
    {
        foreach (Item item1 in itemList1)
        {
            foreach (Item item2 in itemList2)
            {
                float f = Random.Range(0, chanceDenom);
                if (f == 0)
                {
                    Item newItem = craftingFunction(item1, item2);
                    newItem.AddToInventory(gm.stockpile, 1);
                }
            }
        }
    }
}


