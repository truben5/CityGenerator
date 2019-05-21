using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();

    public List<Vector2f> getCellVertices()
    {
        return this.vertices;
    }

    public void setCellVertices(List<Vector2f> regionVertices)
    {
        for (int i=0; i < regionVertices.Count; i++)
        {
            vertices.Add(regionVertices[i]);
        }
        //Debug.Log(vertices[0]);
    }

    // Alters the cell perimeter to accomodate space for roads
    public void MakeRoadSpace()
    {
        Vector2f centroid = getCentroid();

        //Use cross product to find direction of centroid











    }

    // Find the average x and y value from the cell to find the centroid
    private Vector2f getCentroid()
    {
        float xSum = 0;
        float ySum = 0;

        for (int i = 0; i < vertices.Count; i++)
        {
            xSum += vertices[i].x;
            ySum += vertices[i].y;
        }

        float xAvg = xSum / vertices.Count;
        float yAvg = ySum / vertices.Count;

        return new Vector2f(xAvg, yAvg);
    }
	
}
