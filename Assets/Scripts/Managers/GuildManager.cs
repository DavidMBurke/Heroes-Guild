using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public static GuildManager instance = null!;
    public int coin; //base currency in copper, to be displayed in broken down denominations (100 copper -> 1 silver, 100 silver -> 1 gold)
    public List<PlayerCharacter> unassignedEmployees = null!;
    public List<PlayerCharacter> charactersForHire = null!;
    public List<Quest> availableQuests = null!;
    public List<Item> stockpile = new List<Item>();

    // Workshops
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
        GameObject questGameObject = Instantiate(new GameObject());
        Quest quest = questGameObject.AddComponent<Quest>();
        quest.name = "First Quest";
        quest.questName = "First Quest";
        quest.location = "Forest";
        quest.level = 1;
        quest.description = "Go aventure!";
        quest.coinReward = 1000;
        quest.xpReward = 1000;
        availableQuests.Add(quest);
        timePerTick = 1;
        timeAdvancing = false;
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

    public void RemoveFromStockpile(Item item)
    {

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

    public void SetTimePerTick(float timePerTick)
    {
        this.timePerTick = timePerTick;
    }

    public void SetTimeAdvancing(bool isAdvancing)
    {
        timeAdvancing = isAdvancing;
    }

}
