using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    private List<TextMeshProUGUI> tmps = null!;
    private TextMeshProUGUI title = null!;
    private TextMeshProUGUI description = null!;
    public Quest quest = null!;

    void Start()
    {
        tmps = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        title = tmps.First(t => t.name == "Title");
        description = tmps.First(t => t.name == "Description");
    }

    void Update()
    {
        if (quest == null)
        {
            title.text = string.Empty;
            description.text = string.Empty;
            return;
        }
        title.text = quest.questName;
        description.text = quest.description;
    }

    public void SelectQuest(Quest _quest)
    {
        quest = _quest;
    }
}
