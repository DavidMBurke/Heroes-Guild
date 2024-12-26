using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnOrderDisplay : MonoBehaviour
{
    ActionManager actionManager;
    List<TurnOrderBeingDisplay> beingDisplays;
    void Start()
    {
        actionManager = ActionManager.instance;
        beingDisplays = GetComponentsInChildren<TurnOrderBeingDisplay>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTurnOrderDisplays();
    }

    public void UpdateTurnOrderDisplays()
    {
        for (int i = 0; i < beingDisplays.Count; i++)
        {
            if (i < actionManager.beings.Count)
            {
                beingDisplays[i].gameObject.SetActive(true);
                beingDisplays[i].UpdateBeing(actionManager.beings[i]);
                continue;
            }
            beingDisplays[i].gameObject.SetActive(false);
        }
    }

}
