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
    public float roadWidth;
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
        map.transform.Rotate(-90,0,0);
    }

    private void GenerateMapRegions()
    {
        mapRegions = voronoiGenerator.GetComponent<VoronoiGenerator>().GenerateVoronoi(length, width, regionNum);
    }

    // Makes space for roads and calls to create roads instance
    private void AddRoads()
    {
        List<RoadSegment> allRoads = new List<RoadSegment>();

        for (int i = 0; i < mapRegions.Count; i++)
        {
            List<RoadSegment> regionRoads = mapRegions[i].GetComponent<VoronoiCell>().MakeRoadSpace(roadWidth);

            for (int j = 0; j < regionRoads.Count; j++)
            {
                allRoads.Add(regionRoads[j]);
            }
        }

        InstantiateRoads(allRoads);
        //MakeGroundMesh();
    }

    // Instantiates GameObject and calls to create mesh
    private void InstantiateRoads(List<RoadSegment> roadLines)
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

    private void MakeGroundMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

        meshRenderer.transform.parent = map.transform;

        meshRenderer.sharedMaterial = Resources.Load("Material/RoadMaterial", typeof(Material)) as Material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        Vector3[] vertices = {
                                new Vector3(0, 0, 0),
                                new Vector3(width, 0, 0),
                                new Vector3(width, length, 0),
                                new Vector3(0, length, 0)
        };

        int[] tris = { 0, 1, 2, 2, 3, 0};
        Vector3[] normals = { -Vector3.forward , -Vector3.forward , -Vector3.forward , -Vector3.forward };
        Vector2[] uv = { 
                            new Vector2(0, 0),
                            new Vector2(width, 0),
                            new Vector2(width, length),
                            new Vector2(0, length)
        };

        mesh.vertices = vertices;

        mesh.triangles = tris;

        mesh.normals = normals;

        mesh.uv = uv;

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
