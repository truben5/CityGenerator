using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MIConvexHull;

public class Building : MonoBehaviour {

    private List<Vector2f> vertices;
	
    public void SetVertices(List<Vector2f> buildingVertices)
    {
        vertices = buildingVertices;
        CreateMesh(vertices);
    }

    public void CreateMesh(List<Vector2f> buildingVertices)
    {
        
    }
}
