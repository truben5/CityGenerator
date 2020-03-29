using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadNetwork : MonoBehaviour
{
    private List<RoadSegment> roads = new List<RoadSegment>();

    public void CreateRoadMesh(List<RoadSegment> roadSegments, float roadWidth)
    {
        roads = roadSegments;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = Resources.Load("Material/RoadMaterial", typeof(Material)) as Material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();

        for (int i = 0; i < roadSegments.Count; i++)
        {
            AddMeshSegment(roadWidth, roadSegments[i], meshVertices, tris, normals, uv);
        }

        mesh.vertices = meshVertices.ToArray();

        mesh.triangles = tris.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    private void AddMeshSegment(float roadWidth, RoadSegment segment, List<Vector3> meshVertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {
        Vector3 leftDiffVector = new Vector3(0, -roadWidth, 0);
        Vector3 rightDiffVector = new Vector3(0, roadWidth, 0);

        Vector3[] vectors = {
                                segment.furtherLine.GetStart(),
                                segment.furtherLine.GetEnd(),
                                segment.closerLine.GetEnd(),
                                segment.closerLine.GetStart()
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
}
