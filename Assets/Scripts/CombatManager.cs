using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    Being[] beings;
    public Button nextTurnButton; // - 
    public Button moveButton;     // | These buttons will have fixed positions on the action bar
    public Button endMoveButton;  // |
    public Button undoMoveButton; // -
    public Button meleeAttackButton;  // - 
    public Button rangedAttackButton; // | These attack and spell buttons are placeholders, they will be added to the action bar dynamically depending on player abilities
    public Button spell1Button;       // | 
    public Button spell2Button;       // -
    int turnIndex = 0;
    private Being currentBeing;

    public CameraController cameraController;

    void Start()
    {
        InitializeBeings();
        AddButtonListeners();
        StartTurn();
    }

    private void Update()
    {
        SetButtonsActiveState();
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

    public void Move()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.StartMovementAction();
        }
    }

    public void UndoMove()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.UndoMove();
        }
    }

    public void EndMove()
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.EndMovementAction();
        }
    }

    public void ExecuteCharacterAction(CharacterAction action)
    {
        if (currentBeing is PlayerCharacter player)
        {
            player.StartCharacterAction(action);
        }
    }

    private void SetButtonsActiveState()
    {
        SetButtonsActiveState(); // Reset button states
        if (currentBeing is PlayerCharacter player)
        {
            if (!player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, move: true, meleeAttack: true, rangedAttack: true, spells: true);
            }
            if (player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, endMove: true, undoMove: true, meleeAttack: true, rangedAttack: true, spells: true);
            }
            if (!player.hasMovement)
            {
                moveButton.gameObject.SetActive(false);
            }
            if (player.actionPoints == 0)
            {
                meleeAttackButton.gameObject.SetActive(false);
                rangedAttackButton.gameObject.SetActive(false);
                spell1Button.gameObject.SetActive(false);
                spell2Button.gameObject.SetActive(false);
            }
        }
        void SetButtonsActiveState(bool nextTurn = false, bool move = false, bool endMove = false, bool undoMove = false, bool meleeAttack = false, bool rangedAttack = false, bool spells = false)
        {
            nextTurnButton.gameObject.SetActive(nextTurn);
            moveButton.gameObject.SetActive(move);
            endMoveButton.gameObject.SetActive(endMove);
            undoMoveButton.gameObject.SetActive(undoMove);
            meleeAttackButton.gameObject.SetActive(meleeAttack);
            rangedAttackButton.gameObject.SetActive(rangedAttack);
            spell1Button.gameObject.SetActive(spells);
            spell2Button.gameObject.SetActive(spells);

        }
    }
    private void AddButtonListeners()
    {
        nextTurnButton.onClick.AddListener(NextTurn);
        moveButton.onClick.AddListener(Move);
        meleeAttackButton.onClick.AddListener(() => ExecuteCharacterAction(new CharacterAction((attacker) => Attack.BasicAttack(attacker, 2, 10), currentBeing)));
        rangedAttackButton.onClick.AddListener(() => ExecuteCharacterAction(new CharacterAction((attacker) => Attack.BasicAttack(attacker, 15, 10), currentBeing)));
        undoMoveButton.onClick.AddListener(UndoMove);
        endMoveButton.onClick.AddListener(EndMove);
        spell1Button.onClick.AddListener(() => ExecuteCharacterAction(new CharacterAction((caster) => Spells.Spell1Coroutine(caster), currentBeing)));
        spell2Button.onClick.AddListener(() => ExecuteCharacterAction(new CharacterAction((caster) => Spells.FireBall(caster), currentBeing)));
    }


}
