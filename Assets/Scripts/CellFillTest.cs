using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFillTest : MonoBehaviour {

    public GameObject voronoiCell;

    // Use this for initialization
    void Start()
    {
        voronoiCell = Instantiate(voronoiCell);

        List<Vector2f> testVertices = AddTestPoints();
    //voronoiCell.GetComponent<VoronoiCell>().setCellVertices(testVertices);
    float dist = voronoiCell.GetComponent<CellFill>().Distance(testVertices[0].x, testVertices[0].y, testVertices[1].x, testVertices[1].y);
        if (dist != Mathf.Sqrt(200))
        {
            throw new System.Exception("Expected " + Mathf.Sqrt(200) + " but received " + dist);
        }
	}

    private List<Vector2f> AddTestPoints()
    {
        List<Vector2f> vertices = new List<Vector2f>();
        int[] x = new int[] { 10, 20, 30, 40, 40, 30, 20, 10 };
        int[] y = new int[] { 20, 10, 10, 20, 30, 40, 40, 30 } ;

        for (int i=0; i < x.Length; i++)
        {
            Vector2f vertex = new Vector2f(x[i],y[i]);
            vertices.Add(vertex);
        }
        return vertices;
    }

}
