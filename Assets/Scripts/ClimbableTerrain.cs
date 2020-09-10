using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbableTerrain : MonoBehaviour
{
    public GameObject pair;

    private void Awake ()
    {
           
    }

    public void PerformClimb (Transform player)
    {
        if (pair != null)
            player.position = pair.transform.position;
    }
}
