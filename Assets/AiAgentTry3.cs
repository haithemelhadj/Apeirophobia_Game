using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgentTry3 : MonoBehaviour
{
    [Header("refrences")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public Transform playerRef;
    public Transform eyes;

    [Header("Bools")]
    public bool canSeePlayer;
    public Transform lastPlayerSeenPosition;

    [Header("settings")]
    [Range(0f, 180f)] public float fovAngle;
    public float fovRadius;
    public float circleRadius;


    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("layer masks")]
    public LayerMask obstructionMask;
    public LayerMask groundLayer;

    [Header("material")]
    public Material fovMaterial;
    public Color patrolColor;
    public Color chaseColor;
    public Color searchColor;


    [Header("speed")]
    public float currSpeed;
    public float walkSpeed;
    public float runSpeed;

    [Header("Chase")]
    public float chaseRotation = 20f;

    [Header("patrol")]    
    public Transform[] points;
    public int currentPoint = 0;
    public float waitTime;

    [Header("search")]
    public float searchPointRange;
    public float suspisionTime;
    private float suspisionTimer;    


    private void Awake()
    {
        playerRef = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
    }
    private void Update()
    {
        //See();
        suspisionCooldown();

        if (canSeePlayer)
        {
            //set material color
            fovMaterial.color = chaseColor;
            //set run speed
            navAgent.speed = runSpeed;
            //set animation in blend tree
            animator.SetFloat("Speed", runSpeed);
            Chase();
        }
        else if (suspisionTimer > 0)
        {
            //set material color
            fovMaterial.color = searchColor;
            //set run speed
            navAgent.speed = walkSpeed;
            //set animation in blend tree
            animator.SetFloat("Speed", walkSpeed);
            Search();
        }
        else
        {
            //set material color
            fovMaterial.color = patrolColor;
            Patroling();
        }
    }

    
    //------------------------------------------patrol---------------------
    private void Patroling()
    {
        
        if (!walkPointSet)
        {
            GetnextWalkPoint();
        }

        if (walkPointSet)
        {
            if (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                //set run speed
                navAgent.speed = 0f;
                //set animation in blend tree
                animator.SetFloat("Speed", 0f);
            }
            else
            {
                //set run speed
                navAgent.speed = walkSpeed;
                //set animation in blend tree
                animator.SetFloat("Speed", walkSpeed);
                navAgent.SetDestination(walkPoint);
            }
        }

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
        {
            walkPointSet = false;
        }
    }

    private void GetnextWalkPoint()
    {
        waitTime = points[currentPoint].GetComponent<PointInfo>().waitTime;
        currentPoint++;
        currentPoint %= points.Length;
        walkPoint = points[currentPoint].position;
        walkPointSet = true;
    }

    
    //------------------------------------------search---------------------
    private void Search()
    {

        if (!walkPointSet)
        {
            float randomZ = Random.Range(-searchPointRange, searchPointRange);
            float randomX = Random.Range(-searchPointRange, searchPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            {
                walkPointSet = true;
            }
        }


        if (walkPointSet)
        {
            if (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                //set run speed
                navAgent.speed = 0f;
                //set animation in blend tree
                animator.SetFloat("Speed", 0f);
            }
            else
            {
                //set run speed
                navAgent.speed = walkSpeed;
                //set animation in blend tree
                animator.SetFloat("Speed", walkSpeed);
                navAgent.SetDestination(walkPoint);
            }
        }

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
        {
            walkPointSet = false;
        }
    }

    //------------------------------------------chase---------------------
    private void Chase()
    {
        //look faster at player
        Vector3 direction = playerRef.position - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, direction.normalized, Time.deltaTime * chaseRotation);
        //follow player
        navAgent.SetDestination(lastPlayerSeenPosition.position);
    }

    //------------------------------------------See---------------------
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            See();
        }
    }
    private void See()
    {
        if(Vector3.Distance(transform.position, playerRef.position) < circleRadius)
        {
            if (!Physics.Raycast(eyes.position, playerRef.position, obstructionMask))
            {
                canSeePlayer = true;
                lastPlayerSeenPosition.position = playerRef.position;
                suspisionTimer = suspisionTime;
                return;
            }
        }
        if(Vector3.Distance(transform.position, playerRef.position) < fovRadius)
        {
            Vector3 dirToPlayer = (playerRef.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            if(angleBetweenGuardAndPlayer < fovAngle / 2f)
            {
                if(!Physics.Raycast(eyes.position, playerRef.position, obstructionMask))
                {
                    canSeePlayer = true;
                    lastPlayerSeenPosition.position = playerRef.position;
                    suspisionTimer = suspisionTime;

                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else
        {
            canSeePlayer = false;
        }
        
    }


    //------------------------------------------suspision---------------------
    private void suspisionCooldown()
    {
        if (suspisionTimer > 0) 
        {
            suspisionTimer -= Time.deltaTime;
        }
    }
}
