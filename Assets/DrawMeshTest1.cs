using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DrawMeshTest1 : MonoBehaviour
{
    public AiAgentTry3 aiAgent;
    /*
    [HideInInspector] public int segments = 5;
    [HideInInspector] public float fovRadius = 5;
    [HideInInspector] public float angle;
    [HideInInspector] public Material mat;
    [HideInInspector] public LayerMask obstructionMask;
    */

    public int segments = 5;
    public float fovRadius = 5;
    public float angle;
    public Material mat;
    public LayerMask obstructionMask;
    public LayerMask playerMask;

    public Vector3[] rayHits;
    float anglePerSegment;

    Vector3[] vertics;
    Vector2[] uvs;
    int[] triangles;


    RaycastHit hit;
    private Mesh mesh;




    private void Start()
    {
        InitializeMeshCreation();
    }

    private void Update()
    {
        CreateMesh();
    }


    private void InitializeMeshCreation()
    {
        //initialize the rayhit locations array
        rayHits = new Vector3[segments + 1];
        //initialize the mesh arrays
        vertics = new Vector3[segments + 2];
        uvs = new Vector2[segments + 2];
        triangles = new int[segments * 3];

        //get angle per segment
        anglePerSegment = angle / segments;

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
        MeshR.material = mat;
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
        firstVector = Quaternion.AngleAxis(-angle/2, Vector3.up) * firstVector;

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
                /*
                if(Physics.Raycast(transform.position, firstVector, out hit, fovRadius, playerMask))
                {
                    Debug.Log("hit player");
                    aiAgent.canSeePlayer = true;
                    aiAgent.lastPlayerSeenPosition = aiAgent.playerRef.position;
                    aiAgent.suspisionTimer = aiAgent.suspisionTime;
                }
                */
                //Debug.Log("nothing");
                rayHits[i] = transform.position + firstVector * fovRadius;
            }
            rayHits[i] = transform.InverseTransformPoint(rayHits[i]);
            //rayHits[i] -= transform.position;
            //rayHits[i] -= transform.rotation * Vector3.forward;


            //draw raycasts
            if(i==0)
            {
                //Debug.DrawLine(transform.position, rayHits[i], Color.green);
                Debug.DrawRay(transform.position, firstVector* fovRadius, Color.black);
            }
            else
            {
                //Debug.DrawLine(transform.position, rayHits[i], Color.red);
                Debug.DrawRay(transform.position, firstVector* fovRadius, Color.blue);
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




}
