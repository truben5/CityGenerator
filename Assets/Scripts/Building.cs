using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    private List<Vector2f> twoDimensionVertices = new List<Vector2f>();
    private Vector2f center = new Vector2f();

    private List<Vector3> threeDimensionVertices = new List<Vector3>();

    // Sets vertices for buildings as Vector3 and call CreateMesh
    public void SetVertices(List<Vector2f> buildingVertices)
    {
        twoDimensionVertices = buildingVertices;
        float xSum = 0;
        float ySum = 0;

        for (int i = 0; i < buildingVertices.Count; i++)
        {
            xSum += buildingVertices[i].x;
            ySum += buildingVertices[i].y;

            // Creates 3d vertices for each 2d
            Vector3 threeDVertex = new Vector3(buildingVertices[i].x, buildingVertices[i].y, 0);
            threeDimensionVertices.Add(threeDVertex);
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


        // TODO: Replace this with building vertices
        Vector3[] vertices = new Vector3[4]
        {
            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0)
        };
        mesh.vertices = vertices;

        // TODO: Look into this property, may need to alter based on vertices
        int[] tris = new int[6]
        {
            // lower left triangle
            0, 2, 1,
            // upper right triangle
            2, 3, 1
        };
        mesh.triangles = tris;

        // TODO: Look into this property, may need to alter based on vertices
        Vector3[] normals = new Vector3[4]
        {
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward,
            -Vector3.forward
        };
        mesh.normals = normals;

        // TODO: Look into this property, may need to alter based on vertices
        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        mesh.uv = uv;

        meshFilter.mesh = mesh;
    }

    public List<Vector2f> GetTwoDimensionVertices()
    {
        return twoDimensionVertices;
    }

    public Vector2f GetCenter()
    {
        return center;
    }
}
