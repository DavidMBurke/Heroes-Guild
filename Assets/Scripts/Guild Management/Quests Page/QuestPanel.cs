using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    public TextMeshProUGUI title = null!;
    public TextMeshProUGUI location = null!;
    public TextMeshProUGUI description = null!;
    public TextMeshProUGUI rewards = null!;
    public Quest quest = null!;

    void Update()
    {
        if (quest == null)
        {
            title.text = string.Empty;
            location.text = string.Empty;
            description.text = string.Empty;
            rewards.text = string.Empty;
            return;
        }
        title.text = quest.questName;
        location.text = quest.location;
        description.text = quest.description;
        rewards.text = GenerateQuestRewardText();
    }

    private string GenerateQuestRewardText()
    {
        string text = string.Empty;
        text += $"Coin:\n  {quest.coinReward}\n\n";
        text += "Items:\n";
        foreach(var item in quest.itemReward)
        {
            text+= $"  {item.itemName} x {item.quantity}\n";
        }
        return text;
    }

    public void SetQuest(Quest _quest)
    {
        quest = _quest;
    }
}
