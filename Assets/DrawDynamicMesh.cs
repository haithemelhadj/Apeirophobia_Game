using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawDynamicMesh : MonoBehaviour
{
    public AiAgentTry3 aiAgent;
    public Transform playerRef;

    private Mesh myMesh;
    public float Radius;//-------------------------------------
    public float angle;//-------------------------------------

    public float segments = 5;//-------------------------------------
    private float segmentAngle;
    public float[] hitDistances;//-------------------------------------
    public Vector3[] hitPositions;//-------------------------------------

    private Vector3[] verts;
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uvs;

    private float actualAngle;

    public float coifissent;//-------------------------------------

    public Material mat;//-------------------------------------

    void Start()
    {
        Radius = aiAgent.fovRadius * coifissent * (1 / aiAgent.gameObject.transform.localScale.x);// calculate the mech foc raduis
        angle = aiAgent.fovAngle * 0.5f;
        //hitDistances.Length = segments;

        var MeshF = gameObject.AddComponent<MeshFilter>();
        var MeshR = gameObject.AddComponent<MeshRenderer>();
        var MeshMC = gameObject.AddComponent<MeshCollider>();

        MeshR.material = mat;
        //go.renderer.material.mainTexture = Resources.Load("glass", typeof(Texture2D));
        //AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        //MESH
        myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        //--------------------------------INITIALIZE------------------------------------------

        // Calculate actual pythagorean angle
        actualAngle = 90.0f - angle;
        // Segment Angle
        segmentAngle = angle * 2 / segments;
        // Initialise the array lengths
        verts = new Vector3[(int)segments + 1];
        normals = new Vector3[(int)segments + 1];
        triangles = new int[(int)segments * 3];
        uvs = new Vector2[(int)segments +1];
        // Initialise the Array to origin Points
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = new Vector3(0, 0, 0);
            normals[i] = Vector3.up;
        }

        //--------------------------------------------------------------------------

        //BUILD THE MESH
        buildMesh();
        MeshMC.convex = true;
        MeshMC.isTrigger = true;

        MeshMC.sharedMesh = myMesh;
    }

    private void Update()
    {
        buildMesh();
    }

    void buildMesh()
    {
        // Grab the Mesh off the gameObject
        //myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        //Clear the mesh
        myMesh.Clear();




        // Create a dummy angle
        float a = actualAngle;


        //--------------------------------------------------------------
        //create the hit positions
        RaycastHit hit;
        Vector3 firstVector= new Vector3(a, 0, a);
        for (int g = 0; g < segments; g++)
        {
            Physics.Raycast(transform.position, firstVector, out hit, Radius, aiAgent.obstructionMask);
            hitPositions[g] = hit.point;
            Debug.Log(hit.point);
            //rotate the vector by segment angle
            firstVector = Quaternion.Euler(0, segmentAngle, 0) * firstVector;
        }
        // create the vertics and uvs
        verts[0]= aiAgent.eyes.position;
        for (int i = 1; i < verts.Length; i ++)
        {
            verts[i] = hitPositions[i - 1];
            //uvs[i]= hitPositions[i];
        }
        // Generate planar UV Coordinates
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x, verts[i-1].z);
        }
        //create triangles 
        for (int i = 0; i < triangles.Length; i+=3)
        {
            triangles[i] = 0;
            triangles[i + 1] = i + 1;
            triangles[i + 2] = i + 2;
        }

        /*
        //send all raycasts
        for (int j = 0; j < segments; j++)
        {
            if (Physics.Raycast(transform.position, playerRef.position, out hit, Radius, aiAgent.obstructionMask)) 
            {
                
                Debug.Log("raycast hit something");
                hitDistances[j] = hit.distance;

            }
            else
            {
                hitDistances[j] = Radius;
                Debug.Log("raycast hit nothing");
            }
        }
        /*
        */
        //--------------------------------------------------------------
        /*
        // Create the Vertices
        for (int i = 1; i < verts.Length; i += 3)
        {

            verts[i] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * Radius, // x
                                                  0,                                                                // y
                                                  Mathf.Sin(Mathf.Deg2Rad * a) * Radius);  // z

            a += segmentAngle; //print(a);

            verts[i + 1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * Radius, // x
                                                      0,                                                                // y
                                                      Mathf.Sin(Mathf.Deg2Rad * a) * Radius);  // z
        }

        // Create Triangle
        for (int i = 0; i < triangles.Length; i += 3)
        {
            triangles[i] = 0;
            triangles[i + 1] = i + 2;
            triangles[i + 2] = i + 1;
        }
        */
        

        // Put all these back on the mesh
        myMesh.vertices = verts;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uvs;
    }
}
