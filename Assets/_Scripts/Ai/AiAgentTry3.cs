using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class AiAgentTry3 : MonoBehaviour
{
    public bool usesMeshToDetect = true;
    [Header("refrences")]
    public NavMeshAgent navAgent;
    public Animator animator;
    public Transform playerRef;
    public Vector3 playerRefFlastPos;
    public Transform eyes;

    [Header("Bools")]
    public bool canSeePlayer;
    public Vector3 lastPlayerSeenPosition;
    [Header("Level 3")]
    public bool isInLevelThree;

    [Header("settings")]
    [Range(0f, 180f)] public float fovAngle;
    public int segments;
    public float fovRadius;
    public float circleRadius;


    public Vector3 walkPoint;
    public bool walkPointSet;

    [Header("layer masks")]
    public LayerMask obstructionMask;
    public LayerMask groundLayer;
    public LayerMask playerMask;

    [Header("material")]
    public Material fovMaterial;
    public Color patrolColor;
    public Color chaseColor;
    public Color searchColor;


    [Header("speed")]
    public float currSpeed;
    public float walkSpeed;
    public float runSpeed;






    #region Draw Gizmos
    private void OnDrawGizmos()
    {
        /*
        //draw fov
        Vector3 fovLine1 = Quaternion.AngleAxis(fovAngle / 2, transform.up) * transform.forward * fovRadius;
        Vector3 fovLine2 = Quaternion.AngleAxis(-fovAngle / 2, transform.up) * transform.forward * fovRadius;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(eyes.position, fovLine1);
        Gizmos.DrawRay(eyes.position, fovLine2);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(eyes.position, (playerRef.position - eyes.position).normalized * fovRadius);

        Gizmos.color = Color.black;
        Gizmos.DrawRay(eyes.position, transform.forward * circleRadius);

        //draw circle
        for (int i = 0; i < segments; i++)
        {
            float angle = i * 360 / segments;
            Vector3 circlePointA = transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * circleRadius, 0, Mathf.Cos(Mathf.Deg2Rad * angle) * circleRadius);
            angle = (i + 1) * 360 / segments;
            Vector3 circlePointB = transform.position + new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle) * circleRadius, 0, Mathf.Cos(Mathf.Deg2Rad * angle) * circleRadius);

            Gizmos.color = Color.black;
            Gizmos.DrawLine(circlePointA, circlePointB);
        }
        /*
        */
    }
    #endregion

    #region start & update
    private void Awake()
    {
        playerRefFlastPos = new Vector3(playerRef.position.x, transform.position.y, playerRef.position.z);
        if (isInLevelThree)
        {
            //cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        }
        else
        {
        }
        InitializeMeshCreation();
        playerRef = GameObject.Find("Player").transform;
        navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(FOVRoutine());
        lastPlayerSeenPosition = playerRefFlastPos;
    }
    private void Update()
    {
        //animator.SetFloat("Speed", navAgent.speed);
        playerRefFlastPos = new Vector3(playerRef.position.x, transform.position.y, playerRef.position.z);
        if (!isInLevelThree)
        {
            if (usesMeshToDetect)
            {

            }
            CreateMesh();
            suspisionCooldown();
        }
        //Level 3 Code
        if (isInLevelThree)
        {
            DetectToDestroy();
            navAgent.SetDestination(playerRefFlastPos);
            //navAgent.speed = float.
            navAgent.speed = runSpeed;
            animator.SetFloat("Speed", runSpeed);
            //look faster at player
            Vector3 direction = playerRefFlastPos - transform.position;
            transform.forward = Vector3.Slerp(transform.forward, direction.normalized, Time.deltaTime * chaseRotation);
        }
        else if (canSeePlayer)
        {
            //set material color
            this.fovMaterial.color = chaseColor;
            //set run speed
            navAgent.speed = runSpeed;
            //set animation in blend tree
            animator.SetFloat("Speed", runSpeed);
            Chase();
        }
        else if (suspisionTimer > 0)
        {
            //set material color            
            //mesh.SetColors(new List<Color> { chaseColor });
            this.fovMaterial.color = searchColor;
            //set run speed
            navAgent.speed = walkSpeed;
            //set animation in blend tree
            animator.SetFloat("Speed", navAgent.speed);
            Search();
        }
        else
        {
            //set material color            
            this.fovMaterial.color = patrolColor;
            Patroling();
        }
    }
    #endregion  
    //------------------------------------------patrol---------------------
    [Header("patrol")]
    public Transform[] points;
    public int currentPoint = 0;
    public float waitTime;
    #region patrol
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
                //navAgent.speed= float.mo
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
    #endregion
    //------------------------------------------search---------------------
    [Header("search")]
    public float searchPointRange;
    public float suspisionTime;
    public float suspisionTimer;
    #region search
    private void Search()
    {
        //return;
        if (!walkPointSet)
        {
            float randomZ = Random.Range(-searchPointRange, searchPointRange);
            float randomX = Random.Range(-searchPointRange, searchPointRange);
            walkPoint = new Vector3(lastPlayerSeenPosition.x + randomX, transform.position.y, lastPlayerSeenPosition.z + randomZ);
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
                waitTime = suspisionTime / 2;
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
    #endregion
    //------------------------------------------suspision---------------------
    #region suspision
    private void suspisionCooldown()
    {
        if (suspisionTimer > 0)
        {
            suspisionTimer -= Time.deltaTime;
        }
    }
    #endregion

    //------------------------------------------chase---------------------
    [Header("Chase")]
    public float chaseRotation = 20f;

    #region chase
    private void Chase()
    {
        //look faster at player
        Vector3 direction = playerRefFlastPos - transform.position;
        transform.forward = Vector3.Slerp(transform.forward, direction.normalized, Time.deltaTime * chaseRotation);
        //follow player
        navAgent.SetDestination(lastPlayerSeenPosition);
    }
    #endregion
    //------------------------------------------See---------------------
    #region See

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            if (!usesMeshToDetect)
            {
                See();
            }
        }
    }
    private void See()
    {
        if (Vector3.Distance(transform.position, playerRef.position) < circleRadius)
        {
            if (!Physics.Raycast(eyes.position, playerRef.position, obstructionMask))
            {
                canSeePlayer = true;
                lastPlayerSeenPosition = playerRef.position;
                suspisionTimer = suspisionTime;
                return;
            }
        }
        if (Vector3.Distance(transform.position, playerRef.position) < fovRadius)
        {
            Vector3 dirToPlayer = (playerRef.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            if (angleBetweenGuardAndPlayer < fovAngle / 2f)
            {
                Debug.DrawRay(eyes.position, playerRef.position - transform.position, Color.green, 1f);
                Debug.Log("drawing line");
                if (!Physics.Raycast(eyes.position, playerRef.position - transform.position, out RaycastHit hiit, fovRadius, obstructionMask))
                {
                    canSeePlayer = true;
                    Debug.Log("can see player");

                    lastPlayerSeenPosition = playerRef.position;
                    suspisionTimer = suspisionTime;
                    //Debug.Log(hiit.collider.gameObject.name);

                }
                else
                {
                    Debug.Log(hiit.collider.gameObject.name);
                    Debug.Log("obstacle between us");
                    canSeePlayer = false;
                }
            }
            else
            {
                Debug.Log("close but not in front");
                canSeePlayer = false;
            }
        }
        else
        {
            Debug.Log("not even close");
            canSeePlayer = false;
        }

    }
    /*
    */
    #endregion


    #region Camera Shake
    [Header("Camera Shake")]
    public float intensity;
    public CinemachineVirtualCamera cinemachineCamera;
    public void shakeCam()
    {
        if (isInLevelThree)
        {
            cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = intensity;
            Invoke("CancleCamShake", 0.08f);

        }
    }
    public void CancleCamShake()
    {
        cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;

    }
    #endregion


    #region Called Functions
    RaycastHit hit;
    public Vector3[] rayHits;
    float anglePerSegment;

    [Header("Mesh")]
    private Mesh mesh;
    Vector3[] vertics;
    Vector2[] uvs;
    int[] triangles;



    private void InitializeMeshCreation()
    {
        //initialize the rayhit locations array
        rayHits = new Vector3[segments + 1];
        //initialize the mesh arrays
        vertics = new Vector3[segments + 2];
        uvs = new Vector2[segments + 2];
        triangles = new int[segments * 3];

        //get angle per segment
        anglePerSegment = fovAngle / segments;

        /*
        //get values from parent
        angle= aiAgent.fovAngle;
        fovRadius = aiAgent.fovRadius;
        obstructionMask = aiAgent.obstructionMask;
        mat = aiAgent.fovMaterial;
        segments= aiAgent.segments;
        */


        //MESH
        var MeshF = gameObject.AddComponent<MeshFilter>();
        var MeshR = gameObject.AddComponent<MeshRenderer>();
        //var MeshMC = gameObject.AddComponent<MeshCollider>();
        MeshR.material = fovMaterial;
        //go.renderer.material.mainTexture = Resources.Load("glass", typeof(Texture2D));
        //AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        //MESH
        mesh = gameObject.GetComponent<MeshFilter>().mesh;

        //BUILD THE MESH COLLIDER
        //MeshMC.convex = true;
        //MeshMC.isTrigger = true;
        //MeshMC.sharedMesh = mesh;
    }

    private void CreateMesh()
    {

        #region send raycasts
        //get first vector
        Vector3 firstVector = transform.forward;
        firstVector = Quaternion.AngleAxis(-fovAngle / 2, Vector3.up) * firstVector;


        //reset can see player before checking every frame
        if (usesMeshToDetect)
        {
            canSeePlayer = false;

        }

        //send raycasts to get the hit positions
        for (int i = 0; i < rayHits.Length; i++)
        {
            if (Physics.Raycast(transform.position, firstVector, out hit, fovRadius, obstructionMask))
            {
                //Debug.Log("hit ground");
                rayHits[i] = hit.point;
            }
            else
            {

                if (Physics.Raycast(transform.position, firstVector, out hit, fovRadius, playerMask) && usesMeshToDetect)
                {
                    //Debug.Log("hit player");
                    canSeePlayer = true;
                    lastPlayerSeenPosition = playerRefFlastPos;
                    suspisionTimer = suspisionTime;
                }

                //Debug.Log("nothing");
                rayHits[i] = transform.position + firstVector * fovRadius;
            }
            rayHits[i] = transform.InverseTransformPoint(rayHits[i]);
            //rayHits[i] -= transform.position;
            //rayHits[i] -= transform.rotation * Vector3.forward;


            //draw raycasts
            if (i == 0)
            {
                //Debug.DrawLine(transform.position, rayHits[i], Color.green);
                //Debug.DrawRay(transform.position, firstVector * fovRadius, Color.black);
            }
            else
            {
                //Debug.DrawLine(transform.position, rayHits[i], Color.red);
                //Debug.DrawRay(transform.position, firstVector * fovRadius, Color.blue);
            }

            //rotate ray by 1 segment
            firstVector = Quaternion.AngleAxis(anglePerSegment, Vector3.up) * firstVector;
        }
        #endregion

        #region set vertices and uvs
        //vertics[0] = transform.position;
        this.vertics[0] = Vector3.zero;
        for (int i = 1; i < this.vertics.Length; i++)
        {
            this.vertics[i] = rayHits[i - 1];
        }
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(this.vertics[i].x, this.vertics[i].z);
        }
        #endregion

        #region set triangles
        //triangles
        int counter = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            triangles[i] = 0;
            //Debug.Log("tri: " + triangles[i]);
            triangles[i + 1] = counter + 1;
            //Debug.Log("tri: " + triangles[i + 1]);
            triangles[i + 2] = counter + 2;
            //Debug.Log("tri: " + triangles[i + 2]);
            counter++;
        }
        #endregion


        #region draw mesh
        //now draw mesh
        mesh.Clear();
        mesh.vertices = vertics;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        #endregion
    }

    public LayerMask blockLayer;
    public float destroyDistance;
    public float lenghtModifier;
    private void DetectToDestroy()
    {
        Debug.DrawRay(transform.position + Vector3.up * lenghtModifier, Vector3.forward * destroyDistance, Color.red);
        if (Physics.Raycast(transform.position + Vector3.up * lenghtModifier, Vector3.forward, out RaycastHit ray, destroyDistance, blockLayer))
        {
            //Debug.Log("destroying block");
            //Destroy(ray.collider.gameObject);
        }
        if (Physics.Raycast(transform.position + Vector3.up * lenghtModifier, Vector3.forward, out RaycastHit ray2, destroyDistance))
        {
            if (ray2.collider.gameObject.CompareTag("Player"))
            {
                playerRef.GetComponent<PlayerMovementTest1>().GameOver();
            }
        }

    }

    #endregion
}