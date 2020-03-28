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
    public GameObject map;

    public GameObject roads;

    private List<GameObject> mapRegions = new List<GameObject>();

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

    // Makes space for roads and calls to create roads instance
    private void AddRoads()
    {
        List<Line> allRoads = new List<Line>();

        for (int i = 0; i < mapRegions.Count; i++)
        {
            List<Line> regionRoads = mapRegions[i].GetComponent<VoronoiCell>().MakeRoadSpace(roadWidth);

            for (int j = 0; j < regionRoads.Count; j++)
            {
                allRoads.Add(regionRoads[j]);
            }
        }

        InstantiateRoads(allRoads);
    }

    // Instantiates GameObject and calls to create mesh
    private void InstantiateRoads(List<Line> roadLines)
    {
        roads = Instantiate(roads);
        roads.transform.parent = map.transform;
        roads.name = "Roads";
        roads.transform.position = new Vector3();
        roads.GetComponent<RoadNetwork>().CreateRoadMesh(roadLines, roadWidth);
    }

    // Makes buildings in cells
    private void AddBuildings()
    {
        for (int i = 0; i < mapRegions.Count; i++)
        {
            List<List<Vector3>> cellBuildings = new List<List<Vector3>>();
            cellBuildings.Add(mapRegions[i].GetComponent<VoronoiCell>().GetVertices());

            cellBuildings = mapRegions[i].GetComponent<CellFill>().MakeBuildings(cellBuildings, maxBuildingLength);

            mapRegions[i].GetComponent<VoronoiCell>().SetBuildings(cellBuildings);
        }
    }
}
