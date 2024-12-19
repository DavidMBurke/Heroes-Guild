using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    Being[] beings;
    public Button rangedAttackButton;
    public Button nextTurnButton;
    public Button moveButton;
    public Button endMoveButton;
    public Button undoMoveButton;
    public Button meleeAttackButton;
    int turnIndex = 0;

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
    }

    public void NextTurn()
    {
        EndTurn();
        turnIndex++;
        if (turnIndex >= beings.Length)
        {
            turnIndex = 0;
        }
        StartTurn();
    }

    private void EndTurn()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.EndTurn();
        }
        if (beings[turnIndex] is Enemy enemy)
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
        if (beings[turnIndex] is Enemy enemy)
        {
            enemy.StartTurn(this);
        }
    }

    public void MakeAttack(Attack attack)
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartCombatAction(attack);
        }
    }


    public void Move()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartMovementAction();
        }
    }

    public void UndoMove()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.UndoMove();
        }
    }

    public void EndMove()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.EndMovementAction();
        }
    }
    private void SetButtonsActiveState()
    {
        SetButtonsActiveState(); // Reset 
        if (beings[turnIndex] is PlayerCharacter player)
        {
            if (!player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, move: true, meleeAttack: true, rangedAttack: true);
            }
            if (player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, endMove: true, undoMove: true, meleeAttack: true, rangedAttack: true);
            }
            if (!player.hasMovement)
            {
                moveButton.gameObject.SetActive(false);
            }
            if (player.actionPoints == 0)
            {
                meleeAttackButton.gameObject.SetActive(false);
                rangedAttackButton.gameObject.SetActive(false);
            }
        }
        void SetButtonsActiveState(bool nextTurn = false, bool move = false, bool endMove = false, bool undoMove = false, bool meleeAttack = false, bool rangedAttack = false)
        {
            nextTurnButton.gameObject.SetActive(nextTurn);
            moveButton.gameObject.SetActive(move);
            endMoveButton.gameObject.SetActive(endMove);
            undoMoveButton.gameObject.SetActive(undoMove);
            meleeAttackButton.gameObject.SetActive(meleeAttack);
            rangedAttackButton.gameObject.SetActive(rangedAttack);
        }
    }
    private void AddButtonListeners()
    {
        nextTurnButton.onClick.AddListener(NextTurn);
        moveButton.onClick.AddListener(Move);
        meleeAttackButton.onClick.AddListener(() => MakeAttack(Attack.basicMeleeAttack));
        rangedAttackButton.onClick.AddListener(() => MakeAttack(Attack.basicRangedAttack));
        undoMoveButton.onClick.AddListener(UndoMove);
        endMoveButton.onClick.AddListener(EndMove);
    }


}
