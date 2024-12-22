using System.Collections.Generic;
using UnityEngine;

public class OverlapDetector : MonoBehaviour
{
    private List<Being> beingsList;

    public void SetBeingList(List<Being> list)
    {
        beingsList = list;
    }
    private void OnTriggerEnter(Collider other)
    {
        Being being = other.GetComponentInParent<Being>();
        if (being != null)
        {
            beingsList.Add(being);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Being being = other.GetComponentInParent<Being>();
        if (being != null && beingsList.Contains(being))
        {
            beingsList.Remove(being);
        }
    }
}
