using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using csDelaunay;

public class MapGenerator : MonoBehaviour
{

    public int width;
    public int length;
    public int regionNum;
    public int maxBuildingLength;
    public int roadWidth;
    public bool walls;
    public GameObject voronoiGenerator;

    public GameObject road;

    private List<GameObject> mapRegions = new List<GameObject>();
    private List<GameObject> roads = new List<GameObject>();

    void Start()
    {
        GenerateMapRegions();
        AddRoads();
        AddBuildings();
    }

    private void GenerateMapRegions()
    {
        mapRegions = voronoiGenerator.GetComponent<VoronoiGenerator>().GenerateVoronoi(length, width, regionNum);
    }

    private void AddRoads()
    {
        for (int i = 0; i < mapRegions.Count; i++)
        {
            mapRegions[i].GetComponent<VoronoiCell>().CellShrink(roadWidth);
        }
    }

    // Makes buildings in cells
    private void AddBuildings()
    {
        for (int i = 0; i < mapRegions.Count; i++)
        {
            List<List<Vector3>> cellBuildings = new List<List<Vector3>>();
            // Use the vertices for buildings because they provide room for roads
            cellBuildings.Add(mapRegions[i].GetComponent<VoronoiCell>().GetCellVertices());

            mapRegions[i].GetComponent<VoronoiCell>().SetBuildings(mapRegions[i].GetComponent<CellFill>().MakeBuildings(cellBuildings, maxBuildingLength));
        }
    }
}
