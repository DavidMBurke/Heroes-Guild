using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void Display(Interactable interactable, bool canInspect = false, bool canPickUp = false, bool canDrop = false, bool moveFirst = false)
    {
        moveBeforePickup = moveFirst;
        selectedInteractable = interactable;
        transform.position = Input.mousePosition;
        pickUpButton.gameObject.SetActive(canPickUp && interactable.canBePickedUp);
        inspectButton.gameObject.SetActive(canInspect);
        dropButton.gameObject.SetActive(canDrop);
    }

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
