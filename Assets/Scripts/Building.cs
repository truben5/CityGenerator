using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Building : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();
    private Vector2f center = new Vector2f();

    // Sets vertices for buildings as Vector3 and call CreateMesh
    public void SetVertices(List<Vector2f> buildingVertices)
    {
        vertices = buildingVertices;
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

    public List<Vector2f> GetVertices()
    {
        return vertices;
    }

    public Vector2f GetCenter()
    {
        return center;
    }
}
