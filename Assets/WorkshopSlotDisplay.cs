using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkshopSlotDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;
    public WorkshopPage workshopPage;

    private void Update()
    {
        text.text = $"{workshopPage.GetCrafters().Count}/{workshopPage.workstationsCount}";
    }
}
