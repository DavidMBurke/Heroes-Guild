using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public string questName = null!;
    public string location = null!;
    public string description = null!;
    public int level;
    public int xpReward;
    public int coinReward;
    public List<Item> itemReward = null!;
    public List<PlayerCharacter> characters = new();
    public int travelTime = 120;
    public int remainingTime = 0;
    public StatusEnum questStatus = StatusEnum.NotStarted;
    public bool staysAvailable = false;


    public enum StatusEnum
    {
        NotStarted = 0,
        Travelling,
        Ready,
        InDungeon,
        DungeonComplete,
        Returning,
        QuestComplete
    }

    public void StartQuest()
    {
        questStatus = StatusEnum.Travelling;
        remainingTime = travelTime;
    }

    public void EnterDungeon()
    {
        questStatus = StatusEnum.InDungeon;
        remainingTime = travelTime;
    }

    public void ReturnToGuild()
    {
        questStatus = StatusEnum.Returning;
        remainingTime = travelTime;
    }

    public void CompleteQuest()
    {
        foreach (PlayerCharacter character in characters)
        {
            GuildManager.instance.unassignedEmployees.Add(character);
            GuildManager.instance.assignedToQuest.Remove(character);
        }
        foreach (Item item in itemReward)
        {
            item.AddToInventory(GuildManager.instance.stockpile);
        }
    }

    public void Tick()
    {
        switch (questStatus)
        {
            case StatusEnum.NotStarted:
                break;

            case StatusEnum.Travelling:
                remainingTime --;
                if (remainingTime == 0)
                {
                    questStatus = StatusEnum.Ready;
                }
                break;

            case StatusEnum.Ready:
                break;

            case StatusEnum.InDungeon:
                remainingTime--;
                if (remainingTime == 0)
                {
                    questStatus = StatusEnum.DungeonComplete;
                }
                break;

            case StatusEnum.Returning:
                remainingTime --;
                if (remainingTime == 0)
                {
                    questStatus = StatusEnum.QuestComplete;
                }
                break;

            case StatusEnum.QuestComplete:
                break;

            default:
                break;
        }
    }

}
