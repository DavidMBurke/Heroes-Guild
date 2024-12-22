using System;
using System.Collections;

public class CharacterAction
{

    public Func<Being, CharacterAction, IEnumerator> action;

    public Being character;
    public bool endSignal = false;

    public CharacterAction(Func<Being, CharacterAction, IEnumerator> action, Being character)
    {
        this.action = action;
        this.character = character;
    }

    public void EndAction()
    {
        endSignal = true;
    }
}
