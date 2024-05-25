using UnityEngine;

public class DrawFovMesh : MonoBehaviour
{
    public Enemy enemyScript;
    public AiAgentTry3 aiAgent;


    private Mesh myMesh;
    public float Radius;//-------------------------------------
    public float angle;//-------------------------------------

    public float segments = 5;//-------------------------------------
    private float segmentAngle;

    private Vector3[] verts;
    private Vector3[] normals;
    private int[] triangles;
    private Vector2[] uvs;

    private float actualAngle;

    public float coifissent;//-------------------------------------

    public Material mat;//-------------------------------------

    void Start()
    {
        Radius = enemyScript.radius * coifissent * (1 / enemyScript.gameObject.transform.localScale.x);// calculate the mech foc raduis
        angle = enemyScript.fovAngle * 0.5f;

        var MeshF = gameObject.AddComponent<MeshFilter>();
        var MeshR = gameObject.AddComponent<MeshRenderer>();
        var MeshMC = gameObject.AddComponent<MeshCollider>();

        MeshR.material = mat;
        //go.renderer.material.mainTexture = Resources.Load("glass", typeof(Texture2D));
        //AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        //MESH
        myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        //BUILD THE MESH
        buildMesh();
        MeshMC.convex = true;
        MeshMC.isTrigger = true;

        MeshMC.sharedMesh = myMesh;
    }

    void buildMesh()
    {
        // Grab the Mesh off the gameObject
        //myMesh = gameObject.GetComponent<MeshFilter>().mesh;

        //Clear the mesh
        myMesh.Clear();

        // Calculate actual pythagorean angle
        actualAngle = 90.0f - angle;

        // Segment Angle
        segmentAngle = angle * 2 / segments;

        // Initialise the array lengths
        verts = new Vector3[(int)segments * 3];
        normals = new Vector3[(int)segments * 3];
        triangles = new int[(int)segments * 3];
        uvs = new Vector2[(int)segments * 3];

        // Initialise the Array to origin Points
        for (int i = 0; i < verts.Length; i++)
        {
            verts[i] = new Vector3(0, 0, 0);
            normals[i] = Vector3.up;
        }

        // Create a dummy angle
        float a = actualAngle;

        // Create the Vertices
        for (int i = 1; i < verts.Length; i += 3)
        {
            verts[i] = new Vector3(Mathf.Cos(Mathf.Deg2Rad * a) * Radius, // x
                                                  0,                                                                // y
                                                  Mathf.Sin(Mathf.Deg2Rad * a) * Radius);  // z

            a += segmentAngle;

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

        // Generate planar UV Coordinates
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(verts[i].x, verts[i].z);
        }

        // Put all these back on the mesh
        myMesh.vertices = verts;
        myMesh.normals = normals;
        myMesh.triangles = triangles;
        myMesh.uv = uvs;
    }
}
