using System.Collections.Generic;
using UnityEngine;
using csDelaunay;


public class VoronoiGenerator : MonoBehaviour {

    public int cellNum;
    public int width;
    public int length;
    public GameObject voronoiCell;
    public GameObject parentDiagram;

    private List<GameObject> cells = new List<GameObject>();

    void Start()
    {
        GenerateVoronoi();
        AddBuildings();
    }

    // Generates the voronoi diagram from cs Delaunay. Saves the array of vertices for each cell in cells
    public void GenerateVoronoi()
    {
        Rectf bounds = new Rectf(0, 0, length, width);
        List<Vector2f> points = CreateRandomPoints();
        Voronoi voronoi = new Voronoi(points, bounds, 2);

        for (int i=0; i < voronoi.Regions().Count; i++)
        {
            voronoiCell = Instantiate(voronoiCell);
            voronoiCell.transform.parent = parentDiagram.transform;
            voronoiCell.name = "cell " + i;
            voronoiCell.GetComponent<VoronoiCell>().setCellVertices(voronoi.Regions()[i]);
            //Debug.Log(this.cells);
            cells.Add(voronoiCell);
        }

        //Debug.Log(voronoiCell.GetComponent<MakeVoronoiCell>().getCellVertices());
    }

    // Randomly creates a 2d vector in range of length and width
    private List<Vector2f> CreateRandomPoints()
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i=0; i < cellNum; i++)
        {
            points.Add(new Vector2f(Random.Range(0, length), Random.Range(0, width)));
        }
        return points;
    }

    // Makes buildings in cells
    private void AddBuildings()
    {
        for (int i=0; i < cells.Count; i++)
        {
            List<Vector2f> cellVertices = cells[i].GetComponent<VoronoiCell>().getCellVertices();
            for (int j=0; j < cellVertices.Count; j++)
            {
                Debug.Log("Cell " + i + ": " + cellVertices[j].x + ", " + cellVertices[j].y);
            }
            cells[i].GetComponent<CellFill>().MakeBuildings(cellVertices, 5);
        }
    }

    // Draws gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i=0; i < cells.Count; i++)
        {
            List<Vector2f> cellVertices = cells[i].GetComponent<VoronoiCell>().getCellVertices();
            
            for (int j = 0; j < cellVertices.Count; j++)
            {
                int z = j + 1;
                if (z > cellVertices.Count-1)
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
