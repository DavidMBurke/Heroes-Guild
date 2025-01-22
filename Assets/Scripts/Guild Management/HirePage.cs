using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HirePage : MonoBehaviour
{
    public CharacterForHirePanel panel;
    public GameObject characterListItemPrefab;
    public GameObject characterListObject;
    public GameObject hireButton;
    private PlayerCharacter selectedCharacter;
    private GuildManager gm;

    public void Start()
    {
        gm = GuildManager.instance;
        gm.GenerateCharactersForHire(10);
        ResetList();
        hireButton.SetActive(false);
    }

    private void ResetList()
    {
        gm.charactersForHire.RemoveAll(character => character == null);
        foreach (Transform child in characterListObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerCharacter character in gm.charactersForHire)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterListObject.transform);
            CharacterListItem listItem = characterListItemObject.GetComponent<CharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.player = character;
        }
    }

    private void Update()
    {
        hireButton.SetActive(panel.character != null);
    }

    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        panel.AssignCharacter(character);
        hireButton.GetComponentInChildren<TextMeshProUGUI>().text = $"Hire ({character.salary}/week)";
    }

    public void HireSelectedCharacter()
    {
        if (gm.coin < selectedCharacter.salary)
        {
            Debug.Log("Cannot afford");
            return;
        }
        gm.charactersForHire.Remove(selectedCharacter);
        gm.staff.Add(selectedCharacter);
        ResetList();
    }
}
