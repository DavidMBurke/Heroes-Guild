using UnityEngine;
using UnityEditor;


/// <summary>
/// Add buttons to the editor to generate map in edit mode
/// </summary>
[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapGenerator mapGenerator = (MapGenerator)target;
        if (GUILayout.Button("Generate Map"))
        {
            mapGenerator.GenerateBlankMap();
        }
        if (GUILayout.Button("Generate Room"))
        {
            mapGenerator.GenerateRoom();
        }
        if (GUILayout.Button("Generate Perlin Noise"))
        {
            mapGenerator.GeneratePerlinNoise();
        }
        if (GUILayout.Button("Randomize Perlin Noise"))
        {
            mapGenerator.Reseed();
        }
        if (GUILayout.Button("Generate Border"))
        {
            mapGenerator.GenerateBorder();
        }
        if (GUILayout.Button("Conway"))
        {
            mapGenerator.Conway();
        }
        if (GUILayout.Button("Find Groups"))
        {
            mapGenerator.FindGroups();
        }
    }
}
