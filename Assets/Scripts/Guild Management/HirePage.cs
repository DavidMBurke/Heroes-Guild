using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HirePage : MonoBehaviour
{
    public CharacterForHirePanel panel;
    public GameObject characterListItemPrefab;
    public GameObject characterListObject;

    public void Start()
    {
        GuildManager.instance.GenerateCharactersForHire(10);
        foreach (PlayerCharacter character in GuildManager.instance.charactersForHire)
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
        panel.AssignCharacter(character);
    }
}
