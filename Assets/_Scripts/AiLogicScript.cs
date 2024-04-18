using UnityEngine;
using UnityEngine.AI;


public class PathInfo
{
    public Transform transform;
    public float floatValue;

    public PathInfo(Transform transform, float floatValue)
    {
        this.transform = transform;
        this.floatValue = floatValue;
    }
}






public class AiLogicScript : MonoBehaviour
{
    //public float speed = 5.0f;
    public float waitTime = 3.0f;

    //NavMeshAgent agent;



    // Start is called before the first frame update
    //void Start()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Patrol();
    //}



    //public Transform[] path;
    //public float pathCurrIndex;
    //public Transform target;
    private void Search()
    {

    }

    private void Chase()
    {

    }

    //////////////////////////////////////////////////////////////////////
    public PathInfo[] PathInfo;
    public Transform[] waypoints;

    NavMeshAgent agent;
    int waypointIndex;
    Vector3 target;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        UpdateDestination();
    }
    void Update()
    {
        Patrol();
    }
    private void Patrol()
    {
        if (Vector3.Distance(transform.position, target) < 1f)
        {
            if (waitTime > 0)
            {
                waitTime -= Time.deltaTime;
            }
            else
            {
                waitTime = PathInfo[waypointIndex].floatValue;

                waypointIndex++;
                if (waypointIndex == PathInfo.Length)
                {
                    waypointIndex = 0;
                }

                UpdateDestination();
            }
        }
    }

    void UpdateDestination()
    {
        target = PathInfo[waypointIndex].transform.position;
        agent.SetDestination(target);
    }

    void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == PathInfo.Length)
        {
            waypointIndex = 0;
        }
    }
}
