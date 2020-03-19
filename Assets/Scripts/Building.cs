using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    private List<Vector2f> twoDimensionVertices = new List<Vector2f>();
    private Vector2f center = new Vector2f();

    private List<Vector3> threeDimensionVertices = new List<Vector3>();

    private int height = 40;

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

        List<Vector3> vertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < threeDimensionVertices.Count; i++)
        {
            CalculateMeshWall(threeDimensionVertices[i], threeDimensionVertices[(i + 1) % threeDimensionVertices.Count], vertices, tris, normals, uv);
        }

        mesh.vertices = vertices.ToArray();

        mesh.triangles = tris.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uv.ToArray();

        //mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public void CalculateMeshWall(Vector3 v1, Vector3 v2, List<Vector3> vertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {
        Vector3 v3 = v2 + new Vector3(0, 0, height);
        Vector3 v4 = v1 + new Vector3(0, 0, height);

        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);

        tris.Add(vertices.IndexOf(v1));
        tris.Add(vertices.IndexOf(v2));
        tris.Add(vertices.IndexOf(v3));

        tris.Add(vertices.IndexOf(v3));
        tris.Add(vertices.IndexOf(v4));
        tris.Add(vertices.IndexOf(v1));

        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);

        uv.Add((Vector2)v1);
        uv.Add((Vector2)v2);
        uv.Add((Vector2)v3);
        uv.Add((Vector2)v4);
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
