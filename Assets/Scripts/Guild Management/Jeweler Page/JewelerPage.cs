using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JewelerPage : MonoBehaviour
{
    public GameObject characterListPanel;
    public GameObject characterList;
    public GameObject characterListItemPrefab;
    public GameObject assignCharacterButton;
    public GameObject unassignCharacterButton;
    public PlayerCharacter selectedCharacter;
    private GuildManager gm;


    private void Start()
    {
        gm = GuildManager.instance;
        characterListPanel.SetActive(false);
    }

    public void AssignJeweler()
    {
        if (selectedCharacter == null)
        {
            return;
        }
        if (!gm.unassignedEmployees.Contains(selectedCharacter))
        {
            Debug.LogWarning("Character not in unassignedEmployees");
            return;
        }
        gm.jewelers.Add(selectedCharacter);
        gm.unassignedEmployees.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
    }
    public void UnassignJeweler()
    {
        if (selectedCharacter == null)
        {
            return;
        }
        if (!gm.jewelers.Contains(selectedCharacter))
        {
            Debug.LogWarning("Character not in Jewelers");
            return;
        }
        gm.unassignedEmployees.Add(selectedCharacter);
        gm.jewelers.Remove(selectedCharacter);
        UpdateButtons();
        ResetCharacterList();
    }

    private void ResetCharacterList()
    {
        gm.unassignedEmployees.RemoveAll(character => character == null);
        foreach (Transform child in characterList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerCharacter character in gm.jewelers)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            CharacterListItem listItem = characterListItemObject.GetComponent<CharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            listItem.SetDisplayName(character.characterName + " (Assigned)");
        }
        foreach (PlayerCharacter character in gm.unassignedEmployees)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            CharacterListItem listItem = characterListItemObject.GetComponent<CharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
        }
    }

    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        assignCharacterButton.SetActive(true);
        UpdateButtons();
        ResetCharacterList();
    }

    private void UpdateButtons()
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

    public void ToggleCharacterSelectionPanel()
    {
        characterListPanel.SetActive(!characterListPanel.activeInHierarchy);
        if (characterListPanel.activeInHierarchy)
        {
            ResetCharacterList();
        }
    }
}
