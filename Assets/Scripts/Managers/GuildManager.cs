using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the overall state and progression of the guild including economy, time, employees, and quests.
/// </summary>
public class GuildManager : MonoBehaviour
{
    public static GuildManager instance = null!;

    // ========== Currency & Economy ==========
    public int coin; // stored in copper
    public int dailyExpenses;

    // ========== Characters & Resources ==========
    public List<PlayerCharacter> charactersForHire = null!;
    public List<Quest> quests = null!;
    public List<Item> stockpile = new();

    // ========== Employee Management ==========
    public List<PlayerCharacter> unassignedEmployees = new();
    public List<PlayerCharacter> assignedToQuest = new();
    public List<PlayerCharacter> jewelers = new();
    public List<PlayerCharacter> armorSmiths = new();
    public List<PlayerCharacter> weaponSmiths = new();
    public List<PlayerCharacter> leatherWorkers = new();
    public List<PlayerCharacter> tailors = new();
    public List<PlayerCharacter> fletchers = new();
    public List<PlayerCharacter> enchanters = new();
    public List<PlayerCharacter> arcanists = new();
    public List<PlayerCharacter> alchemists = new();
    public List<PlayerCharacter> cooks = new();
    public Dictionary<string, List<PlayerCharacter>> workerGroups;

    // ========== UI Pages ==========
    public WorkshopPage jewelerPage = null!;
    public WorkshopPage armorSmithPage = null!;
    public WorkshopPage weaponSmithPage = null!;
    public WorkshopPage leatherWorkersPage = null!;
    public WorkshopPage tailorPage = null!;
    public WorkshopPage fletcherPage = null!;
    public WorkshopPage enchanterPage = null!;
    public WorkshopPage arcanistPage = null!;
    public WorkshopPage alchemistPage = null!;
    public WorkshopPage cooksPage = null!;
    public HirePage hirePage = null!;

    // ========== Time & Progression ==========
    public float elapsedTime = 0;
    public bool timeAdvancing;
    public int year = 1;
    public int season;
    public int day = 1;
    public int hour;
    public int minute;
    public float timePerTick;
    public float dayStartHour = 6;
    public float dayEnd = 20;

    private void Awake()
    {
        instance = this;
        AddPlaceholderQuests();
        timePerTick = 1;
        timeAdvancing = false;
        coin = 10000;

        workerGroups = new Dictionary<string, List<PlayerCharacter>>
        {
            { "Unassigned", unassignedEmployees },
            { "In Quest", assignedToQuest },
            { "Jeweler", jewelers },
            { "Armor Smith", armorSmiths },
            { "Weapon Smith", weaponSmiths },
            { "Leather Worker", leatherWorkers },
            { "Tailor", tailors },
            { "Fletcher", fletchers },
            { "Enchanter", enchanters },
            { "Arcanist", arcanists },
            { "Alchemist", alchemists },
            { "Cook", cooks }
        };
    }

