using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class WorkshopPage : MonoBehaviour
{
    public PlayerCharacter selectedCharacter = null!;
    private GuildManager gm = null!;

    public CraftingQueue craftingQueue = null!;
    AssignWorkersPanel assignWorkersPanel = null!;
    CraftingPanel craftingPanel = null!;

    public GameObject craftingButtonsParent = null!;
    List<Button> craftingButtons = null!;

    public int workstationsCount = 3;
    public TextMeshProUGUI workstationsAssignedText = null!;

    private void Awake()
    {
        gm = GuildManager.instance;
        craftingQueue = GetComponentInChildren<CraftingQueue>();
        assignWorkersPanel = GetComponentInChildren<AssignWorkersPanel>();
        assignWorkersPanel.workshopPage = this;
        craftingPanel = GetComponentInChildren<CraftingPanel>();
        craftingPanel.workshopPage = this;
        InitializeCraftingButtons();
        SetPopupsInactive();
    }

    private void Start()
    {
        craftingPanel.InitializeCraftingOptions(GetCraftingOptions(), GetCrafters());
        UpdateWorkStationsAvailabilityText();
    }

    public void SetPopupsInactive()
    {
        assignWorkersPanel.gameObject.SetActive(false);
        craftingPanel.gameObject.SetActive(false);
    }

    public void UpdateWorkStationsAvailabilityText()
    {
        workstationsAssignedText.text = $"{GetCrafters().Count}/{workstationsCount} Assigned";
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

    public void Tick()
    {
        foreach (PlayerCharacter crafter in GetCrafters())
        {
#nullable enable
            ItemInQueue? queuedItem = craftingQueue.itemQueue.FirstOrDefault(i => i.assignedCrafter == crafter);
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
                queuedItem.assignedCrafter = crafter;
            }
            Skill crafterSkill = crafter.nonCombatSkills.skills[GetCraftingSkillName()];
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

    private void InitializeCraftingButtons()
    {
        craftingButtons = new List<Button>(craftingButtonsParent.GetComponentsInChildren<Button>());
        List<CraftingOption> options = GetCraftingOptions();

        for (int i = 0; i < craftingButtons.Count; i++)
        {
            if (i < options.Count)
            {
                craftingButtons[i].gameObject.SetActive(true);
                craftingButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = options[i].itemName;

                int index = i;
                craftingButtons[i].onClick.RemoveAllListeners();
                craftingButtons[i].onClick.AddListener(() => ToggleCraftingPanel(options[index]));
            }
            else
            {
                craftingButtons[i].gameObject.SetActive(false);
            }
        }
    }

    void ToggleCraftingPanel(CraftingOption option)
    {
        if (craftingPanel.gameObject.activeInHierarchy && craftingPanel.selectedCraftingOption == option)
        {
            craftingPanel.gameObject.SetActive(false);
            return;
        }
        craftingPanel.gameObject.SetActive(true);
        craftingPanel.SelectCraftingOption(option);
        assignWorkersPanel.gameObject.SetActive(false);
    }

    public abstract List<CraftingOption> GetCraftingOptions();
    public abstract List<PlayerCharacter> GetCrafters();
    public abstract string GetCraftingSkillName();
}
