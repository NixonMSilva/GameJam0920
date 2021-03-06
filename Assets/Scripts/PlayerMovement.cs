﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private Collider2D col;

    public ContactFilter2D cfClimb;
    public ContactFilter2D cfTakedown;

    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 2.0f;

    public bool isSneaking = false;

    public bool canMove = true;

    private bool isMessageActive = false;

    public float noiseLevel = 0f;
    public float playerHealth = 100f;

    public GameObject ui_noiseLevelIndicator;
    public GameObject ui_healthLevelIndicator;

    public GameObject ui_messageBox;

    public GameObject ui_takedownTip;
    public GameObject ui_retryPanel;

    private MessageController messageController;

    private float speed_x, speed_y;

    private AudioManager audioMgr;

    private Animator anim;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        messageController = ui_messageBox.GetComponent<MessageController>();

        audioMgr = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        ui_retryPanel.SetActive(false);

        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void Start ()
    {
        playerHealth = 100f;
        messageController.HideMessageBox();
    }

    private void FixedUpdate ()
    {
        rb.velocity = new Vector2(moveH, moveV);
    }

    // Update is called once per frame
    private void Update ()
    {

        

        // Condition to avoid player health extrapolating the limits
        if (playerHealth > 100f)
            playerHealth = 100f;

        // Condition to kill the player if the health falls below 0
        if (playerHealth <= 0f)
            Die();

        if (Input.GetKey(KeyCode.LeftControl))
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
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakedownCheck();
        }
                
        /*
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (isMessageActive)
            {
                messageController.HideMessageBox();
                isMessageActive = false;
            }
            else
            {
                messageController.ShowMessageBox();
                isMessageActive = true;
            }
        } */

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

        // Animator stuff
        anim.SetFloat("speed", rb.velocity.sqrMagnitude);
        anim.SetFloat("horizontal", rb.velocity.x);
        anim.SetFloat("vertical", rb.velocity.y);
        anim.SetBool("isSneaking", isSneaking);

        Debug.Log("X: " + rb.velocity.x + "Y: " + rb.velocity.y);

        UpdateNoiseBar();
        UpdateHealthBar();
        TakedownTip();
    }

    private void UpdateNoiseBar ()
    {
        Vector3 newBarSize = new Vector3((noiseLevel / 50f), 1f, 1f);
        ui_noiseLevelIndicator.transform.localScale = newBarSize;
    }

    private void UpdateHealthBar ()
    {
        Vector3 newBarSize = new Vector3(0f, 1f, 1f);
        if (playerHealth > 0f)
            newBarSize.x = playerHealth / 100f;
        ui_healthLevelIndicator.transform.localScale = newBarSize;
    }

    private void ClimbCheck ()
    {
        ClimbableTerrain ct;
        List<Collider2D> lc = new List<Collider2D>();
        if (col.OverlapCollider(cfClimb, lc) > 0)
        {
            ct = lc[0].gameObject.GetComponent<ClimbableTerrain>();
            if (ct != null)
            {
                ct.PerformClimb(this.transform);
            }
        }
    }

    private void TakedownTip ()
    {
        List<Collider2D> lc = new List<Collider2D>();
        bool enemyFound = false;
        if (col.OverlapCollider(cfTakedown, lc) > 0)
        {
            //Debug.Log(lc.Count);
            foreach (Collider2D spot in lc)
            {
                if (spot != null && spot.CompareTag("takedown"))
                {
                    if (!spot.GetComponentInParent<EnemyController>().GetChaseStatus())
                    {
                        ui_takedownTip.SetActive(true);
                        enemyFound = true;
                        break;
                    }
                }
            }
            if (!enemyFound)
                ui_takedownTip.SetActive(false);
        }
        else
        {
            ui_takedownTip.SetActive(false);
        }
    }

    private void TakedownCheck ()
    {
        EnemyController ec = null;
        List<Collider2D> lc = new List<Collider2D>();
        if (col.OverlapCollider(cfTakedown, lc) > 0)
        {
            foreach (Collider2D spot in lc)
            {
                if (spot != null && spot.CompareTag("takedown"))
                {
                    ec = spot.gameObject.GetComponentInParent<EnemyController>();
                    break;
                }
            }
            if (ec != null)
            {
                ec.Takedown();
            }
        }
    }

    public float GetNoiseLevel ()
    {
        return noiseLevel;
    }

    public void TakeDamage (float damage)
    {
        playerHealth -= damage;
        audioMgr.PlaySound("Player Grunt");
    }

    public void Die ()
    {
        Destroy(this.gameObject);
        ui_retryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddHealth (float amount)
    {
        playerHealth += amount;
    }
}