    private void Update()
    {
        if (timeAdvancing)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > timePerTick)
            {
                Tick();
                elapsedTime -= timePerTick;
            }
        }
    }

    // ========== Currency Methods ==========

    public void AddCoin(int amount) => coin += amount;
    public void RemoveCoin(int amount) => coin -= amount;

    // ========== Stockpile Management ==========

    public void AddToStockpile(Item item)
    {
        Item existing = stockpile.FirstOrDefault(i => i.itemName == item.itemName && i.description == item.description);
        if (existing != null)
        {
            existing.quantity += item.quantity;
        }
        else
        {
            stockpile.Add(item);
        }
    }

    public void RemoveFromStockpile(Item item, int quantity = 1)
    {
        Item existing = stockpile.FirstOrDefault(i => i.itemName == item.itemName && i.description == item.description);
        if (existing != null)
        {
            existing.quantity -= quantity;
            if (existing.quantity <= 0)
            {
                stockpile.Remove(existing);
            }
        }
        else
        {
            Debug.LogError($"Tried to remove {item.itemName} from stockpile but item was not found.");
        }
    }

    // ========== Character Generation ==========

    public void GenerateCharactersForHire(int amount)
    {
        charactersForHire.Clear();
        for (int i = 0; i < amount; i++)
        {
            charactersForHire.Add(PlayerCharacter.CreateNewCharacter());
        }
    }

    public void GenerateEmployees(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            unassignedEmployees.Add(PlayerCharacter.CreateNewCharacter());
        }
    }

    // ========== Daily & Tick Logic ==========

    public void Tick()
    {
        IncrementTime();

        if (hour == dayStartHour && minute == 0)
            StartDay();

        jewelerPage.Tick();
        armorSmithPage.Tick();
        weaponSmithPage.Tick();
        leatherWorkersPage.Tick();
        tailorPage.Tick();
        fletcherPage.Tick();
        enchanterPage.Tick();
        arcanistPage.Tick();
        alchemistPage.Tick();
        cooksPage.Tick();

        foreach (Quest quest in quests)
        {
            quest.Tick();
        }
    }

    public void StartDay()
    {
        List<PlayerCharacter> toRemove = new();
        foreach (var employee in charactersForHire)
        {
            if (Random.Range(0, 10) < 2)
                toRemove.Add(employee);
        }
        charactersForHire.RemoveAll(c => toRemove.Contains(c));

        for (int i = 0; i < 10; i++)
        {
            if (Random.Range(0, 10) < 3)
                charactersForHire.Add(PlayerCharacter.CreateNewCharacter());
        }

        hirePage.ResetList();
    }

    public void EndDay() { /* Future end-of-day logic */ }

    private void IncrementTime()
    {
        minute += 1;
        if (minute >= 60)
        {
            minute = 0;
            hour += 1;
        }

        if (hour >= 24)
        {
            hour = 0;
            day += 1;
            SubtractDailyExpenses();
        }

        if (day >= 30)
        {
            day = 0;
            season += 1;
        }

        if (season >= 4)
        {
            season = 0;
            year += 1;
        }
    }

    public void CalculateDailyExpenses()
    {
        int total = 0;
        foreach (var group in workerGroups.Values)
        {
            foreach (var worker in group)
            {
                total += worker.salary;
            }
        }
        dailyExpenses = total;
    }

    private void SubtractDailyExpenses() => coin -= dailyExpenses;

    public void SetTimePerTick(float timePerTick) => this.timePerTick = timePerTick;
    public void SetTimeAdvancing(bool isAdvancing) => timeAdvancing = isAdvancing;

    /// <summary>
    /// Advances time to the start of the next day.
    /// </summary>
    public void AdvanceToNextDay()
    {
        int timeout = 60 * 24;
        int iteration = 0;
        Tick();

        while (!(hour == 6 && minute == 0))
        {
            if (++iteration > timeout)
            {
                Debug.LogError("Skipped greater than 24 hours");
                break;
            }
            Tick();
        }
    }

    // ========== Placeholder Quest Setup ==========

    private void AddPlaceholderQuests()
    {
        quests = new List<Quest>();

        // Missing Caravan Quest
        quests.Add(CreateQuest("Missing Caravan", "Forest", 1,
            "The supply caravan that was supposed to arrive yesterday never came...",
            1000, 1000,
            new List<Item> {
                Fabrics.Cloths[(int)Fabrics.ClothEnum.BasicCloth].Clone(100),
                Fabrics.Threads[(int)Fabrics.ThreadEnum.BasicThread].Clone(100),
                PlantParts.Woods[(int)PlantParts.WoodsEnum.AshWood].Clone(100),
                PlantParts.Misc[(int)PlantParts.MiscEnum.PlantFiber].Clone(100),
            }));

        // Hunting Expedition
        quests.Add(CreateQuest("Hunting Expedition", "Forest", 1,
            "Go and collect resources from the monsters in the nearby forest!",
            1000, 1000,
            new List<Item> {
                MonsterParts.Bones[(int)MonsterParts.BonesEnum.BloodfangBone].Clone(100),
                MonsterParts.Bones[(int)MonsterParts.BonesEnum.BrindlegrazerBone].Clone(100),
                MonsterParts.Leathers[(int)MonsterParts.LeathersEnum.BloodfangLeather].Clone(100),
                MonsterParts.Leathers[(int)MonsterParts.LeathersEnum.BrindlegrazerLeather].Clone(100),
                MonsterParts.Essences[(int)MonsterParts.EssencesEnum.TinyNatureEssence].Clone(50),
                MonsterParts.Essences[(int)MonsterParts.EssencesEnum.SmallNatureEssence].Clone(50),
                Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawBrindlegrazerMeat].Clone(100),
                Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawBloodfangMeat].Clone(100),
                Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawGigantopillarMeat].Clone(100),
            }));

        // Mining Expedition
        quests.Add(CreateQuest("Mining Expedition", "Caves", 1,
            "Go and collect metal ores in the nearby caves!",
            1000, 1000,
            new List<Item> {
                Metals.MetalIngots[(int)Metals.MetalIngotEnum.CopperIngot].Clone(100),
                Metals.MetalIngots[(int)Metals.MetalIngotEnum.IronIngot].Clone(100),
                Metals.MetalIngots[(int)Metals.MetalIngotEnum.BrassIngot].Clone(100),
                Metals.MetalIngots[(int)Metals.MetalIngotEnum.SilverIngot].Clone(100),
                Jewelry.Gems[(int)Jewelry.GemEnum.RedFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.OrangeFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.YellowFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.GreenFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.BlueFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.PurpleFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.BlackFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.WhiteFluorite].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.RedGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.OrangeGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.YellowGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.GreenGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.BlueGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.PurpleGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.BlackGarnet].Clone(5),
                Jewelry.Gems[(int)Jewelry.GemEnum.WhiteGarnet].Clone(5),
            }));
    }

    private Quest CreateQuest(string name, string location, int level, string description, int coinReward, int xpReward, List<Item> itemReward)
    {
        GameObject questGO = Instantiate(new GameObject(name));
        Quest quest = questGO.AddComponent<Quest>();
        quest.questName = name;
        quest.location = location;
        quest.level = level;
        quest.description = description;
        quest.coinReward = coinReward;
        quest.xpReward = xpReward;
        quest.staysAvailable = true;
        quest.itemReward = itemReward;
        return quest;
    }
}
