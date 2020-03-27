using System.Collections.Generic;
using UnityEngine;
using csDelaunay;


public class VoronoiGenerator : MonoBehaviour {

    public GameObject voronoiCell;
    public GameObject parentDiagram;

    private List<GameObject> cells = new List<GameObject>();
    private List<GameObject> roads = new List<GameObject>();

    // Generates the voronoi diagram from cs Delaunay. Saves the array of vertices for each cell in cells
    public List<GameObject> GenerateVoronoi(int length, int width, int cellNum)
    {
        Rectf bounds = new Rectf(0, 0, length, width);
        List<Vector2f> points = CreateRandomPoints(length, width, cellNum);
        // Creates diagram
        // Point array, bounds, number of lloyd iterations
        Voronoi voronoi = new Voronoi(points, bounds, 3);

        for (int i=0; i < points.Count; i++)
        {
            voronoiCell = Instantiate(voronoiCell);
            voronoiCell.transform.parent = parentDiagram.transform;
            voronoiCell.name = "cell " + i;

            // If performance issues this is a place that might be able to be optimized
            List<Vector3> cellVertices = ConvertToUnityVectors(voronoi.Regions()[i]);
            voronoiCell.GetComponent<VoronoiCell>().SetVertices(cellVertices);

            Vector3 centroid = voronoiCell.GetComponent<VoronoiCell>().GetCentroid();
            voronoiCell.transform.position = new Vector3(centroid.x, centroid.y, 0);

            cells.Add(voronoiCell);
        }

        return cells;
    }

    // Randomly creates a 2d vector in range of length and width
    private List<Vector2f> CreateRandomPoints(int length, int width, int cellNum)
    {
        List<Vector2f> points = new List<Vector2f>();
        for (int i=0; i < cellNum; i++)
        {
            points.Add(new Vector2f(Random.Range(0, length), Random.Range(0, width)));
        }
        return points;
    }

    private List<Vector3> ConvertToUnityVectors(List<Vector2f> vectors)
    {
        List<Vector3> unityVectors = new List<Vector3>();
        for (int i = 0; i < vectors.Count; i++)
        {
            Vector3 newVector = new Vector3(vectors[i].x, vectors[i].y, 0);
            unityVectors.Add(newVector);
        }

        return unityVectors;
    }

    // Gizmos to draw out all the voronoi cells in black
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.black;
    //    for (int i = 0; i < cells.Count; i++)
    //    {
    //        List<Vector2f> cellVertices = cells[i].GetComponent<VoronoiCell>().GetCellVertices();

    //        for (int j = 0; j < cellVertices.Count; j++)
    //        {
    //            int z = (j + 1) % cellVertices.Count;

    //            Vector3 startVector = new Vector3(cellVertices[j].x, cellVertices[j].y, 0);
    //            Vector3 endVector = new Vector3(cellVertices[z].x, cellVertices[z].y, 0);
    //            Gizmos.DrawLine(startVector, endVector);
    //        }
    //    }
    //}
}
