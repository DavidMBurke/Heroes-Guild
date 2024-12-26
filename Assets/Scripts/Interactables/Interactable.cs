using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows access for right clicking and selecting options of interaction. Most things in the game will have this
/// </summary>
public class Interactable : MonoBehaviour
{
    public bool canBePickedUp;
    public float weight = 0;
    public string description = string.Empty;
    public Sprite sprite;
}
