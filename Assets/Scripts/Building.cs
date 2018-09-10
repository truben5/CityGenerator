using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();

    // Sets vertices for buildings as Vector3 and call CreateMesh
    public void SetVertices(List<Vector2f> buildingVertices)
    {
        vertices = buildingVertices;
    }

    public List<Vector2f> GetVertices()
    {
        return vertices;
    }
}
