using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolController : MonoBehaviour
{
    public GameObject[] patrolNodes;
    public int currIndex = 0;
    public int noOfNodes;

    private void Awake ()
    {
        noOfNodes = patrolNodes.Length;
    }
}
