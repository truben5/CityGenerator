using System.Collections.Generic;
using UnityEngine;
using csDelaunay;


public class VoronoiGenerator : MonoBehaviour {

    public int cellNum;
    public int width;
    public int length;
    public GameObject voronoiCell;
    public GameObject parentDiagram;

    private List<List<Vector2f>> cells = new List<List<Vector2f>>();

    void Start()
    {
        GenerateVoronoi();
        //OnDrawGizmos();
    }

    void GenerateVoronoi()
    {
        Rectf bounds = new Rectf(0, 0, length, width);
        List<Vector2f> points = CreateRandomPoints();
        Voronoi voronoi = new Voronoi(points, bounds, 3);

        for (int i=0; i < voronoi.Regions().Count; i++)
        {
            voronoiCell = Instantiate(voronoiCell);
            //voronoiCell.transform.SetParent(parentDiagram);
            voronoiCell.name = "cell " + i;
            voronoiCell.GetComponent<MakeVoronoiCell>().setCellVertices(voronoi.Regions()[i]);
            //Debug.Log(this.cells);
            //Debug.Log(voronoiCell.GetComponent<MakeVoronoiCell>().getCellVertices());
            cells.Add(voronoiCell.GetComponent<MakeVoronoiCell>().getCellVertices());
        }

        //Debug.Log(voronoiCell.GetComponent<MakeVoronoiCell>().getCellVertices());
    }

    private List<Vector2f> CreateRandomPoints()
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i=0; i < cellNum; i++)
        {
            points.Add(new Vector2f(Random.Range(0, length), Random.Range(0, width)));
        }
        return points;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i=0; i < cells.Count; i++)
        {
            List<Vector2f> cellVertices = cells[i];
            
            for (int j = 0; j < cells[i].Count; j++)
            {
                int z = j + 1;
                if (z > cells[i].Count-1)
                {
                    z = 0;
                }
                Vector3 startVector = new Vector3(cellVertices[j].x, cellVertices[j].y, 0);
                Vector3 endVector = new Vector3(cellVertices[z].x, cellVertices[z].y, 0);
                Gizmos.DrawLine(startVector, endVector);
            }
            
        }
    }

}
