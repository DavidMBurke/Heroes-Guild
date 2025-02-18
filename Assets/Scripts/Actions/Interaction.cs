using System.Collections;
using UnityEngine;

/// <summary>
/// Interaction functions to be passed in as coroutines
/// </summary>
public class Interaction
{
    /// <summary>
    /// Bring up the interaction popup for items in the world (non-UI items)
    /// </summary>
    /// <param name="being"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerator InteractWithWorldItem(Being being, CharacterAction action) {
        InteractableDisplay interactableDisplay = Object.FindObjectOfType<InteractableDisplay>();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Interactable", "Attackable")))
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

    /// <summary>
    /// Bring up interaction popup for item in the inventory
    /// </summary>
    /// <param name="being"></param>
    /// <param name="action"></param>
    /// <param name="interactable"></param>
    /// <returns></returns>
    public static IEnumerator InteractWithInventoryItem(Being being, CharacterAction action, Interactable interactable)
    {
        InteractableDisplay interactableDisplay = Object.FindObjectOfType<InteractableDisplay>();
        interactableDisplay.gameObject.SetActive(true);
        interactableDisplay.Display(interactable, canInspect: true, canDrop: true);
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

    /// <summary>
    /// Move being to an item and take it from world to being inventory once in pickup range
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="being"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Move an item from world to being inventory
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="being"></param>
    public static void PickUp(Interactable interactable, Being being)
    {
        if (being is PlayerCharacter player)
        {
            //player.AddToInventory(interactable);
            interactable.gameObject.SetActive(false);
            return;
        }
        //being.inventory.Add(interactable);
        interactable.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Remove item from being inventory and drop it on the ground around them
    /// </summary>
    /// <param name="interactable"></param>
    /// <param name="being"></param>
    public static void Drop(Interactable interactable, Being being)
    {
        if (being is PlayerCharacter player)
        {
            //player.RemoveFromInventory(interactable);
        }
        if (being is Enemy enemy)
        {
            //enemy.inventory.Remove(interactable);
        }
        interactable.gameObject.SetActive(true);
        Vector3 p = being.gameObject.transform.position;
        p.x += Random.Range(-being.interactDistance, being.interactDistance);
        p.z += Random.Range(-being.interactDistance, being.interactDistance);
        interactable.gameObject.transform.position = p;
    }

    /// <summary>
    /// TBI - Currently puts description in console, will show in UI in a way TBD
    /// </summary>
    /// <param name="interactable"></param>
    public static void Inspect(Interactable interactable)
    {
        Debug.Log(interactable.description);
    }
}
