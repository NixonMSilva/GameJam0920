using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    private bool isPlayerOnRange = false, hasSeenPlayer = false, isPlayerOccluded = false;
    private bool canAttack = true;

    public float attackRange = 0.44f;

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

    private PatrolController patrol;

    public int enemyType;

    AudioManager audioMgr;

    private void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        originalAngle = rb.transform.rotation;

        los_rb = lineOfSight.GetComponent<Rigidbody2D>();

        playerRb = player.GetComponent<Rigidbody2D>();
        playerCol = player.GetComponent<CircleCollider2D>();
        playerMov = player.GetComponent<PlayerMovement>();

        destinationSetter = GetComponent<AIDestinationSetter>();

        pathfinding = GetComponent<AIPath>();

        audioMgr = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        patrol = GetComponent<PatrolController>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void FixedUpdate ()
    {
        // Type 1 enemy ignores sound
        if (enemyType != 1)
        {
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
                            ChasePlayer();
                            isPlayerOccluded = false;
                        }
                    }
                    else
                    {
                        ChasePlayer();
                        isPlayerOccluded = false;
                    }
                }
                else if (hasSeenPlayer)
                {

                    LookAtPlayer();
                }
            }
            else
            {
                StopChasingPlayer();
            }
        }
        

        Debug.Log(gameObject.name + " has seen player? " + hasSeenPlayer);
        Debug.Log(gameObject.name + " can attack? " + canAttack);
        // Process of attacking the player
        if (hasSeenPlayer && canAttack)
        {
            Debug.Log("Attack attempt!");
            if (Vector3.Distance(this.transform.position, player.transform.position) <= attackRange)
            {
                Debug.Log(Vector3.Distance(this.transform.position, player.transform.position));
                AttackPlayer();
            }
        }

        // Patrol

        if (!hasSeenPlayer)
        {
            if (enemyType == 1)
            {
                ReturnToBase();
            }
            else if (patrol.hasPatrolPath && patrol.noOfNodes > 0)
                SeekNextNode();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        los_rb.position = this.transform.position;
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

    private void LookAtPath (Vector3 pathPosition)
    {
        Vector2 direction = pathPosition - this.transform.position;
        if (direction != Vector2.zero)
        {
            los_rb.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        }
    }

    private void ResetOrientation ()
    {
        // rb.MoveRotation(originalAngle);
    }

    private void ChasePlayer ()
    {
        LookAtPlayer();
        pathfinding.canSearch = true;
        destinationSetter.target = player.transform;
        hasSeenPlayer = true;
    }

    private void StopChasingPlayer ()
    {
        pathfinding.canSearch = false;
        hasSeenPlayer = false;
        rb.velocity = Vector3.zero;
        ResetOrientation();
    }
    
    private void SeekNextNode ()
    {
        //Debug.Log(patrol.noOfNodes);
        Transform nextNode = patrol.patrolNodes[patrol.currIndex].transform;
        LookAtPath(nextNode.position);
        pathfinding.canSearch = true;
        destinationSetter.target = nextNode;
        //Debug.Log(destinationSetter.target.gameObject.name);
        if (Vector3.Distance(gameObject.transform.position, nextNode.position) < 0.2f)
        {
            if (patrol.currIndex < (patrol.noOfNodes - 1))
                patrol.currIndex++;
            else
                patrol.currIndex = 0;
        }
    }


    public void PlayerOnLOS ()
    {
        ChasePlayer();
    }

    public void LeftLOS ()
    {
        if (enemyType == 1)
        {
            StopChasingPlayer();
        }
    }

    public void Takedown ()
    {
        // Debug.Log("Takedown activated");
        // Type 1 enemy = Immortal enemies
        if (!hasSeenPlayer && enemyType != 1)
            Destroy(this.gameObject);
    }

    public int GetEnemyType ()
    {
        return enemyType;
    }

    public void SetPlayerOnRange (bool status)
    {
        isPlayerOnRange = status;
    }

    private void AttackPlayer ()
    {
        canAttack = false;
        Debug.Log("Attack");
        if (enemyType == 1)
            playerMov.TakeDamage(100f);
        else
            playerMov.TakeDamage(Random.Range(15f, 25f));
        StartCoroutine(AttackCooldown(2f));
    }

    private void ReturnToBase ()
    {
        if (Vector2.Distance(this.transform.position, patrol.patrolNodes[0].transform.position) < 0.05f)
        {
            destinationSetter.target = patrol.patrolNodes[0].transform;
            pathfinding.canSearch = true;
        }
        else
        {
            pathfinding.canSearch = false;
        }

    }

    IEnumerator AttackCooldown (float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canAttack = true;
    }

    public bool GetChaseStatus ()
    {
        return hasSeenPlayer;
    }
}
