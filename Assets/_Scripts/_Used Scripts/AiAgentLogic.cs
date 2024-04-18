using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AiAgentLogic : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] targetsList;
    public int nextTarget;
    public Transform position;
    private void Start()
    {
        nextTarget = 0;
        StartCoroutine(FOVRoutine());
    }
    private void Update()
    {
        if (!canSeePlayer)
        {
            GoToPoint(targetsList[nextTarget].transform);
            Debug.Log("point");
        }
        else
        {
            GoToPoint(lastPlayerSeenPosition);
            Debug.Log("player");
        }

        //Patrol(position);

    }
    #region Ai states

    float waitTime;
    public void GoToPoint(Transform target)
    {
        //if ai reaches the destination
        //update wait time
        //update next destination
        if (Vector3.Distance(transform.position, target.transform.position) < 1f)
        {

            //add wait time here
            waitTime = targetsList[nextTarget].GetComponent<PointInfo>().waitTime;

            //got to next target
            nextTarget++;
            if (nextTarget >= targetsList.Length)
            {
                nextTarget = 0;
            }
        }
        //if ai is far from destination
        else
        {
            //wait for time and then go to destination
            if (waitTime > 0f)
            {
                waitTime -= Time.deltaTime;
                return;
            }
            agent.SetDestination(target.transform.position);

        }
        //if (!canSeePlayer)
        //{
        //    GoToPoint(targetsList[nextTarget]);
        //}
    }


    public void Follow(Transform target)
    {

    }

    private void Detect()
    {
        //raycasts in fov, follow youtube video
        //Physics.
    }

    #endregion

    #region fov
    [Header("Field of View")]
    public float radius;
    [Range(0, 180)] public float fovAngle;

    public GameObject playerRef;
    private Transform lastPlayerSeenPosition;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    [HideInInspector] public bool canSeePlayer;


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
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToTarget) < fovAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    lastPlayerSeenPosition = playerRef.transform;

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
        else if (canSeePlayer)
        {
            canSeePlayer = false;

        }
    }



    #endregion

    #region mesh 

    private void drawMesh()
    {
        Mesh mesh = new Mesh();

    }
    #endregion

}
