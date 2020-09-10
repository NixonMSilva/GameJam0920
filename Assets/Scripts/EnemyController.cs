using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    private bool isPlayerOnRange = false, hasSeenPlayer = false, isPlayerOccluded = false;

    public GameObject player;
    public GameObject lineOfSight;

    private Rigidbody2D rb;

    private Quaternion originalAngle;

    private Rigidbody2D los_rb;

    private Rigidbody2D playerRb;
    private Collider2D playerCol;
    private PlayerMovement playerMov;

    public LayerMask opaqueObjects;

    public AIDestinationSetter destinationSetter;
    private AIPath pathfinding;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        originalAngle = rb.transform.rotation;

        los_rb = lineOfSight.GetComponent<Rigidbody2D>();

        playerRb = player.GetComponent<Rigidbody2D>();
        playerCol = player.GetComponent<CircleCollider2D>();
        playerMov = player.GetComponent<PlayerMovement>();

        destinationSetter = GetComponent<AIDestinationSetter>();
        destinationSetter.target = player.transform;

        pathfinding = GetComponent<AIPath>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        los_rb.position = this.transform.position;

        if (isPlayerOnRange)
        {
            if (playerMov.GetNoiseLevel() > 30f)
            {
                RaycastHit2D rc = Physics2D.Raycast(this.transform.position, player.transform.position, Mathf.Infinity, opaqueObjects);
                if (rc.collider != null)
                {
                    float distanceToPlayer = Vector2.Distance(this.transform.position, player.transform.position);
                    float distanceToOccluder = Vector2.Distance(this.transform.position, rc.transform.position);
                    if (distanceToOccluder < distanceToPlayer)
                    {
                        isPlayerOccluded = true;
                    }
                    else
                    {
                        LookAtPlayer();
                        ChasePlayer();
                        isPlayerOccluded = false;
                    }
                }
                else
                {
                    LookAtPlayer();
                    ChasePlayer();
                    isPlayerOccluded = false;
                }
            }
        }
        else
        {
            StopChasingPlayer();
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D (Collider2D other)
    {
        if (other.Equals(playerCol))
        {
            isPlayerOnRange = true;
            // Debug.Log("Player entering detection range");
        }
    }

    private void OnTriggerStay2D (Collider2D other)
    {
        /*
        if (other.Equals(playerCol))
            Debug.Log("Player on detection range");
        */
    }

    private void OnTriggerExit2D (Collider2D other)
    {
        if (other.Equals(playerCol))
        {
            isPlayerOnRange = false;
            ResetOrientation();
            // Debug.Log("Player leaving detection range");
        }
            
    }

    private void LookAtPlayer ()
    {
        Vector2 direction = player.transform.position - this.transform.position;
        if (direction != Vector2.zero)
        {
            // rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            los_rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        }
    }

    private void ResetOrientation ()
    {
        // rb.MoveRotation(originalAngle);
    }

    private void ChasePlayer ()
    {
        pathfinding.canSearch = true;
        hasSeenPlayer = true;
    }

    private void StopChasingPlayer ()
    {
        pathfinding.canSearch = false;
        hasSeenPlayer = false;
    }

    public void PlayerOnLOS ()
    {
        LookAtPlayer();
        ChasePlayer();
    }
}
