using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MyMeshNxM : MonoBehaviour
{
    //protected float meshLength = 8.0f;
    //protected float meshWidth = 1.0f;

    protected float meshLength = 0.8f;
    protected float meshWidth = 8.0f;
    private double rotationDegrees = 360.0;
    private float radius = 4.0f;
    private float height = 0.0f;

    protected int N = 5;
    protected int M = 5;

    protected Vector3[] verts; // vertices on the mesh
    protected int[] tris; // triangles defined by vertices
    protected Vector3[] norms; // normals at each vertex
    //protected Vector2[] uv; // uv values for texture mapping


    // Use this for initialization
    void Start()
    {
        MeshInitialization();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if ((mControllers != null) && (ManipulationOn))
        {
            UpdateMeshNormals();
        }
        */
    }

    public void SetHeight(float h)
    {
        height = h;
        MeshInitialization();
    }

    private double ConvertDegreesToRadians(double degrees)
    {
        double rad = (Math.PI / 180.0) * degrees;
        return rad;
    }

    public void MeshInitialization()
    {
        //Step 1: Obtain the mesh component and delete whatever is there
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        //Step 2: Identify the number of N, M, and Triangles
        int numTriangles = (N - 1) * (M - 1) * 2;

        //Step 3: Create arrays to store vertices in mesh, triangle vertices, and normal vectors
        verts = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        tris = new int[numTriangles * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle has 3 vertices
        norms = new Vector3[N * M];         // MUST be the same as number of vertices

        //Step 4: Define dN and dM which are the distances between each vertex in the N and M direction
        float dN = meshLength / (N - 1);
        float dM = meshWidth / (M - 1);

        //Step 5: Define a start point (lower left corner of mesh) and variable to track which triangle is being created

        //Vector3 startPoint = new Vector3(radius, -5.0f, 0.0f);
        int currentTriangle = 0;
        int curVert = 0;
        //Step 6: Compute the vertices, triangle vertices, and normal vectors at each vertex
        for (int n = 0; n < N; n++)
        {
            for (int m = 0; m < M; m++)
            {

                //Obtain an angle in radians
                double radAngle = ConvertDegreesToRadians(rotationDegrees / (M - 1));
                //Increment angle by 2 * theta at each row of vectors
                radAngle = radAngle * m;
                //Compute the x and z values
                float x = (float)(radius * Math.Cos(radAngle));
                float z = (float)(radius * Math.Sin(radAngle));
                if (curVert == 0)
                {
                    verts[n * M + m] = new Vector3(x, (n * dN) + height, z + radius);

                }
                else if (curVert == 1)
                {
                    verts[n * M + m] = new Vector3(x - radius, (n * dN) + height, z);
                }
                else if (curVert == 2)
                {
                    verts[n * M + m] = new Vector3(x, (n * dN) + height, z - radius);
                }
                else if (curVert == 3)
                {
                    verts[n * M + m] = new Vector3(x + radius, (n * dN) + height, z);
                }
                else if (curVert == 4)
                {
                    verts[n * M + m] = new Vector3(x, (n * dN) + height, z + radius);
                }
                //verts[n * M + m] = new Vector3(x, n * dN, z);
                Debug.Log(verts[n * M + m]);
                curVert++;
                if (curVert == 5)
                {
                    curVert = 0;
                }

                // process two new triangles that can be traversed from that point
                if (currentTriangle < numTriangles && m < M - 1)
                {
                    tris[currentTriangle * 3] = n * M + m;
                    tris[currentTriangle * 3 + 1] = (n + 1) * M + m;
                    tris[currentTriangle * 3 + 2] = (n + 1) * M + (m + 1);
                    currentTriangle++; // increment currentTriangle

                    tris[currentTriangle * 3] = n * M + m;
                    tris[currentTriangle * 3 + 1] = (n + 1) * M + (m + 1);
                    tris[currentTriangle * 3 + 2] = n * M + (m + 1);
                    currentTriangle++; // increment currentTriangle
                }
            }
        }
        for (int idx = 0; idx < norms.Length; idx++)
        {
            norms[idx] = new Vector3(0, 1, 0);
        }

        //Step 7: Assign the vertices, triangles, and normal vectors to the mesh
           theMesh.vertices = verts; //  new Vector3[3];
           theMesh.triangles = tris; //  new int[3];
           theMesh.normals = norms;
    }
}
