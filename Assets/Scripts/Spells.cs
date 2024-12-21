using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells
{
    public static IEnumerator Spell1Coroutine(Being caster)
    {
        float timer = 0f;
        Debug.Log($"Spell cast by {caster.name}");
        if (caster is PlayerCharacter player)
        {
            player.actionPoints -= 1;
        }
        while (timer < 3)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        Debug.Log($"Ending Spell Cast");
    }

    public static IEnumerator FireBall(Being caster, CharacterAction action)
    {
        bool inSpell = true;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(5, 5, 5);
        Renderer renderer = sphere.GetComponent<Renderer>();
        Collider sphereCollider = sphere.GetComponent<Collider>();
        sphereCollider.isTrigger = true;
        renderer.material = new Material(Shader.Find("Transparent/Diffuse"));
        renderer.material.color = new Color(1, 0, 0, 0.25f);
        List<Being> overlappingBeings = new List<Being>();
        OverlapDetector detector = sphere.AddComponent<OverlapDetector>();
        detector.SetBeingList(overlappingBeings);

        while (inSpell)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                sphere.transform.position = hit.point;
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (UIManager.CheckForUIElement())
                {
                    yield return null;
                    continue;
                }
                foreach (Being being in overlappingBeings)
                {
                    being.health -= 10;
                }
                if (caster is PlayerCharacter player)
                {
                    player.actionPoints -= 1;
                }
                inSpell = false;
            }
            if (Input.GetMouseButtonDown(1) || action.endSignal)
            {
                inSpell = false;
            }
            yield return null;
        }
        GameObject.Destroy(sphere);
    }

}
