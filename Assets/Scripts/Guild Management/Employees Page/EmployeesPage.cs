using UnityEngine;
using UnityEngine.UI;

public class EmployeesPage : MonoBehaviour
{
    public CharacterForHirePanel panel = null!;
    public GameObject characterListItemPrefab = null!;
    public GameObject characterListObject = null!;
    private PlayerCharacter selectedCharacter = null!;
    private GuildManager gm = null!;

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
        foreach (PlayerCharacter character in gm.unassignedEmployees)
        {
            GameObject characterListItemObject = Instantiate(characterListItemPrefab, characterListObject.transform);
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
        panel.AssignCharacter(character);
    }
}
