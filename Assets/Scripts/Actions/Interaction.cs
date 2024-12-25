using System.Collections;
using UnityEngine;


public class Interaction
{
    public static IEnumerator Interact(Being being, CharacterAction action) {
        InteractableDisplay interactableDisplay = Object.FindObjectOfType<InteractableDisplay>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Interactable")))
        {
            interactableDisplay.gameObject.transform.position = interactableDisplay.offScreenPosition;
            yield break;
        }

        interactableDisplay.gameObject.SetActive(true);
        Interactable interactable = hit.collider.GetComponentInParent<Interactable>();
        float distanceToTarget = Vector3.Distance(being.transform.position, interactable.transform.position);
        if (distanceToTarget > being.interactDistance && ActionManager.instance.IsFreeMode())
        {
            interactableDisplay.Display(interactable, canInspect: true, canPickUp: true, moveFirst: true);
            yield return null;
        }
        if (distanceToTarget > being.interactDistance && ActionManager.instance.IsTurnBasedMode())
        {
            interactableDisplay.Display(interactable, canInspect: true);
            yield return null;
        }
        if (distanceToTarget <= being.interactDistance) {
            interactableDisplay.Display(interactable, canInspect: true, canPickUp: true);
            yield return null;
        }
        while (action.endSignal == false)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Escape) || !being.isTurn)
            {
                action.EndAction();
            }
            yield return null;
        }
        interactableDisplay.gameObject.transform.position = interactableDisplay.offScreenPosition;
        yield break;
    }

    public static IEnumerator MoveAndPickUp(Interactable interactable, Being being)
    {
        float distanceToTarget = Vector3.Distance(interactable.transform.position, being.transform.position);    
        float timer = 5;
        while (timer > 0 && distanceToTarget > being.interactDistance && !Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Escape))
        {
            being.transform.position = Vector3.MoveTowards(being.transform.position, interactable.transform.position, being.moveSpeed * Time.deltaTime);
            timer -= Time.deltaTime;
            distanceToTarget = Vector3.Distance(interactable.transform.position, being.transform.position);
            yield return null;
        }
        PickUp(interactable, being);
    }

    public static void PickUp(Interactable interactable, Being being)
    {
        if (being is PlayerCharacter player)
        {
            player.AddToInventory(interactable);
            interactable.gameObject.SetActive(false);
            return;
        }
        being.inventory.Add(interactable);
        interactable.gameObject.SetActive(false);
    }

    public static void Drop(Interactable interactable, Being being)
    {
        interactable.gameObject.SetActive(true);
        Vector3 p = being.gameObject.transform.position;
        p.x += Random.Range(-being.interactDistance, being.interactDistance);
        p.z += Random.Range(-being.interactDistance, being.interactDistance);
        interactable.gameObject.transform.position = p;
    }

    public static void Inspect(Interactable interactable)
    {
        Debug.Log(interactable.description);
    }
}
