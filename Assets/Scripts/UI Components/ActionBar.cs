using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private List<Button> playerActionButtons = new List<Button>();
    public ActionButtons actionButtons;
    public Button buttonPrefab;

    private void Start()
    {
        ActionManager.instance.OnPlayerSelected += HandlePlayerSelected;
        actionButtons = GetComponentInChildren<ActionButtons>();

        if (actionButtons == null)
        {
            Debug.LogError("actionButtons null in ActionBar.Start");
        }

        if (ActionManager.instance.currentBeing is PlayerCharacter player)
        {
            HandlePlayerSelected(player);
        }

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
        if (ActionManager.instance.currentBeing is PlayerCharacter player)
        {
            if (!player.isInCharacterAction)
            {
                SetButtonsActive(endTurn: true, move: true, playerActions: true);
            }
            if (player.isInCharacterAction)
            {
                SetButtonsActive(endTurn: true);
            }
            if (player.isInMovementAction)
            {
                endMoveButton.gameObject.SetActive(true);
                moveButton.gameObject.SetActive(false);
            }
            if (!player.hasMovement && ActionManager.instance.IsTurnBasedMode())
            {
                moveButton.gameObject.SetActive(false);
            }
            if (player.actionPoints == 0 && ActionManager.instance.IsTurnBasedMode())
            {
                foreach (Button button in playerActionButtons)
                {
                    button.gameObject.SetActive(false);
                }
            }
            if (ActionManager.instance.IsFreeMode())
            {
                endTurnButton.gameObject.SetActive(false);
            }
        }
    }

    private void PopulateCharacterActionButtons(PlayerCharacter player)
    {
        if (actionButtons == null)
        {
            Debug.LogWarning("actionButtons null in PopulateCharacterActionButtons");
            return;
        }

        ClearActionButtons();

        foreach (CharacterAction action in player.actionList)
        {

            Button newButton = Instantiate(buttonPrefab, parent: actionButtons.gameObject.transform);
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = action.actionName;
            newButton.onClick.AddListener(() => ActionManager.instance.ExecuteCharacterAction(action));
            playerActionButtons.Add(newButton);
        }
    }

    private void ClearActionButtons()
    {
        foreach (Button button in playerActionButtons)
        {
            Destroy(button.gameObject);
        }
        playerActionButtons.Clear();
    }

    /// <summary>
    /// Set buttons active or inactive
    /// </summary>
    /// <param name="endTurn"></param>
    /// <param name="move"></param>
    /// <param name="endMove"></param>
    void SetButtonsActive(bool endTurn = false, bool move = false, bool endMove = false, bool playerActions = false)
    {
        endTurnButton.gameObject.SetActive(endTurn);
        moveButton.gameObject.SetActive(move);
        endMoveButton.gameObject.SetActive(endMove);
        foreach (Button button in playerActionButtons)
        {
            button.gameObject.SetActive(playerActions);
        }
    }

    /// <summary>
    /// Apply actions to buttons
    /// </summary>
    private void AddButtonListeners()
    {
        endTurnButton.onClick.AddListener(ActionManager.instance.NextTurn);
        moveButton.onClick.AddListener(() => ActionManager.instance.ExecuteCharacterAction(new CharacterAction((player, action) => Movement.Move(player, action), ActionManager.instance.currentBeing, "Move")));
        endMoveButton.onClick.AddListener(ActionManager.instance.EndMove);
    }

    private void HandlePlayerSelected(PlayerCharacter player)
    {
        if (player == null)
        {
            Debug.LogWarning("null player in HandlePlayerSelectied");
            return;
        }
        PopulateCharacterActionButtons(player);
    }
}
