using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    Being[] beings;
    public Button nextTurnButton;
    public Button moveButton;
    public Button endMoveButton;
    public Button undoMoveButton;
    public Button attackButton;
    int turnIndex = 0;

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
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartTurn();
        }
        if (beings[turnIndex] is Enemy enemy)
        {
            enemy.StartTurn(this);
        }
    }

    public void Attack()
    {
        if (beings[turnIndex] is PlayerCharacter player)
        {
            player.StartCombatAction();
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
                SetButtonsActiveState(nextTurn: true, move: true, attack: true);
            }
            if (player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, endMove: true, undoMove: true, attack: true);
            }
            if (!player.hasMovement)
            {
                moveButton.gameObject.SetActive(false);
            }
            if (player.actionPoints == 0)
            {
                attackButton.gameObject.SetActive(false);
            }
        }
        void SetButtonsActiveState(bool nextTurn = false, bool move = false, bool endMove = false, bool undoMove = false, bool attack = false)
        {
            nextTurnButton.gameObject.SetActive(nextTurn);
            moveButton.gameObject.SetActive(move);
            endMoveButton.gameObject.SetActive(endMove);
            undoMoveButton.gameObject.SetActive(undoMove);
            attackButton.gameObject.SetActive(attack);
        }
    }
    private void AddButtonListeners()
    {
        nextTurnButton.onClick.AddListener(NextTurn);
        moveButton.onClick.AddListener(Move);
        attackButton.onClick.AddListener(Attack);
        undoMoveButton.onClick.AddListener(UndoMove);
        endMoveButton.onClick.AddListener(EndMove);
    }


}
