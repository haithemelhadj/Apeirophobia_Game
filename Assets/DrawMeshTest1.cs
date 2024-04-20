using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class DrawMeshTest1 : MonoBehaviour
{
    public GameObject GameObject;

    public int segments = 5;
    public float fovRadius = 5;
    public float angle;

    public Vector3[] rayHits;
    Vector3[] vertics;
    Vector2[] uvs;
    int[] triangles;

    RaycastHit hit;

    private Mesh mesh;

    public LayerMask obstructionMask;

    private void Start()
    {
        rayHits = new Vector3[segments + 1];
        vertics = new Vector3[segments + 2];
        uvs = new Vector2[segments + 2];
        triangles = new int[segments * 3];
        

        var MeshF = gameObject.AddComponent<MeshFilter>();
        var MeshR = gameObject.AddComponent<MeshRenderer>();
        var MeshMC = gameObject.AddComponent<MeshCollider>();

        //go.renderer.material.mainTexture = Resources.Load("glass", typeof(Texture2D));
        //AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        //MESH
        mesh = gameObject.GetComponent<MeshFilter>().mesh;

        //BUILD THE MESH
        MeshMC.convex = true;
        MeshMC.isTrigger = true;

        MeshMC.sharedMesh = mesh;

    }
    private void CreateMesh()
    {
        mesh.Clear();
        //get angle per segment
        float anglePerSegment = angle / segments;
        //get first vector
        Vector3 firstVector = transform.forward;
        float startAngle =  90.0f + angle / 2;
        float a = startAngle;
        firstVector = new Vector3(a, 0, a);
        firstVector = Quaternion.Euler(0, a, 0) * firstVector;

        
        //send raycasts to get the hit positions
        for (int i = 0; i < rayHits.Length; i++)
        {
            if (Physics.Raycast(GameObject.transform.position, firstVector, out hit, fovRadius, obstructionMask))
            {
                Debug.Log("hit ground");
                rayHits[i] = hit.point;
            }
            else
            {
                Debug.Log("nothing");
                rayHits[i] = GameObject.transform.position + (firstVector / a) * fovRadius + transform.forward;
            }
            if(i==0)
                Debug.DrawLine(GameObject.transform.position, rayHits[i], Color.green);
            else
            Debug.DrawLine(GameObject.transform.position, rayHits[i], Color.red);
            firstVector = Quaternion.Euler(0, anglePerSegment, 0) * firstVector;
        }
        /*
        */
        //----------------------------------------------------------------
        /*
        //Vector3[] vertics;
        // Initialise the Array to origin Points
        for (int i = 0; i < vertics.Length; i++)
        {
            vertics[i] = new Vector3(0, 0, 0);
            //normals[i] = Vector3.up;
        }

        // Create a dummy angle
        float actualAngle = 90.0f - angle/2;
        float segmentAngle = angle * 2 / segments;
        a = actualAngle;

        // Create the Vertices
        for (int i = 1; i < vertics.Length; i ++)
        {
            vertics[i] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * fovRadius, // x
                                                  0,                                                                // y
                                                  Mathf.Sin(Mathf.Deg2Rad * a) * fovRadius);  // z

            a += segmentAngle;

            vertics[i + 1] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * fovRadius, // x
                                                      0,                                                                // y
                                                      Mathf.Sin(Mathf.Deg2Rad * a) * fovRadius);  // z          
        }
        /*
        */
        //----------------------------------------------------------------


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

        //triangles
        int counter = 0;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            triangles[i] = 0;
            Debug.Log("tri: " + triangles[i]);
            triangles[i + 1] = counter + 1;
            Debug.Log("tri: " + triangles[i + 1]);
            triangles[i + 2] = counter + 2;
            Debug.Log("tri: " + triangles[i + 2]);
            counter++;
        }




        //now draw mesh
        mesh.Clear();
        mesh.vertices = this.vertics;
        mesh.uv = uvs;
        mesh.triangles = triangles;
    }

    private void Update()
    {
        CreateMesh();
    }

}
