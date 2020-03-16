using System.Collections.Generic;
using UnityEngine;
using csDelaunay;


public class VoronoiGenerator : MonoBehaviour {

    public int cellNum;
    public int width;
    public int length;
    public int maxLength;
    public GameObject voronoiCell;
    public GameObject parentDiagram;

    private List<GameObject> cells = new List<GameObject>();

    void Start()
    {
        GenerateVoronoi();
        AddRoads();
        AddBuildings();

    }

    // Generates the voronoi diagram from cs Delaunay. Saves the array of vertices for each cell in cells
    public void GenerateVoronoi()
    {
        Rectf bounds = new Rectf(0, 0, length, width);
        List<Vector2f> points = CreateRandomPoints();
        // Creates diagram
        // Point array, bounds, number of lloyd iterations
        Voronoi voronoi = new Voronoi(points, bounds, 3);

        for (int i=0; i < points.Count; i++)
        {
            voronoiCell = Instantiate(voronoiCell);
            voronoiCell.transform.parent = parentDiagram.transform;
            voronoiCell.name = "cell " + i;
            voronoiCell.GetComponent<VoronoiCell>().SetCellVertices(voronoi.Regions()[i]);

            Vector2f centroid = voronoiCell.GetComponent<VoronoiCell>().GetCentroid();
            voronoiCell.transform.position = new Vector3(centroid.x, centroid.y, 0);

            //Debug.Log(this.cells);
            cells.Add(voronoiCell);
        }
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

    private void AddRoads()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<VoronoiCell>().CellShrink();
        }
    }
    // Make space between each cell for the roads
   /* private void createRoads()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].GetComponent<VoronoiCell>().MakeRoadSpace();
        }
    }
    */
    // Makes buildings in cells
    private void AddBuildings()
    {
        for (int i=0; i < cells.Count; i++)
        {
            List<List<Vector2f>> cellBuildings = new List<List<Vector2f>>();
            // Use the vertices for buildings because they provide room for roads
            cellBuildings.Add(cells[i].GetComponent<VoronoiCell>().GetVerticesForBuildings());
            //for (int j=0; j < cellVertices.Count; j++)
            //{
            //    Debug.Log("Cell " + i + ": " + cellVertices[j].x + ", " + cellVertices[j].y);
            //}
            cells[i].GetComponent<VoronoiCell>().SetBuildings(cells[i].GetComponent<CellFill>().MakeBuildings(cellBuildings, maxLength));
        }
    }

    ////Draws gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        for (int i = 0; i < cells.Count; i++)
        {
            List<Vector2f> cellVertices = cells[i].GetComponent<VoronoiCell>().GetCellVertices();

            for (int j = 0; j < cellVertices.Count; j++)
            {
                int z = (j + 1) % cellVertices.Count;

                Vector3 startVector = new Vector3(cellVertices[j].x, cellVertices[j].y, 0);
                Vector3 endVector = new Vector3(cellVertices[z].x, cellVertices[z].y, 0);
                Gizmos.DrawLine(startVector, endVector);
            }
        }
    }
}
