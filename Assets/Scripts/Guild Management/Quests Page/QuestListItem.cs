using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItem : MonoBehaviour
{
    public Quest quest = null!;
    public TextMeshProUGUI questName = null!;
    public TextMeshProUGUI questLocation = null!;
    public TextMeshProUGUI questLevel = null!;
    public TextMeshProUGUI status = null!;
    public TextMeshProUGUI time = null!;
    public Image background;
    public Button button;
    private Color initialColor;
    public Color highlightColor;
    public Quest.StatusEnum questStatus;

    private void Update()
    {
        if (quest == null)
        {
            Debug.LogError("No quest assigned to QuestListItem");
        }
        questName.text = quest.questName;
        questLocation.text = quest.location;
        questLevel.text = quest.level.ToString();
        switch (quest.questStatus)
        {
            case Quest.StatusEnum.NotStarted:
                status.text = "Not Started";
                time.text = $"Time Available: {quest.remainingTime / 60:D2}:{quest.remainingTime % 60:D2}";
                break;
            case Quest.StatusEnum.Travelling:
                status.text = "Travelling to Destination";
                time.text = $"Time until Arrival: {quest.remainingTime / 60:D2}:{quest.remainingTime % 60:D2}";
                break;
            case Quest.StatusEnum.Ready:
                status.text = "Ready to Begin";
                time.text = string.Empty;
                break;
            case Quest.StatusEnum.InDungeon:
                status.text = "In Dungeon";
                time.text = $"Time until Complete: {quest.remainingTime / 60:D2}:{quest.remainingTime % 60:D2}";
                break;
            case Quest.StatusEnum.DungeonComplete:
                status.text = "Ready to Return";
                time.text = string.Empty;
                break;
            case Quest.StatusEnum.Returning:
                status.text = "Returning";
                time.text = $"Time until Return: {quest.remainingTime / 60:D2}:{quest.remainingTime % 60:D2}";
                break;
            case Quest.StatusEnum.QuestComplete:
                status.text = "Quest Complete";
                time.text = string.Empty;
                break;
            default:
                Debug.LogError("Unknown quest status");
                break;
        }
    }


    public void SetHighlight(bool highlight = true)
    {
        if (highlight)
        {
            background.color = highlightColor;
        }
        else
        {
            background.color = initialColor;
        }
    }

}
