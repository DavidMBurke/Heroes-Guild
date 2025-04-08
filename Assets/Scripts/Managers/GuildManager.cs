using System;
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
    public List<PlayerCharacter> unassignedEmployees = null!;
    public List<PlayerCharacter> jewelers = null!;
    public List<PlayerCharacter> armorSmiths = null!;
    public List<PlayerCharacter> weaponSmiths = null!;
    public List<PlayerCharacter> leatherWorkers = null!;
    public List<PlayerCharacter> tailors = null!;
    public List<PlayerCharacter> fletchers = null!;
    public List<PlayerCharacter> enchanters = null!;
    public List<PlayerCharacter> arcanists = null!;
    public List<PlayerCharacter> alchemists = null!;
    public List<PlayerCharacter> cooks = null!;
    Dictionary<string, List<PlayerCharacter>> workerGroups;

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
        quest.name = "First Quest";
        quest.questName = "First Quest";
        quest.location = "Forest";
        quest.level = 1;
        quest.description = "Go aventure!";
        quest.coinReward = 1000;
        quest.xpReward = 1000;
        quests.Add(quest);

        GameObject questGameObject2 = Instantiate(new GameObject());
        Quest quest2 = questGameObject2.AddComponent<Quest>();
        quest2.name = "Placeholder Quest";
        quest2.questName = "Placeholder Quest";
        quest2.location = "Forest";
        quest2.level = 2;
        quest2.description = "Yada yada yada!";
        quest2.coinReward = 1000;
        quest2.xpReward = 1000;
        quests.Add(quest2);
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
