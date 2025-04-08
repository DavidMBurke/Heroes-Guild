using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsPage : MonoBehaviour
{
    public QuestPanel questPanel = null!;
    public GameObject questList = null!;
    public GameObject questListItemPrefab = null!;
    public GameObject startQuestPanel = null!;

    public Quest selectedQuest = null!;
    private GuildManager gm = null!;
    

    private void Start()
    {
        gm = GuildManager.instance;
        ResetList();
        startQuestPanel.SetActive(false);
    }

    private void ResetList()
    {
        gm.quests.RemoveAll(quest => quest == null);
        foreach (Transform child in questList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Quest quest in gm.quests)
        {
            QuestListItem questListItem = Instantiate(questListItemPrefab, questList.transform).GetComponent<QuestListItem>();
            Quest q = quest;
            questListItem.button.onClick.AddListener(() =>
            {
                SelectQuest(q);
            });
            questListItem.quest = q;
            if (q == selectedQuest)
            {
                questListItem.SetHighlight();
            }
        }
    }

    public void SelectQuest(Quest quest)
    {
        selectedQuest = quest;
        questPanel.SetQuest(quest);
        ResetList();
    }

    public void OpenStartQuestMenu()
    {
        startQuestPanel.SetActive(true);
        QuestManagementPanel panel = startQuestPanel.GetComponent<QuestManagementPanel>();
        panel.SetQuest(selectedQuest);
        panel.ClearCharacterSelection();
    }

    public void HideStartQuestMenu()
    {
        startQuestPanel.SetActive(false);
    }

    public void RemoveQuest(Quest quest)
    {
        GuildManager.instance.quests.Remove(quest);
        foreach (Transform child in questList.transform)
        {
            QuestListItem listItem = child.GetComponent<QuestListItem>();
            if (listItem != null && listItem.quest == quest)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }
}
