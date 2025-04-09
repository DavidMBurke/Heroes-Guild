using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public static GuildManager instance = null!;
    public int coin; //base currency in copper, to be displayed in broken down denominations (100 copper -> 1 silver, 100 silver -> 1 gold)
    public int dailyExpenses;
    public List<PlayerCharacter> charactersForHire = null!;
    public List<Quest> quests = null!;
    public List<Item> stockpile = new List<Item>();

    // Employees
    public List<PlayerCharacter> unassignedEmployees = new();
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
    
    // Time
    public float elapsedTime = 0;
    public bool timeAdvancing;
    public int year = 1;
    public int season; //30 day per season
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

        workerGroups = new Dictionary<string, List<PlayerCharacter>> {
            { "UnassignedEmployees", unassignedEmployees },
            { "Jewelers", jewelers },
            { "ArmorSmiths", armorSmiths },
            { "WeaponSmiths", weaponSmiths },
            { "LeatherWorkers", leatherWorkers },
            { "Tailors", tailors },
            { "Fletchers", fletchers },
            { "Enchanters", enchanters },
            { "Arcanists", arcanists },
            { "Alchemists", alchemists },
            { "Cooks", cooks }
        };

    }

    private void AddPlaceholderQuests()
    {
        GameObject questGameObject = Instantiate(new GameObject());
        Quest quest = questGameObject.AddComponent<Quest>();
        quest.name = "Missing Caravan";
        quest.questName = "Missing Caravan";
        quest.location = "Forest";
        quest.level = 1;
        quest.description = "The supply caravan that was supposed to arrive yesterday never came. We will need those supplies to get started in establishing our guild!";
        quest.coinReward = 1000;
        quest.xpReward = 1000;
        quest.staysAvailable = true;
        quest.itemReward = new List<Item> {
            Fabrics.Cloths[(int)Fabrics.ClothEnum.BasicCloth].Clone(100),
            Fabrics.Threads[(int)Fabrics.ThreadEnum.BasicThread].Clone(100),
            PlantParts.Woods[(int)PlantParts.WoodsEnum.AshWood].Clone(100),
            PlantParts.Misc[(int)PlantParts.MiscEnum.PlantFiber].Clone(100),
            
        };
        quests.Add(quest);

        GameObject HuntingQuestObject = Instantiate(new GameObject());
        Quest huntingQuest = HuntingQuestObject.AddComponent<Quest>();
        huntingQuest.name = "Hunting Expedition";
        huntingQuest.questName = "Hunting Expedition";
        huntingQuest.location = "Forest";
        huntingQuest.level = 1;
        huntingQuest.description = "Go and collect resources from the monsters in the nearby forest!";
        huntingQuest.coinReward = 1000;
        huntingQuest.xpReward = 1000;
        huntingQuest.staysAvailable = true;
        huntingQuest.itemReward = new List<Item> {
            MonsterParts.Bones[(int)MonsterParts.BonesEnum.BloodfangBone].Clone(100),
            MonsterParts.Bones[(int)MonsterParts.BonesEnum.BrindlegrazerBone].Clone(100),
            MonsterParts.Leathers[(int)MonsterParts.LeathersEnum.BloodfangLeather].Clone(100),
            MonsterParts.Leathers[(int)MonsterParts.LeathersEnum.BrindlegrazerLeather].Clone(100),
            MonsterParts.Essences[(int)MonsterParts.EssencesEnum.TinyNatureEssence].Clone(50),
            MonsterParts.Essences[(int)MonsterParts.EssencesEnum.SmallNatureEssence].Clone(50),
            Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawBrindlegrazerMeat].Clone(100),
            Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawBloodfangMeat].Clone(100),
            Food.Meat.RawMeat[(int)Food.Meat.RawMeatEnum.RawGigantopillarMeat].Clone(100),
            
        };
        quests.Add(huntingQuest);

        GameObject MiningQuestObject = Instantiate(new GameObject());
        Quest miningQuest = MiningQuestObject.AddComponent<Quest>();
        miningQuest.name = "Mining Expedition";
        miningQuest.questName = "Mining Expedition";
        miningQuest.location = "Caves";
        miningQuest.level = 1;
        miningQuest.description = "Go and collect metal ores in the nearby caves!";
        miningQuest.coinReward = 1000;
        miningQuest.xpReward = 1000;
        miningQuest.staysAvailable = true;
        miningQuest.itemReward = new List<Item> {
            Metals.MetalIngots[(int)Metals.MetalIngotEnum.CopperIngot].Clone(100),
            Metals.MetalIngots[(int)Metals.MetalIngotEnum.IronIngot].Clone(100),
            Metals.MetalIngots[(int)Metals.MetalIngotEnum.BrassIngot].Clone(100),
            Metals.MetalIngots[(int)Metals.MetalIngotEnum.SilverIngot].Clone(100),
        };
        quests.Add(miningQuest);
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

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public void RemoveCoin(int amount)
    {
        coin -= amount;
    }

    public void AddToStockpile(Item item)
    {
        Item stockPileItem = stockpile.FirstOrDefault(i => i.itemName == item.itemName && i.description == item.description);
        if (stockPileItem != null)
        {
            stockPileItem.quantity += item.quantity;
        }
        else
        {
            stockpile.Add(item);
        }

    }

    public void RemoveFromStockpile(Item item, int quantity = 1)
    {
        Item stockpileItem = stockpile.FirstOrDefault(i => i.itemName == item.itemName && i.description == item.description);
        if (stockpileItem != null)
        {
            stockpileItem.quantity -= quantity;
            if (stockpileItem.quantity <= 0)
            {
                stockpile.Remove(stockpileItem);
            }
        } 
        else
        {
            Debug.LogError($"Tried to remove {item.itemName} from stockpile but item was not found");
        }
        
    }

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
        
    }
    
    public void EndDay()
    {

    }

    private void IncrementTime()
    {
        minute += 1;
        if (minute >= 60)
        {
            minute -= 60;
            hour += 1;
        }
        if (hour >= 24)
        {
            hour -= 24;
            day += 1;
            SubtractDailyExpenses();
        }
        if (day >= 30)
        {
            day -= 30;
            season += 1;
        }
        if (season >= 4)
        {
            season -= 4;
            year += 1;
        }
    }

    public void CalculateDailyExpenses()
    {
        int expenses = 0;
        foreach (var group in workerGroups)
        {
            foreach (PlayerCharacter worker in group.Value)
            {
                expenses += worker.salary;
            }
        }
        dailyExpenses = expenses;
    }

    private void SubtractDailyExpenses()
    {
        coin -= dailyExpenses;
    }

    public void SetTimePerTick(float timePerTick)
    {
        this.timePerTick = timePerTick;
    }

    public void SetTimeAdvancing(bool isAdvancing)
    {
        timeAdvancing = isAdvancing;
    }

    public void AdvanceToNextDay()
    {
        int timeout = 60 * 24;
        int iteration = 0;
        Tick();
        while (!(hour == 6 && minute == 0))
        {
            iteration++;
            if (iteration > timeout)
            {
                Debug.LogError("Skipped greater than 24 hours");
            }
            Tick();
        }
    }

}
