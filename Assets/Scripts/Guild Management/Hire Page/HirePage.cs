using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HirePage : MonoBehaviour
{
    public CharacterSummaryPanel panel = null!;
    public GameObject characterListItemPrefab = null!;
    public GameObject characterList = null!;
    public GameObject hireButton = null!;
    private PlayerCharacter selectedCharacter = null!;
    private GuildManager gm = null!;
    public TextMeshProUGUI costToHire = null!;
    private int hiringFeeMultiplier = 30; //multiplies by daily salary
    public GameObject noEmployeesForHireText = null!;


    public void Start()
    {
        gm = GuildManager.instance;
        gm.GenerateCharactersForHire(10);
        ResetList();
        hireButton.SetActive(false);
    }

    public void ResetList()
    {
        gm.charactersForHire.RemoveAll(character => character == null);
        foreach (Transform child in characterList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (PlayerCharacter character in gm.charactersForHire)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterList.transform);
            CharacterListItem listItem = characterListItemObject.GetComponent<CharacterListItem>();
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                SelectCharacter(character);
            });
            listItem.SetCharacter(character);
            if (character == selectedCharacter)
            {
                listItem.SetHighlight();
            }
            listItem.jobDropdown.gameObject.SetActive(false);
        }
        noEmployeesForHireText.SetActive(gm.charactersForHire.Count == 0);
    }

    private void Update()
    {
        hireButton.SetActive(panel.character != null);
        costToHire.gameObject.SetActive(panel.character != null);
    }

    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        panel.AssignCharacter(character);
        costToHire.text = $"Cost: {hiringFeeMultiplier * character.salary} (+{character.salary}/day)";
        ResetList();
    }

    public void HireSelectedCharacter()
    {
        if (gm.coin < (hiringFeeMultiplier * selectedCharacter.salary))
        {
            AlertManager.instance.ShowAlert("Cannot afford");
            return;
        }
        gm.coin -= (hiringFeeMultiplier * selectedCharacter.salary);
        gm.charactersForHire.Remove(selectedCharacter);
        gm.unassignedEmployees.Add(selectedCharacter);
        ResetList();
        GuildManager.instance.CalculateDailyExpenses();
    }
}
