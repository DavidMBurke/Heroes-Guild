using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerCharacter))]
public class PlayerCharacterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PlayerCharacter character = (PlayerCharacter)target;

        if (GUILayout.Button("Log Equipment Slots"))
        {
            character.LogEquipment();
        }
    }
}
