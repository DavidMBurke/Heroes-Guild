using System;
using System.Collections;

/// <summary>
/// Class to hold info for action coroutines, being doing the action, and an end signal to end the action from outside the coroutine
/// </summary>
public class CharacterAction
{

    /// <summary>
    /// coroutine
    /// </summary>
    public Func<Being, CharacterAction, IEnumerator> action;

    /// <summary>
    /// Being performing the action
    /// </summary>
    public Being character;

    /// <summary>
    /// Name displayed on action bar / menus
    /// </summary>
    public string actionName;

    /// <summary>
    /// Action description
    /// </summary>
    public string description;

    /// <summary>
    /// flag to end coroutine from inside or outside the function
    /// </summary>
    public bool endSignal = false;

    /// <summary>
    /// Create a character action to pass as a coroutine 
    /// </summary>
    /// <param name="action"></param>
    /// <param name="character"></param>
    /// <param name="actionName"></param>
    public CharacterAction(Func<Being, CharacterAction, IEnumerator> action, Being character, string actionName = "", string description = "")
    {
        this.action = action;
        this.character = character;
        this.actionName = actionName;
        this.description = description;
    }

    /// <summary>
    /// End action by setting endSignal to true
    /// </summary>
    public void EndAction()
    {
        endSignal = true;
    }
}
