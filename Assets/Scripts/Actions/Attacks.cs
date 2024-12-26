

using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Collection of Attack functions to be passed in as coroutines
/// </summary>
public class Attack
{
    /// <summary>
    /// Basic attack that does damage at a range
    /// </summary>
    /// <param name="attacker"></param>
    /// <param name="range"></param>
    /// <param name="damage"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerator BasicAttack(Being attacker, float range, int damage, CharacterAction action)
    {
        bool inAttack = true;
        Vector3 scale = attacker.rangeIndicator.gameObject.transform.localScale;
        attacker.rangeIndicatorColor = attacker.rangeIndicatorCombatColor;
        while (inAttack)
        {
            attacker.rangeIndicator.gameObject.SetActive(true);
            attacker.rangeIndicator.gameObject.transform.localScale = new Vector3(range * 2, scale.y, range * 2);
            if (Input.GetMouseButtonDown(0))
            {
                UIManager.CheckForUIElement();
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Attackable")))
                {
                    yield return null;
                    continue;
                }

                Being target = hit.collider.gameObject.GetComponentInParent<Being>();
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
                target.health -= damage;
                if (attacker is PlayerCharacter player)
                {
                    player.actionPoints -= 1;
                    player.EndCharacterAction();
                    break;
                }
            }
            if (Input.GetMouseButtonDown(1) || action.endSignal)
            {
                inAttack = false;
            }
            yield return null;
        }
        attacker.rangeIndicator.gameObject.SetActive(false);
    }


}
