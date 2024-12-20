using System;
using System.Collections;

public class CharacterAction
{

    public Func<Being, IEnumerator> actionFunction;
    public Being character;

    public CharacterAction(Func<Being, IEnumerator> actionFunction, Being character)
    {
        this.actionFunction = actionFunction;
        this.character = character;
    }

}
