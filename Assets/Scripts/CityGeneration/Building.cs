using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    private List<Vector3> flatVertices = new List<Vector3>();
    private Vector3 center = new Vector3();

    private int height;

    void Awake()
    {
        height = Random.Range(15, 40);
    }

    // Sets vertices for buildings and calculate center vector
    public void SetVertices(List<Vector3> buildingVertices)
    {
        flatVertices = buildingVertices;
        CalculateCenter(buildingVertices);
    }

    public void CalculateCenter(List<Vector3> buildingVertices)
    {
        float xSum = 0;
        float ySum = 0;

        for (int i = 0; i < buildingVertices.Count; i++)
        {
            xSum += buildingVertices[i].x;
            ySum += buildingVertices[i].y;
        }
        center.x = xSum / buildingVertices.Count;
        center.y = ySum / buildingVertices.Count;
    }

    public void CreateBuildingMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < flatVertices.Count; i++)
        {
            AddMeshWall(flatVertices[i], flatVertices[(i + 1) % flatVertices.Count], meshVertices, tris, normals, uv);
        }

        AddFlatMeshRoof(meshVertices, tris, normals, uv);

        mesh.vertices = meshVertices.ToArray();

        mesh.triangles = tris.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    // Adds a quad representing a wall between the two points passed in
    public void AddMeshWall(Vector3 v1, Vector3 v2, List<Vector3> meshVertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {

        Vector3[] vectors = {   
                                v1, 
                                v2, 
                                v2 + new Vector3(0, 0, height), 
                                v1 + new Vector3(0, 0, height) 
        };

        for (int i = 0; i < 4; i++)
        {
            meshVertices.Add(vectors[i]);
            normals.Add(-Vector3.forward);
            uv.Add((Vector2)vectors[i]);
        }

        // Add triangle for quad
        tris.Add(meshVertices.IndexOf(vectors[0]));
        tris.Add(meshVertices.IndexOf(vectors[1]));
        tris.Add(meshVertices.IndexOf(vectors[2]));

        tris.Add(meshVertices.IndexOf(vectors[2]));
        tris.Add(meshVertices.IndexOf(vectors[3]));
        tris.Add(meshVertices.IndexOf(vectors[0]));
    }

    // Uses the floor vertices to add a mesh to the top of the building
    public void AddFlatMeshRoof(List<Vector3> vertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {
        int startIndex = vertices.Count;

        for (int i=0; i < flatVertices.Count; i++)
        {
            int currIndex = vertices.Count;

            // Add height to lift the position
            Vector3 roofVertex = flatVertices[i] + new Vector3(0, 0, height);


            vertices.Add(roofVertex);

            // Determine triangles based on if it is first triangle or not
            if (i == 2)
            {
                tris.Add(startIndex);
                tris.Add(startIndex + 1);
                tris.Add(startIndex + 2);
            }
            else if (i > 2)
            {
                tris.Add(startIndex);
                tris.Add(currIndex - 1);
                tris.Add(currIndex);
            }

            normals.Add(-Vector3.forward);

            uv.Add((Vector2)roofVertex);
        }
    }

    public List<Vector3> GetFlatVertices()
    {
        return flatVertices;
    }

    public Vector3 GetCenter()
    {
        return center;
    }
}
