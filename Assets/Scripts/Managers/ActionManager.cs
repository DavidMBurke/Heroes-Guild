using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manage state related to Actions and Turn Order
/// </summary>
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

    /// <summary>
    /// Return true if in Turn Based Mode
    /// </summary>
    /// <returns></returns>
    public bool IsTurnBasedMode()
    {
        return (actionMode == ActionModes.TurnBased);
    }

    /// <summary>
    /// Return true if in Free Mode
    /// </summary>
    /// <returns></returns>
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
    

    /// <summary>
    /// Set Mode to Turn Based Mode, initialize beings and start the first turn
    /// </summary>
    private void StartTurnBasedMode()
    {
        actionMode = ActionModes.TurnBased;
        InitializeBeings();
        StartTurn();
    }

    /// <summary>
    /// Set Mode to Free Mode, switch to a live party member
    /// </summary>
    private void StartFreeMode()
    {
        currentBeing = partyManager.partyMembers.First(p => p.isAlive);
        actionMode = ActionModes.Free;
    }

    /// <summary>
    /// Set initiative values and sort beings into turn order
    /// </summary>
    private void InitializeBeings()
    {
        beings = FindObjectsOfType<Being>().ToList();
        foreach (Being being in beings)
        {
            being.isTurn = false;
            being.initiative = Random.Range(0, 100);
        }
        beings = beings.OrderBy(b => b.initiative).ToList();
        currentBeing = beings[turnIndex];
    }
    
    /// <summary>
    /// End the current turn, select the next being in turn order, and start their turn
    /// </summary>
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

    /// <summary>
    /// End the current being's turn
    /// </summary>
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

    /// <summary>
    /// Start the current being's turn
    /// </summary>
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
    }

    /// <summary>
    /// Change turns to another character (used in Free Mode)
    /// </summary>
    /// <param name="selectedPlayer"></param>
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

    /// <summary>
    /// Update endMove flag on player to true
    /// </summary>
    /// TODO - This probably isn't necessary with the endSignal flags on actions, see if it can be removed
    public void EndMove()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.endMove = true;
        }
    }

    /// <summary>
    /// End the current being's action and start a new one
    /// </summary>
    /// <param name="action"></param>
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
