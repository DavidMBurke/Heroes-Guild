using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignWorkersPanel : MonoBehaviour
{
    GuildManager gm;
    public WorkshopPage workshopPage;

    public GameObject characterList;
    public GameObject characterListItemPrefab;
    public GameObject assignCharacterButton;
    public GameObject unassignCharacterButton;
    public Button closeButton;

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

    private void OnEnable()
    {
        ResetCharacterList();
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
        foreach (PlayerCharacter character in gm.jewelers)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            JobAssignmentCharacterListItem listItem = characterListItemObject.GetComponent<JobAssignmentCharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            string text1 = character.characterName + (" (Assigned)");
            string text2 = "Lvl: " + character.level.ToString();
            string skillName = NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting);
            string text3 = skillName + ": " + character.nonCombatSkills.skills[skillName].level.ToString();
            listItem.SetText(text1, text2, text3);
        }
        foreach (PlayerCharacter character in gm.unassignedEmployees)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            JobAssignmentCharacterListItem listItem = characterListItemObject.GetComponent<JobAssignmentCharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            string text1 = character.characterName;
            string text2 = "Lvl: " + character.level.ToString();
            string skillName = NonCombatSkills.GetName(NonCombatSkills.Enum.JewelryCrafting);
            string text3 = skillName + ": " + character.nonCombatSkills.skills[skillName].level.ToString();
            listItem.SetText(text1, text2, text3);
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
        if (gm.unassignedEmployees.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(true);
            unassignCharacterButton.SetActive(false);
        }
        if (gm.jewelers.Contains(selectedCharacter))
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(true);
        }
        if (selectedCharacter == null)
        {
            assignCharacterButton.SetActive(false);
            unassignCharacterButton.SetActive(false);
        }
    }

    public void AssignCrafter()
    {
        PlayerCharacter crafter = workshopPage.selectedCharacter;
        if (crafter == null)
        {
            return;
        }
        if (!gm.unassignedEmployees.Contains(crafter))
        {
            Debug.LogWarning("Character not in unassignedEmployees");
            return;
        }
        if (gm.jewelers.Count >= workshopPage.workstationsCount)
        {
            Debug.Log("More workstations needed to assign more crafters.");
            return;
        }
        gm.jewelers.Add(crafter);
        gm.unassignedEmployees.Remove(crafter);
        UpdateButtons(crafter);
        ResetCharacterList();
        workshopPage.UpdateWorkStationsAvailabilityText();
    }
    public void UnassignCrafter()
    {
        PlayerCharacter crafter = workshopPage.selectedCharacter;
        if (crafter == null)
        {
            return;
        }
        if (!gm.jewelers.Contains(crafter))
        {
            Debug.LogWarning("Character not currently assigned as crafter.");
            return;
        }
        gm.unassignedEmployees.Add(crafter);
        gm.jewelers.Remove(crafter);
        UpdateButtons(crafter);
        ResetCharacterList();
        workshopPage.UpdateWorkStationsAvailabilityText();
    }

}
