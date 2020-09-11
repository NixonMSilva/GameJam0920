using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBagController : MonoBehaviour
{
    public float healthIncrease = 20f;

    private void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().AddHealth(healthIncrease);
            Destroy(this.gameObject);
        }
    }
}
