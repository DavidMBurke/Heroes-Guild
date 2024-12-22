using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    public Being[] beings;
    int turnIndex = 0;
    public Being currentBeing;
    public CameraController cameraController;

    void Start()
    {
        InitializeBeings();
        StartTurn();
    }

    private void InitializeBeings()
    {
        beings = FindObjectsOfType<Being>();
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
        if (turnIndex >= beings.Length)
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
        cameraController.FocusOn(beings[turnIndex].transform);
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartTurn();
        }
        if (currentBeing is Enemy enemy)
        {
            enemy.StartTurn(this);
        }
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
