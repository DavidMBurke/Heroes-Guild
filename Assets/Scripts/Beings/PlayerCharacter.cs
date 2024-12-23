using UnityEngine;

public class PlayerCharacter : Being
{
    public int maxActionPoints = 1;
    public int actionPoints = 0;
    public bool hasMovement = true;
    public bool endMove = false; // Used to signal move action to end mid-movement.

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

}
