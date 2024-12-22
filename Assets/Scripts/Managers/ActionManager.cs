using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public List<Being> beings;
    int turnIndex = 0;
    public Being currentBeing;
    public CameraController cameraController;
    private PartyManager partyManager;

    public enum ActionModes
    {
        TurnBased,
        Free
    }

    public ActionModes actionMode = ActionModes.Free;

    void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        currentBeing = partyManager.partyMembers.First();
    }

    private void Update()
    {
        // TODO - Lots of unneccessary list generation here, can be done once and updated at turn ends
        List<Being> enemies = FindObjectsOfType<Being>().ToList().Where(b => b.GetType() == typeof(Enemy)).ToList();
        bool enemiesAreAlive = enemies.FirstOrDefault(e => e.isAlive == true) != null;
        if (actionMode == ActionModes.Free && enemiesAreAlive)
        {
            StartTurnBasedMode();
        }
        if (actionMode == ActionModes.TurnBased && !enemiesAreAlive)
        {
            StartFreeMode();
        }
    }

    
    private void StartTurnBasedMode()
    {
        actionMode = ActionModes.TurnBased;
        InitializeBeings();
        StartTurn();
    }

    private void StartFreeMode()
    {
        actionMode = ActionModes.Free;
    }

    private void InitializeBeings()
    {
        beings = FindObjectsOfType<Being>().ToList();
        foreach (Being being in beings)
        {
            being.isTurn = false;
        }
        currentBeing = beings[turnIndex];
    }

    public void NextTurn()
    {
        EndTurn();
        turnIndex++;
        if (turnIndex >= beings.Count)
        {
            turnIndex = 0;
        }
        currentBeing = beings[turnIndex];
        StartTurn();
    }

    private void EndTurn()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.EndTurn();
        }
        if (currentBeing is Enemy enemy)
        {
            enemy.EndTurn();
        }
    }

    private void StartTurn()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartTurn();
        }
        if (currentBeing is Enemy enemy)
        {
            enemy.StartTurn(this);
        }
        cameraController.FocusOn(currentBeing.transform);
    }

    public void EndMove()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.endMove = true;
        }
    }

    public void ExecuteCharacterAction(CharacterAction action)
    {
        if (currentBeing.currentAction != null)
        {
            currentBeing.currentAction.EndAction();
        }

        currentBeing.currentAction = action;

        if (currentBeing is PlayerCharacter player)
        {
            player.StartCharacterAction(action);
        }
    }

}
