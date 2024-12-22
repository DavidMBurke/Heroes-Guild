using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public Button nextTurnButton; // - 
    public Button moveButton;     // | These buttons will have fixed positions on the action bar
    public Button endMoveButton;  // |
    public Button meleeAttackButton;  // - 
    public Button rangedAttackButton; // | These attack and spell buttons are placeholders, they will be added to the action bar dynamically depending on player abilities
    public Button spell1Button;       // | 
    public Button spell2Button;       // -
    public ActionManager actionManager;

    private void Start()
    {
        actionManager = FindObjectOfType<ActionManager>();
        AddButtonListeners();
    }

    private void Update()
    {
        SetButtonsActiveState();
    }

    private void SetButtonsActiveState()
    {
        SetButtonsActiveState(); // Reset button states
        if (actionManager.currentBeing is PlayerCharacter player)
        {
            if (!player.isInCharacterAction)
            {
                SetButtonsActiveState(nextTurn: true, move: true, meleeAttack: true, rangedAttack: true, spells: true);
            }
            if (player.isInCharacterAction)
            {
                SetButtonsActiveState(nextTurn: true);
            }
            if (player.isInMovementAction)
            {
                SetButtonsActiveState(nextTurn: true, endMove: true, meleeAttack: true, rangedAttack: true, spells: true);
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
        void SetButtonsActiveState(bool nextTurn = false, bool move = false, bool endMove = false, bool meleeAttack = false, bool rangedAttack = false, bool spells = false)
        {
            nextTurnButton.gameObject.SetActive(nextTurn);
            moveButton.gameObject.SetActive(move);
            endMoveButton.gameObject.SetActive(endMove);
            meleeAttackButton.gameObject.SetActive(meleeAttack);
            rangedAttackButton.gameObject.SetActive(rangedAttack);
            spell1Button.gameObject.SetActive(spells);
            spell2Button.gameObject.SetActive(spells);
        }
    }
    private void AddButtonListeners()
    {
        nextTurnButton.onClick.AddListener(actionManager.NextTurn);
        moveButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((player, action) => Movement.Move(player, action), actionManager.currentBeing)));
        endMoveButton.onClick.AddListener(actionManager.EndMove);
        meleeAttackButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), actionManager.currentBeing)));
        rangedAttackButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 15, 10, action), actionManager.currentBeing)));
        spell1Button.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((caster, action) => Spells.Spell1Coroutine(caster), actionManager.currentBeing)));
        spell2Button.onClick.AddListener(() => actionManager. ExecuteCharacterAction(new CharacterAction((caster, action) => Spells.FireBall(caster, action), actionManager.currentBeing)));
    }
}
