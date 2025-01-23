using UnityEngine;
using UnityEngine.UI;

public class EmployeesPage : MonoBehaviour
{
    public CharacterForHirePanel panel;
    public GameObject characterListItemPrefab;
    public GameObject characterListObject;
    private PlayerCharacter selectedCharacter;
    private GuildManager gm;

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
        foreach (PlayerCharacter character in gm.employees)
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
    public void SelectCharacter(PlayerCharacter character)
    {
        selectedCharacter = character;
        panel.AssignCharacter(character);
    }
}
