using System;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    Being[] beings;
    public Button nextTurnButton;
    public Button moveButton;
    int beingsTurnIndex = 0;

    void Start()
    {
        beings = FindObjectsOfType<Being>();
        foreach (Being being in beings)
        {
            being.isTurn = false;
        }
        beings[beingsTurnIndex].isTurn = true;

        nextTurnButton.onClick.AddListener(NextTurn);
        moveButton.onClick.AddListener(Move);
    }

    private void Update()
    {
        if (beings[beingsTurnIndex] is PlayerCharacter player)
        {
            if (!player.hasMovement)
            {
                moveButton.gameObject.SetActive(false);
            }
        }
    }

    public void NextTurn()
    {
        if (beings[beingsTurnIndex] is PlayerCharacter lastPlayer)
        {
            lastPlayer.EndTurn();
        }
        if (beings[beingsTurnIndex] is Enemy lastEnemy)
        {
            lastEnemy.EndTurn();
        }
        beingsTurnIndex++;
        if (beingsTurnIndex >= beings.Length)
        {
            beingsTurnIndex = 0;
        }
        if (beings[beingsTurnIndex] is PlayerCharacter player)
        {
            SetButtonsActive(true);
            player.StartTurn();
        }
        if (beings[beingsTurnIndex] is Enemy enemy)
        {
            SetButtonsActive(false);
            enemy.StartTurn(this);
        }
    }

    private void SetButtonsActive(bool x)
    {
        nextTurnButton.gameObject.SetActive(x);
        moveButton.gameObject.SetActive(x);
    }

    public void Move()
    {
        if (beings[beingsTurnIndex] is PlayerCharacter player)
        {
            player.StartMovementAction();
        }

    }
}
