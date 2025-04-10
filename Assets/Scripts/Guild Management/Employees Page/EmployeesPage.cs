using UnityEngine;
using UnityEngine.UI;

public class EmployeesPage : MonoBehaviour
{
    public CharacterSummaryPanel panel = null!;
    public GameObject characterListItemPrefab = null!;
    public GameObject characterListObject = null!;
    private PlayerCharacter selectedCharacter = null!;
    private GuildManager gm = null!;
    public GameObject noEmployeesText = null!;

    public void Start()
    {
        gm = GuildManager.instance;
        ResetList();
    }

    private void OnEnable()
    {
        ResetList();
    }
    private void ResetList()
    {
        if (gm == null)
        {
            return;
        }
        foreach (Transform child in characterListObject.transform)
        {
            Destroy(child.gameObject);
        }
        int employeeCount = 0;
        foreach (var group in gm.workerGroups)
        {
            foreach (PlayerCharacter character in group.Value)
            {
                employeeCount++;
                GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterListObject.transform);
                CharacterListItem listItem = characterListItemObject.GetComponent<CharacterListItem>();
                Button button = listItem.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    SelectCharacter(character);
                });
                listItem.canAssignJobs = true;
                listItem.SetCharacter(character);
                if (character == selectedCharacter)
                {
                    listItem.SetHighlight();
                }
                listItem.UpdateDisplayInfo();
            }
        }
        noEmployeesText.gameObject.SetActive(employeeCount == 0);
    }
    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        panel.AssignCharacter(character);
        ResetList();
    }
}
