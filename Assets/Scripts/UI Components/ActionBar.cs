using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Set and update buttons on the action bar 
/// </summary>
public class ActionBar : MonoBehaviour
{
    public Button endTurnButton; // - 
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
        UpdateButtons();
    }

    /// <summary>
    /// Activate or deactivate buttons based on player actions available
    /// </summary>
    private void UpdateButtons()
    {
        SetButtonsActive(); // Reset button states
        if (actionManager.currentBeing is PlayerCharacter player)
        {
            if (!player.isInCharacterAction)
            {
                SetButtonsActive(endTurn: true, move: true, meleeAttack: true, rangedAttack: true, spells: true);
            }
            if (player.isInCharacterAction)
            {
                SetButtonsActive(endTurn: true);
            }
            if (player.isInMovementAction)
            {
                SetButtonsActive(endTurn: true, endMove: true, meleeAttack: true, rangedAttack: true, spells: true);
            }
            if (!player.hasMovement && actionManager.IsTurnBasedMode())
            {
                moveButton.gameObject.SetActive(false);
            }
            if (player.actionPoints == 0 && actionManager.IsTurnBasedMode())
            {
                meleeAttackButton.gameObject.SetActive(false);
                rangedAttackButton.gameObject.SetActive(false);
                spell1Button.gameObject.SetActive(false);
                spell2Button.gameObject.SetActive(false);
            }
            if (actionManager.IsFreeMode())
            {
                endTurnButton.gameObject.SetActive(false);
                endMoveButton.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Set buttons active or inactive
    /// </summary>
    /// <param name="endTurn"></param>
    /// <param name="move"></param>
    /// <param name="endMove"></param>
    /// <param name="meleeAttack"></param>
    /// <param name="rangedAttack"></param>
    /// <param name="spells"></param>
    void SetButtonsActive(bool endTurn = false, bool move = false, bool endMove = false, bool meleeAttack = false, bool rangedAttack = false, bool spells = false)
    {
        endTurnButton.gameObject.SetActive(endTurn);
        moveButton.gameObject.SetActive(move);
        endMoveButton.gameObject.SetActive(endMove);
        meleeAttackButton.gameObject.SetActive(meleeAttack);
        rangedAttackButton.gameObject.SetActive(rangedAttack);
        spell1Button.gameObject.SetActive(spells);
        spell2Button.gameObject.SetActive(spells);
    }

    /// <summary>
    /// Apply actions to buttons
    /// </summary>
    private void AddButtonListeners()
    {
        endTurnButton.onClick.AddListener(actionManager.NextTurn);
        moveButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((player, action) => Movement.Move(player, action), actionManager.currentBeing)));
        endMoveButton.onClick.AddListener(actionManager.EndMove);
        meleeAttackButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 2, 10, action), actionManager.currentBeing)));
        rangedAttackButton.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((attacker, action) => Attack.BasicAttack(attacker, 15, 10, action), actionManager.currentBeing)));
        spell1Button.onClick.AddListener(() => actionManager.ExecuteCharacterAction(new CharacterAction((caster, action) => Spells.Spell1Coroutine(caster), actionManager.currentBeing)));
        spell2Button.onClick.AddListener(() => actionManager. ExecuteCharacterAction(new CharacterAction((caster, action) => Spells.FireBall(caster, action), actionManager.currentBeing)));
    }
}
