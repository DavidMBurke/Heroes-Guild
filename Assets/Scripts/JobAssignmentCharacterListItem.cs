using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class JobAssignmentCharacterListItem : MonoBehaviour
{
    public PlayerCharacter character = null!;
    public TextMeshProUGUI text1 = null!;
    public TextMeshProUGUI text2 = null!;
    public TextMeshProUGUI text3 = null!;

    public void SetText(string text1 = "", string text2 = "", string text3 = "")
    {
        this.text1.text = text1;
        this.text2.text = text2;
        this.text3.text = text3;
    }

    public void SetCharacter(PlayerCharacter character)
    {
        this.character = character;
    }

    public PlayerCharacter GetCharacter()
    {
        return character;
    }
}
