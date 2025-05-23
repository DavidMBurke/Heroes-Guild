using System.Collections;
using UnityEngine;

/// <summary>
/// Collection of attack functions to be executed as coroutines.
/// </summary>
public class Attack
{
    /// <summary>
    /// Performs a basic ranged attack on an enemy within range.
    /// Displays the attack range, waits for a valid target click, applies damage, and deducts action points.
    /// </summary>
    /// <param name="attacker">The being initiating the attack.</param>
    /// <param name="range">The maximum distance the attack can reach.</param>
    /// <param name="damage">The amount of health the attack removes from the target.</param>
    /// <param name="action">The action context for coroutine management.</param>
    /// <returns>An IEnumerator coroutine used for execution in Unity.</returns>
    public static IEnumerator BasicAttack(Being attacker, float range, CombatSkills.Enum toHitSkill, Attributes.Enum damageAttribute, CharacterAction action)
    {
        bool inAttack = true;
        Vector3 scale = attacker.rangeIndicator.transform.localScale;
        attacker.isInCharacterAction = true;
        attacker.rangeIndicatorColor = attacker.rangeIndicatorCombatColor;

        while (inAttack && !action.endSignal)
        {
            attacker.rangeIndicator.SetActive(true);
            attacker.rangeIndicator.transform.localScale = new Vector3(range * 2, scale.y, range * 2);

            if (Input.GetMouseButtonDown(0))
            {
                UIManager.CheckForUIElement();

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Attackable")))
                {
                    yield return null;
                    continue;
                }

                Being target = hit.collider.GetComponentInParent<Being>();
                if (target == null)
                {
                    yield return null;
                    continue;
                }

                float distanceToTarget = Vector3.Distance(target.transform.position, attacker.transform.position);
                if (target == attacker || distanceToTarget > range)
                {
                    yield return null;
                    continue;
                }
                DiceRoll toHitDice = DiceRoll.StandardRoll((int)attacker.combatSkills.GetSkill(toHitSkill).modifiedLevel);
                DiceRoll damageDice = new(1, 10, (int)attacker.attributes.GetAttribute(damageAttribute).modifiedLevel);
                Debug.Log($"{attacker.characterName} rolled {toHitDice.GetResult()} ({toHitDice.GetResultText()}) to hit.");

                bool successfulHit = target.AttemptAttackOnThisBeing(toHitDice, damageDice);

                if (attacker is PlayerCharacter player)
                {
                    player.actionPoints -= 1;
                    break;
                }
            }

            if (Input.GetMouseButtonDown(1) || action.endSignal)
            {
                inAttack = false;
            }

            yield return null;
        }

        attacker.rangeIndicator.SetActive(false);
    }

    public static IEnumerator BasicAutoAttack(Being attacker, Being target, float range)
    {
        attacker.isInCharacterAction = true;

        if (target == null || !target.isAlive)
        {
            Debug.LogWarning("Target is null or dead");
            yield break;
        }

        float distanceToTarget = Vector3.Distance(attacker.transform.position, target.transform.position);
        if (distanceToTarget > range)
        {
            Debug.Log($"{attacker.characterName} is out of range for attack ");
            yield break;
        }

        DiceRoll toHitDice = DiceRoll.StandardRoll((int)attacker.combatSkills.GetSkill(CombatSkills.Enum.Melee).modifiedLevel);
        DiceRoll damageDice = new(1, 10, (int)attacker.attributes.GetAttribute(Attributes.Enum.Strength).modifiedLevel);
        Debug.Log($"{attacker.characterName} auto-attacks {target.characterName} with roll {toHitDice.GetResult()}({toHitDice.GetResultText()})");

        bool hitSuccess = target.AttemptAttackOnThisBeing(toHitDice, damageDice);

        yield return new WaitForSeconds(0.5f);

        attacker.actionPoints -= 1;
        
    }
}
