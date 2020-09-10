using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    public GameObject parent;

    private EnemyController ec;

    private void Awake ()
    {
        ec = parent.GetComponent<EnemyController>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        // ec.PlayerOnLOS();
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name.Equals("Player"))
        {
            Debug.Log("Player on sight!");
            ec.PlayerOnLOS();
        }
    }
}
