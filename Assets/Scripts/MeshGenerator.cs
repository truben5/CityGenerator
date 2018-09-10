using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    private MeshFilter filter;

    void Start()
    {
        filter = GetComponent<MeshFilter>();
        //filter.mesh = CreateMesh();
    }

    // Uses the buidling vertices to create a building mesh
    public Mesh CreateMesh(List<Vector2> v)
    {
        MIConvexHull.IVertex n = new IVertex(); 
        Mesh mesh = new Mesh();
        
        var convexHull = ConvexHull.Create(v);
        List<Vector3> vertices = Make3D(v);
        mesh.SetVertices(vertices);

        //mesh.SetTriangles();
         
        return mesh;
    }

    public List<Vector3> Make3D(List<Vector2f> v)
    {
        List<Vector3> vertices = new List<Vector3>();
        Vector3 vector;
        for (int i = 0; i < v.Count; i++)
        {
            vector.x = v[i].x;
            vector.y = v[i].y;
            vector.z = 0;
            vertices.Add(vector);
        }
        return vertices;
    }
}
