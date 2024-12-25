using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager instance;
    public List<Being> beings;
    public Being currentBeing;
    public CameraController cameraController;
    int turnIndex = 0;
    PartyManager partyManager;

    public delegate void PlayerSelected(PlayerCharacter newPlayer);
    public event PlayerSelected OnPlayerSelected;

    public enum ActionModes
    {
        TurnBased,
        Free
    }

    private ActionModes actionMode = ActionModes.Free;

    public bool IsTurnBasedMode()
    {
        return (actionMode == ActionModes.TurnBased);
    }

    public bool IsFreeMode()
    {
        return (actionMode == ActionModes.Free);
    }

    void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        SelectCharacter(partyManager.partyMembers.FirstOrDefault());
    }

    private void Awake()
    {
        instance = this;
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
        if (Input.GetMouseButtonDown(1) && !UIManager.CheckForUIElement()) {
            ExecuteCharacterAction(new CharacterAction((player, action) => Interaction.InteractWithWorldItem(player, action), currentBeing));
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
        currentBeing = partyManager.partyMembers.First(p => p.isAlive);
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

    public void SelectCharacter(PlayerCharacter selectedPlayer)
    {
        if (selectedPlayer == currentBeing)
        {
            return;
        }
        EndTurn();
        currentBeing = selectedPlayer;
        selectedPlayer.StartTurn();

        if (currentBeing is PlayerCharacter player)
        {
            OnPlayerSelected?.Invoke(player);
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
