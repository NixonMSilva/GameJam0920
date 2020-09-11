using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingController : MonoBehaviour
{
    public GameObject parent;

    private EnemyController ec;

    private void Awake ()
    {
        ec = parent.GetComponent<EnemyController>();
    }

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            ec.SetPlayerOnRange(true);
        }
    }

    private void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            ec.SetPlayerOnRange(false);
        }
    }
}
