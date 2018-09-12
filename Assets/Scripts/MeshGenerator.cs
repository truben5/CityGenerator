using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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
    public Mesh CreateMesh(IEnumerable<Vector3> points)
    {
        Mesh mesh = new Mesh();

        var vertices = points.Select(x => new Vertex(x)).ToList();

        var result = ConvexHull.Create(vertices);

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
