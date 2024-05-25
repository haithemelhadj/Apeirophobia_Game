using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent navAgent;
    public Transform player;
    public LayerMask groundLayer, playerLayer;
    //public float health;
    public float walkPointRange;
    //public float timeBetweenAttacks;
    public float sightRange;
    //public float attackRange;
    //public int damage;
    public Animator animator;
    //public ParticleSystem hitEffect;

    private Vector3 walkPoint;
    private bool walkPointSet;
    //private bool alreadyAttacked;
    //private bool takeDamage;
    //bool playerInSightRange;// = Physics.CheckSphere(transform.position, sightRange, playerLayer);

    public Material fovMaterial;
    //public Material CircleZoneMaterial;

    public CircleCollision circleCollision;


    public float currSpeed;
    public float walkSpeed;
    public float runSpeed;

    public Transform fovTransform;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();

        currentPoint = 0;
        StartCoroutine(FOVRoutine());
    }


    private void Update()
    {
        //bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        if (!playerInSightRange)
        {
            Patroling();
        }
        if (playerInSightRange || circleCollision.isCollidingWithPlayer)
        {
            ChasePlayer();
        }
        //else if (playerInAttackRange && playerInSightRange)
        //{
        //    //AttackPlayer();
        //    Debug.Log("Attack");
        //}
        //else if (!playerInSightRange && takeDamage)
        //{
        //    ChasePlayer();
        //}
    }

    public Color patrolColor;
    public Color chaseColor;

    private void Patroling()
    {
        fovMaterial.color = patrolColor;
        //CircleZoneMaterial.color = patrolColor;
        if (!walkPointSet)
        {
            //SearchWalkPoint();
            GetnextWalkPoint();
        }

        if (walkPointSet)
        {
            if (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                navAgent.speed = 0f;
                animator.SetFloat("Speed", 0f);
                //return;
            }
            else
            {
                navAgent.speed = walkSpeed;
                animator.SetFloat("Speed", walkSpeed);
                navAgent.SetDestination(walkPoint);
            }
        }


        //float distanceToWalkPoint = Vector3.Distance( transform.position , walkPoint);
        //animator.SetFloat("Velocity", 0.2f);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
        {
            walkPointSet = false;
        }
    }

    //this one is in an array
    public Transform[] points;
    public int currentPoint = 0;
    public float waitTime;

    private void GetnextWalkPoint()
    {
        waitTime = points[currentPoint].GetComponent<PointInfo>().waitTime;
        currentPoint++;
        currentPoint %= points.Length;
        walkPoint = points[currentPoint].position;
        walkPointSet = true;
    }


    //this one is random
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        //look at player
        fovMaterial.color = chaseColor;
        //CircleZoneMaterial.color = chaseColor;
        navAgent.speed = runSpeed;
        animator.SetFloat("Speed", runSpeed);
        Vector3 direction = player.position - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, direction.normalized, Time.deltaTime * 20);
        //transform.LookAt(player);
        navAgent.SetDestination(player.position);
        //animator.SetFloat("Velocity", 0.6f);
        navAgent.isStopped = false; // Add this line
    }


    #region fov
    [Header("Field of View")]
    public float radius;
    [Range(0, 180)] public float fovAngle;
    public Transform fovChild;

    public GameObject playerRef;
    private Transform lastPlayerSeenPosition;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    //[HideInInspector] 
    public bool playerInSightRange;


    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }


    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(fovChild.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(fovChild.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    playerInSightRange = true;
                    lastPlayerSeenPosition = playerRef.transform;

                }
                else
                {
                    playerInSightRange = false;

                }
            }
            else
            {
                playerInSightRange = false;

            }
        }
        else if (playerInSightRange)
        {
            playerInSightRange = false;

        }
    }



    #endregion



    #region extra
    //  private void AttackPlayer()
    //{
    //    navAgent.SetDestination(transform.position);

    //    if (!alreadyAttacked)
    //    {
    //        transform.LookAt(player.position);
    //        alreadyAttacked = true;
    //        animator.SetBool("Attack", true);
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);

    //        RaycastHit hit;
    //        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
    //        {
    //            /*
    //                YOU CAN USE THIS TO GET THE PLAYER HUD AND CALL THE TAKE DAMAGE FUNCTION

    //            PlayerHUD playerHUD = hit.transform.GetComponent<PlayerHUD>();
    //            if (playerHUD != null)
    //            {
    //               playerHUD.takeDamage(damage);
    //            }
    //             */
    //        }
    //    }
    //}


    //private void ResetAttack()
    //{
    //    alreadyAttacked = false;
    //    animator.SetBool("Attack", false);
    //}

    //public void TakeDamage(float damage)
    //{
    //    health -= damage;
    //    hitEffect.Play();
    //    StartCoroutine(TakeDamageCoroutine());

    //    if (health <= 0)
    //    {
    //        Invoke(nameof(DestroyEnemy), 0.5f);
    //    }
    //}

    //private IEnumerator TakeDamageCoroutine()
    //{
    //    takeDamage = true;
    //    yield return new WaitForSeconds(2f);
    //    takeDamage = false;
    //}

    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    private IEnumerator DestroyEnemyCoroutine()
    {
        //animator.SetBool("Dead", true);
        yield return new WaitForSeconds(1.8f);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    #endregion

}
