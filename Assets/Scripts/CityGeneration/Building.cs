using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : ResizablePolygon {

    private float height;

    void Awake()
    {
        height = Random.Range(15f, 30f);
    }

    // Pulls in each point a random amount towards centroid
    public void ShrinkBuilding()
    {
        for (int i= 0; i < vertices.Count; i++)
        {
            float pullAmount = Random.Range(1.0f, 4.0f);
            vertices[i] = PullInPolygonVertex(vertices[i], pullAmount);
        }
    }

    public void CreateBuildingMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = Resources.Load("Material/BuildingMaterial", typeof(Material)) as Material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < vertices.Count; i++)
        {
            AddMeshWall(vertices[i], vertices[(i + 1) % vertices.Count], meshVertices, tris, normals, uv);
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
    
    // Uses the floor vertices to add a flat mesh to the top of the building
    public void AddFlatMeshRoof(List<Vector3> meshVertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {
        int startIndex = meshVertices.Count;

        for (int i=0; i < vertices.Count; i++)
        {
            int currIndex = meshVertices.Count;

            // Add height to lift the position
            Vector3 roofVertex = vertices[i] + new Vector3(0, 0, height);

            meshVertices.Add(roofVertex);

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
}
