using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerCharacter : Being
{
    public int maxActionPoints = 1;
    public int actionPoints = 0;
    public bool hasMovement = true;
    public bool endMove = false; // Used to signal move action to end mid-movement.

    public delegate void InventoryUpdated();
    public event InventoryUpdated OnInventoryUpdated;
    private List<Interactable> _inventory = new List<Interactable>();
    public new List<Interactable> inventory
    {
        get => _inventory;
        set
        {
            _inventory = value;
            OnInventoryUpdated?.Invoke();
        }
    }

    new void Start()
    {
        base.Start();
    }

    private new void Update()
    {
        base.Update();
    }

    public void StartTurn()
    {
        isTurn = true;
        hasMovement = true;
        startingPosition = transform.position;
        actionPoints = maxActionPoints;
    }

    public void EndTurn()
    {
        isTurn = false;
        hasMovement = false;
        if (isInCharacterAction)
        {
            EndCharacterAction();
        }
    }

    public void StartCharacterAction(CharacterAction action)
    {
        if (isInCharacterAction)
        {
            EndCharacterAction();
        }
        StartCoroutine(action.action(action.character, action));
    }

    public void EndCharacterAction()
    {
        isInCharacterAction = false;
        currentAction.EndAction();
    }

    new private void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void AddToInventory(Interactable item)
    {
        if (item == null)
        {
            return;
        }
        inventory.Add(item);
        OnInventoryUpdated?.Invoke();
    }

    public void RemoveFromInventory(Interactable item)
    {
        if (inventory.Remove(item))
        {
            OnInventoryUpdated?.Invoke();
            return;
        }
    }

}
