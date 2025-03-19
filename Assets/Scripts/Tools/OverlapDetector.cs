using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component for detecting overlapping items
/// </summary>
public class OverlapDetector : MonoBehaviour
{
    /// <summary>
    /// List of overlapping beings
    /// </summary>
    private List<Being> beingsList = null!;
    
    /// <summary>
    /// Update beings list
    /// </summary>
    /// <param name="list"></param>
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
