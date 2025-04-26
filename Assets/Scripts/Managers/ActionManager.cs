using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages combat and exploration states, handling turn-based and free action modes.
/// </summary>
public class ActionManager : MonoBehaviour
{
    public static ActionManager instance = null!;
    public List<Being> beings = null!;
    public Being currentBeing = null!;
    public CameraController cameraController = null!;
    private int turnIndex = 0;
    private PartyManager partyManager = null!;

    public delegate void PlayerSelected(PlayerCharacter newPlayer);
    public event PlayerSelected OnPlayerSelected = null!;

    public enum ActionModes
    {
        TurnBased,
        Free
    }

    private ActionModes actionMode = ActionModes.Free;

    // ========== Unity Lifecycle ==========

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        partyManager = FindObjectOfType<PartyManager>();
        SelectCharacter(partyManager.partyMembers.FirstOrDefault());
    }

    private void Update()
    {
        // Transition to turn-based mode if enemies are aware of players
        List<Enemy> enemies = FindObjectsOfType<Being>()
            .OfType<Enemy>()
            .Where(e => e.isAwareOfPlayers)
            .ToList();

        bool enemiesAreAlive = enemies.Any(e => e.isAlive);

        if (actionMode == ActionModes.Free && enemiesAreAlive)
        {
            StartTurnBasedMode();
        }
        else if (actionMode == ActionModes.TurnBased && !enemiesAreAlive)
        {
            StartFreeMode();
        }

        // Right click interaction trigger
        if (Input.GetMouseButtonDown(1) && !UIManager.CheckForUIElement())
        {
            ExecuteCharacterAction(new CharacterAction((player, action) =>
                Interaction.InteractWithWorldItem(player, action), currentBeing));
        }
    }

    // ========== Mode Checks ==========

    /// <summary>
    /// Returns whether the game is currently in turn-based mode.
    /// </summary>
    public bool IsTurnBasedMode() => actionMode == ActionModes.TurnBased;

    /// <summary>
    /// Returns whether the game is currently in free movement mode.
    /// </summary>
    public bool IsFreeMode() => actionMode == ActionModes.Free;

    // ========== Mode Transitions ==========

    /// <summary>
    /// Switches to turn-based mode, initializes characters, and starts the turn cycle.
    /// </summary>
    private void StartTurnBasedMode()
    {
        PartyManager.instance.ClearCharactersFromMovementGroup();
        actionMode = ActionModes.TurnBased;
        InitializeBeings();
        StartTurn();
    }

    /// <summary>
    /// Switches to free mode, selecting the first available party member.
    /// </summary>
    private void StartFreeMode()
    {
        currentBeing = partyManager.partyMembers.First(p => p.isAlive);
        if (currentBeing is PlayerCharacter player)
        {
            OnPlayerSelected?.Invoke(player);
        }
        actionMode = ActionModes.Free;
    }

    /// <summary>
    /// Initializes the list of beings and determines turn order based on initiative.
    /// </summary>
    private void InitializeBeings()
    {
        beings = new List<Being>();

        // Add live players
        var players = FindObjectsOfType<Being>()
            .Where(b => b.isAlive && b.GetType() == typeof(PlayerCharacter))
            .ToList();
        beings.AddRange(players);

        // Add live and aware enemies
        var engagedEnemies = FindObjectsOfType<Being>()
            .OfType<Enemy>()
            .Where(e => e.isAlive && e.isAwareOfPlayers)
            .Cast<Being>()
            .ToList();
        beings.AddRange(engagedEnemies);

        // Set initiative and sort turn order
        foreach (Being being in beings)
        {
            being.isTurn = false;
            being.initiative = Random.Range(0, 100);
        }

        beings = beings.OrderBy(b => b.initiative).ToList();
        currentBeing = beings[turnIndex];
    }

    // ========== Turn Management ==========

    /// <summary>
    /// Ends the current turn and starts the next being's turn in order.
    /// </summary>
    public void NextTurn()
    {
        EndTurn();
        turnIndex = (turnIndex + 1) % beings.Count;
        currentBeing = beings[turnIndex];
        StartTurn();
    }

    /// <summary>
    /// Ends the turn for the current being.
    /// </summary>
    private void EndTurn()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.EndTurn();
        }
        else if (currentBeing is Enemy enemy)
        {
            enemy.EndTurn();
        }
    }

    /// <summary>
    /// Starts the turn for the current being.
    /// </summary>
    private void StartTurn()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartTurn();
            OnPlayerSelected?.Invoke(player);
        }
        else if (currentBeing is Enemy enemy)
        {
            enemy.StartTurn();
        }
    }

    // ========== Character Control ==========

    /// <summary>
    /// Selects a new player-controlled character in free mode.
    /// </summary>
    public void SelectCharacter(PlayerCharacter selectedPlayer)
    {
        if (currentBeing is PlayerCharacter previousPlayer)
        {
            previousPlayer.endMove = true;
            previousPlayer.isInMovementAction = false;
        }

        EndTurn();
        currentBeing = selectedPlayer;
        selectedPlayer.StartTurn();
        OnPlayerSelected?.Invoke(selectedPlayer);
    }

    /// <summary>
    /// Marks the current player's move as ended.
    /// </summary>
    public void EndMove()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.endMove = true;
        }
    }

    /// <summary>
    /// Executes a new character action, replacing any currently running one.
    /// </summary>
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
