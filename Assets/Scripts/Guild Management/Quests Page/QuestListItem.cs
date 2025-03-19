using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestListItem : MonoBehaviour
{
    public Quest quest = null!;
    public TextMeshProUGUI questName = null!;
    public TextMeshProUGUI questLocation = null!;
    public TextMeshProUGUI questLevel = null!;

    private void Start()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        questName = textItems.First(tmp => tmp.gameObject.name == "Name");
        questLocation = textItems.First(tmp => tmp.gameObject.name == "Location");
        questLevel = textItems.First(tmp => tmp.gameObject.name == "Level");
    }

    private void Update()
    {
        if (quest == null)
        {
            Debug.LogError("No quest assigned to QuestListItem");
        }
        questName.text = quest.questName;
        questLocation.text = quest.location;
        questLevel.text = quest.level.ToString();
    }
}
