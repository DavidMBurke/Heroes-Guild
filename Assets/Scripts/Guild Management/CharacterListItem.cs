using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterListItem : MonoBehaviour
{
    private PlayerCharacter player = null!;
    private TextMeshProUGUI displayName = null!;
    private TextMeshProUGUI level = null!;
    public Image background;
    private Color initialColor;
    public Color highlightColor;
    public TMP_Dropdown jobDropdown = null!;
    public bool canAssignJobs = false;

    private void Awake()
    {
        List<TextMeshProUGUI> textItems = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        displayName = textItems.First(tmp => tmp.gameObject.name == "Name");
        level = textItems.First(tmp => tmp.gameObject.name == "Level");
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

    private void Start()
    {
        initialColor = GetComponent<Image>().color;
        if (canAssignJobs)
        {
            PopulateJobDropdown();
        }
    }

    public void SetCharacter(PlayerCharacter newPlayer)
    {
        player = newPlayer;
        UpdateDisplayInfo();
    }

    public void SetDisplayName(string name)
    {
        displayName.text = name;
    }

    private void UpdateDisplayInfo()
    {
        if (player == null)
        {
            Debug.LogError("No player assigned to CharacterListItem");
        }
        displayName.text = player.characterName;
        level.text = player.level.ToString();
    }

    public PlayerCharacter GetPlayer()
    {
        return player;
    }

    private void PopulateJobDropdown()
    {
        jobDropdown.ClearOptions();

        var jobOptions = GuildManager.instance.workerGroups.Keys
            .Where(job => job != "In Quest")
            .ToList();

        jobDropdown.AddOptions(jobOptions);
        jobDropdown.onValueChanged.AddListener(OnJobSelected);
    }

    private void OnJobSelected(int index)
    {
        string selectedJob = jobDropdown.options[index].text;

        if (selectedJob == "Unassigned")
        {
            foreach (List<PlayerCharacter> group in GuildManager.instance.workerGroups.Values)
            {
                if (group.Contains(player))
                {
                    group.Remove(player);
                }
            }
            GuildManager.instance.unassignedEmployees.Add(player);
            return;
        }

        List<PlayerCharacter> workshopList = GuildManager.instance.workerGroups[selectedJob];

        WorkshopPage workshop = selectedJob switch
        {
            "Jeweler" => GuildManager.instance.jewelerPage,
            "Armor Smith" => GuildManager.instance.armorSmithPage,
            "Weapon Smith" => GuildManager.instance.weaponSmithPage,
            "Leather Worker" => GuildManager.instance.leatherWorkersPage,
            "Tailor" => GuildManager.instance.tailorPage,
            "Fletcher" => GuildManager.instance.fletcherPage,
            "Enchanter" => GuildManager.instance.enchanterPage,
            "Arcanist" => GuildManager.instance.arcanistPage,
            "Alchemist" => GuildManager.instance.alchemistPage,
            "Cook" => GuildManager.instance.cooksPage,
            _ => null
        };

        if (workshop == null)
        {
            Debug.LogError($"No workshop found for job: {selectedJob}");
            return;
        }

        if (workshop.GetCrafters().Count >= workshop.workstationsCount)
        {
            AlertManager.instance.ShowAlert($"No available workstations for {selectedJob}");
            return;
        }

        foreach (List<PlayerCharacter> group in GuildManager.instance.workerGroups.Values )
        {
            if (group.Contains(player))
            {
                group.Remove(player);
            }
        }

        workshopList.Add(player);
        GuildManager.instance.unassignedEmployees.Remove(player);

        workshop.UpdateWorkStationsAvailabilityText();

    }
}
