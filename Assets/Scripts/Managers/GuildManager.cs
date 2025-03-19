using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public static GuildManager instance;
    public int coin; //base currency in copper, to be displayed in broken down denominations (100 copper -> 1 silver, 100 silver -> 1 gold)
    public List<PlayerCharacter> unassignedEmployees;
    public List<PlayerCharacter> charactersForHire;
    public List<Quest> availableQuests;
    public List<Item> stockpile = new List<Item>();

    // Workshops
    public List<PlayerCharacter> jewelers;
    public List<PlayerCharacter> armorSmiths;
    public List<PlayerCharacter> weaponSmiths;
    public List<PlayerCharacter> leatherWorkers;
    public List<PlayerCharacter> tailors;
    public List<PlayerCharacter> fletchers;
    public List<PlayerCharacter> enchanters;
    public List<PlayerCharacter> arcanists;
    public List<PlayerCharacter> alchemists;
    public List<PlayerCharacter> cooks;
    public WorkshopPage jewelerPage;
    public WorkshopPage armorSmithPage;
    public WorkshopPage weaponSmithPage;
    public WorkshopPage leatherWorkersPage;
    public WorkshopPage tailorPage;
    public WorkshopPage fletcherPage;
    public WorkshopPage enchanterPage;
    public WorkshopPage arcanistPage;
    public WorkshopPage alchemistPage;
    public WorkshopPage cooksPage;

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
