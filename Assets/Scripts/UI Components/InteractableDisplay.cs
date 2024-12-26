using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Pop up for items when right clicked to provide available options
/// </summary>
public class InteractableDisplay : MonoBehaviour
{
    private List<TextMeshProUGUI> textComponents;
    private List<Button> buttonComponents;
    private TextMeshProUGUI itemName;
    private Interactable selectedInteractable;
    private Button pickUpButton;
    private Button inspectButton;
    private Button dropButton;
    public Vector3 offScreenPosition;
    private bool moveBeforePickup;

    void Start()
    {
        textComponents = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        buttonComponents = GetComponentsInChildren<Button>().ToList();
        itemName = textComponents.FirstOrDefault(t => t.name == "Interactable Name");
        pickUpButton = buttonComponents.FirstOrDefault(b => b.name == "Pick Up Button");
        inspectButton = buttonComponents.FirstOrDefault(b => b.name == "Inspect Button");
        dropButton = buttonComponents.FirstOrDefault(b => b.name == "Drop Button");
        pickUpButton.onClick.AddListener(() => Pickup(moveBeforePickup));
        inspectButton.onClick.AddListener(() => Interaction.Inspect(selectedInteractable));
        dropButton.onClick.AddListener(() => Interaction.Drop(selectedInteractable, ActionManager.instance.currentBeing));
        Vector3 offScreenPosition = new Vector3(-500, -500, 0);
    }

    void Update()
    {
        if (selectedInteractable == null)
        {
            return;
        }
        itemName.text = selectedInteractable.name;
    }

    /// <summary>
    /// Move the popup to the cursor position and include the applicable options
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="canInspect"></param>
    /// <param name="canPickUp"></param>
    /// <param name="canDrop"></param>
    /// <param name="moveFirst"></param>
    public void Display(Interactable interactable, bool canInspect = false, bool canPickUp = false, bool canDrop = false, bool moveFirst = false)
    {
        moveBeforePickup = moveFirst;
        selectedInteractable = interactable;
        transform.position = Input.mousePosition;
        pickUpButton.gameObject.SetActive(canPickUp && interactable.canBePickedUp);
        inspectButton.gameObject.SetActive(canInspect);
        dropButton.gameObject.SetActive(canDrop);
    }

    /// <summary>
    /// Pick up interactable. If allowed, move into range of the item before picking up.
    /// </summary>
    /// <param name="moveFirst"></param>
    private void Pickup(bool moveFirst = false)
    {
        if (moveFirst)
        {
            StartCoroutine(Interaction.MoveAndPickUp(selectedInteractable, ActionManager.instance.currentBeing));
            return;
        }
        Interaction.PickUp(selectedInteractable, ActionManager.instance.currentBeing);
    }
}
