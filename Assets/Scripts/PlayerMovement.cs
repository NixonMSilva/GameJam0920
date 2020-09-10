﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Collider2D col;

    public ContactFilter2D cf;

    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 2.0f;

    public bool isSneaking = false;

    public float noiseLevel = 0f;
    public float playerHealth = 100f;

    public GameObject ui_noiseLevelIndicator;
    public GameObject ui_healthLevelIndicator;
    

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        playerHealth = 100f;
    }

    private void FixedUpdate ()
    {
        rb.velocity = new Vector2(moveH, moveV);
    }

    // Update is called once per frame
    private void Update()
    {
        // Condition to avoid player health extrapolating the limits
        if (playerHealth > 100f)
            playerHealth = 100f;
        
        if (Input.GetKey(KeyCode.C))
        {
            isSneaking = true;
            moveSpeed = 1.0f;
        }
        else
        {
            isSneaking = false;
            moveSpeed = 2.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClimbCheck();
        }

        moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
        moveV = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (moveH != 0f || moveV != 0f)
        {
            if (isSneaking)
            {
                noiseLevel = 20f;
            }
            else
            {
                noiseLevel = 50f;
            }
        }
        else
        {
            noiseLevel = 0f;
        }

        UpdateNoiseBar();
        UpdateHealthBar();
    }

    private void UpdateNoiseBar ()
    {
        Vector3 newBarSize = new Vector3((noiseLevel / 50f), 1f, 1f);
        ui_noiseLevelIndicator.transform.localScale = newBarSize;
    }

    private void UpdateHealthBar ()
    {
        Vector3 newBarSize = new Vector3((playerHealth / 100f), 1f, 1f);
        ui_healthLevelIndicator.transform.localScale = newBarSize;
    }

    private void ClimbCheck ()
    {
        ClimbableTerrain ct;
        List<Collider2D> lc = new List<Collider2D>();
        if (col.OverlapCollider(cf, lc) > 0)
        {
            ct = lc[0].gameObject.GetComponent<ClimbableTerrain>();
            if (ct != null)
            {
                ct.PerformClimb(this.transform);
            }
        }
    }

    public float GetNoiseLevel ()
    {
        return noiseLevel;
    }
}
