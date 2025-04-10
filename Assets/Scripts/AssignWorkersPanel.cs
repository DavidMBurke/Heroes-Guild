using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignWorkersPanel : MonoBehaviour
{
    GuildManager gm = null!;
    public WorkshopPage workshopPage = null!;

    public GameObject characterList = null!;
    public GameObject characterListItemPrefab = null!;
    public GameObject assignCharacterButton = null!;
    public GameObject unassignCharacterButton = null!;
    public Button closeButton = null!;
    public GameObject noAvailableWorkerText = null!;

    private void Awake()
    {
        gm = GuildManager.instance;
    }

    private void Start()
    {
        assignCharacterButton.SetActive(false);
        unassignCharacterButton.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void ResetCharacterList()
    {
        if (gm == null)
        {
            gm = GuildManager.instance;
        }
        gm.unassignedEmployees.RemoveAll(character => character == null);

        foreach (Transform child in characterList.transform)
        {
            Destroy(child.gameObject);
        }

        // Get current crafters dynamically
        List<PlayerCharacter> currentCrafters = workshopPage.GetCrafters();
        string skillName = workshopPage.GetCraftingSkillName();

        // Populate Assigned Workers
        foreach (PlayerCharacter character in currentCrafters)
        {
            CreateCharacterListItem(character, true, skillName);
        }

        // Populate Unassigned Workers
        foreach (PlayerCharacter character in gm.unassignedEmployees)
        {
            CreateCharacterListItem(character, false, skillName);
        }

        noAvailableWorkerText.gameObject.SetActive(gm.unassignedEmployees.Count == 0);
    }

    private void CreateCharacterListItem(PlayerCharacter character, bool isAssigned, string skillName)
    {
        GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
        JobAssignmentCharacterListItem listItem = characterListItemObject.GetComponent<JobAssignmentCharacterListItem>();
        Button button = listItem.GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            SelectCharacter(character);
        });

        listItem.SetCharacter(character);
        string text1 = character.characterName + (isAssigned ? " (Assigned)" : "");
        string text2 = "Lvl: " + character.level.ToString();
        string text3 = skillName + ": " + character.nonCombatSkills.skills[skillName].level.ToString();
        listItem.SetText(text1, text2, text3);
        if (character == workshopPage.selectedCharacter)
        {
            listItem.SetHighlight(true);
        }
    }

    public void SelectCharacter(PlayerCharacter character)
    {
        workshopPage.selectedCharacter = character;
        assignCharacterButton.SetActive(true);
        UpdateButtons(character);
        ResetCharacterList();
    }

    private void UpdateButtons(PlayerCharacter selectedCharacter)
    {
        List<PlayerCharacter> currentCrafters = workshopPage.GetCrafters();

        if (gm.unassignedEmployees.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(true);
            unassignCharacterButton.SetActive(false);
        }
        else if (currentCrafters.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(true);
        }
        else
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(false);
        }
    }

    public void AssignCrafter()
    {
        PlayerCharacter crafter = workshopPage.selectedCharacter;
        if (crafter == null) return;

        if (!gm.unassignedEmployees.Contains(crafter))
        {
            Debug.LogWarning("Character not in unassignedEmployees");
            return;
        }

        List<PlayerCharacter> currentCrafters = workshopPage.GetCrafters();
        if (currentCrafters.Count >= workshopPage.workstationsCount)
        {
            AlertManager.instance.ShowAlert("More workstations needed to assign more crafters.");
            return;
        }

        // Dynamically assign to the correct list
        currentCrafters.Add(crafter);
        gm.unassignedEmployees.Remove(crafter);

        UpdateButtons(crafter);
        ResetCharacterList();
        workshopPage.UpdateWorkStationsAvailabilityText();
    }

    public void UnassignCrafter()
    {
        PlayerCharacter crafter = workshopPage.selectedCharacter;
        if (crafter == null) return;

        List<PlayerCharacter> currentCrafters = workshopPage.GetCrafters();
        if (!currentCrafters.Contains(crafter))
        {
            Debug.LogWarning("Character not currently assigned as crafter.");
            return;
        }

        // Dynamically remove from the correct list
        gm.unassignedEmployees.Add(crafter);
        currentCrafters.Remove(crafter);

        UpdateButtons(crafter);
        ResetCharacterList();
        workshopPage.UpdateWorkStationsAvailabilityText();
    }
}
