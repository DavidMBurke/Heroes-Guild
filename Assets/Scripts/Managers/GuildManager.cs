using System.Collections.Generic;
using UnityEngine;

public class GuildManager : MonoBehaviour
{
    public static GuildManager instance;
    public int coin; //base currency in copper, to be displayed in broken down denominations (100 copper -> 1 silver, 100 silver -> 1 gold)
    public List<PlayerCharacter> employees;
    public List<PlayerCharacter> charactersForHire;
    public List<Quest> availableQuests;

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
    }

    public void AddCoin(int amount)
    {
        coin += amount;
    }

    public void RemoveCoin(int amount)
    {
        coin -= amount;
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
            employees.Add(PlayerCharacter.CreateNewCharacter());
        }
    }
}
