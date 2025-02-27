using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkshopPage : MonoBehaviour
{

    public PlayerCharacter selectedCharacter;
    private GuildManager gm;

    public CraftingQueue craftingQueue;
    AssignWorkersPanel assignWorkersPanel;
    CraftingPanel craftingPanel;


    public int workstationsCount = 3;
    public TextMeshProUGUI workstationsAssignedText;


    private void Awake()
    {
        gm = GuildManager.instance;
        craftingQueue = GetComponentInChildren<CraftingQueue>();
        assignWorkersPanel = GetComponentInChildren<AssignWorkersPanel>();
        assignWorkersPanel.workshopPage = this;
        craftingPanel = GetComponentInChildren<CraftingPanel>();
        craftingPanel.workshopPage = this;
        SetPopupsInactive();
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
    }

    public void SetPopupsInactive()
    {
        assignWorkersPanel.gameObject.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
    }

    public void UpdateWorkStationsAvailabilityText()
    {
        workstationsAssignedText.text = $"{gm.jewelers.Count}/{workstationsCount} Assigned";
    }

    public void ToggleCharacterSelectionPanel()
    {
        assignWorkersPanel.gameObject.SetActive(!assignWorkersPanel.gameObject.activeInHierarchy);
        assignWorkersPanel.ResetCharacterList();
        if (assignWorkersPanel.gameObject.activeInHierarchy)
        {
            craftingPanel.gameObject.SetActive(false);
        }
    }

    public void ToggleCraftPanel()
    {
        craftingPanel.gameObject.SetActive(!craftingPanel.gameObject.activeInHierarchy);
        if (craftingPanel.gameObject.activeInHierarchy)
        {
            assignWorkersPanel.gameObject.SetActive(false);
        }
    }

    public void Tick()
    {
        foreach (PlayerCharacter jeweler in gm.jewelers)
        {
            #nullable enable
            ItemInQueue? queuedItem = craftingQueue.itemQueue.FirstOrDefault(i => i.assignedCrafter == jeweler);
            #nullable disable
            if (queuedItem == null)
            {
                queuedItem = craftingQueue.itemQueue.FirstOrDefault(i => i.assignedCrafter == null);
            }
            if (queuedItem == null)
            {
                continue;
            }
            if (queuedItem.assignedCrafter == null)
            {
                queuedItem.assignedCrafter = jeweler;
            }
            Skill crafterSkill = jeweler.nonCombatSkills.skills[NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting)];
            int remainingWork = queuedItem.workToComplete - queuedItem.workDone;
            int workAdded = (crafterSkill.level > remainingWork) ? remainingWork : crafterSkill.level;
            queuedItem.workDone += workAdded;
            crafterSkill.AddXP(workAdded);
            if (queuedItem.workDone >= queuedItem.workToComplete)
            {
                craftingQueue.itemQueue.Remove(queuedItem);
                craftingQueue.completedItems.Add(queuedItem);
            }
        }
        craftingQueue.UpdateItemValues();
    }

    public void ToggleNecklaceCrafting()
    {
        ToggleCraftPanel();
        if (!craftingPanel.gameObject.activeInHierarchy)
        {
            return;
        }
        craftingPanel.addToQueueButton.onClick.RemoveAllListeners();
        craftingPanel.addToQueueButton.onClick.AddListener(() => craftingPanel.AddNecklaceToQueue());
    }

}
