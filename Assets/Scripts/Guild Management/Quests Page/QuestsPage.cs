using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestsPage : MonoBehaviour
{
    public QuestPanel questPanel;
    public GameObject questList;
    public GameObject questListItemPrefab;
    public GameObject startQuestPanel;

    private Quest selectedQuest;
    private GuildManager gm;

    private void Start()
    {
        gm = GuildManager.instance;
        ResetList();
        startQuestPanel.SetActive(false);
    }

    private void ResetList()
    {
        gm.availableQuests.RemoveAll(quest => quest == null);
        foreach (Transform child in questList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Quest quest in gm.availableQuests)
        {
            QuestListItem questListItem = Instantiate(questListItemPrefab, questList.transform).GetComponent<QuestListItem>();
            Button button = questListItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectQuest(quest);
            });
            questListItem.quest = quest;
        }
    }

    public void SelectQuest(Quest quest)
    {
        selectedQuest = quest;
        questPanel.SelectQuest(quest);
        ResetList();
        startQuestPanel.SetActive(true);
    }

    public void OpenStartQuestMenu()
    {
        startQuestPanel.SetActive(true);
        startQuestPanel.GetComponent<StartQuestPanel>().ClearCharacterSelection();
    }

    public void HideStartQuestMenu()
    {
        startQuestPanel.SetActive(false);
    }
}
