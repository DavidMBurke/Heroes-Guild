using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    void Start()
    {
        MeshRenderer tileRenderer = gameObject.GetComponent<MeshRenderer>();
        tileRenderer.material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
