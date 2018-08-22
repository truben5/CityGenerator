using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVoronoiCell : MonoBehaviour {

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
	
}
