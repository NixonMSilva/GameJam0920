using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : TextTriggerController
{
    public GameObject enemyA, enemyB, explosion;

    private void OnDestroy ()
    {
        enemyA.SetActive(true);
        enemyB.SetActive(true);
        explosion.SetActive(true);
    }
}
