using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class VoronoiCell : ResizablePolygon {

    private List<GameObject> buildingsList = new List<GameObject>();

    public GameObject cellBuilding;
    private GameObject instanceCellBuilding;

    // Returns list of all buildings contained in cell
    public List<GameObject> GetBuilding()
    {
        return buildingsList;
    }

    // Pulls in all edges of polygons to create room for roads
    // Returns lines used for roads
    public void MakeRoadSpace(float roadWidth)
    {
        // Uses centroid to move vertices closer to centroid
        for (int i = 0; i < vertices.Count; i++)
        {
            //RoadSegment roadSegment = CreateRoadSegment(vertices[i], vertices[(i + 1) % vertices.Count], roadWidth);

            //roadLines.Add(roadSegment);

            vertices[i] = PullInPolygonVertex(vertices[i], roadWidth);
        }
    }

    // Takes in array of points and creates the building objects within a cell
    public void SetBuildings(List<List<Vector3>> cellBuildings)
    {

        GameObject buildings = new GameObject();
        buildings.transform.parent = this.transform;
        buildings.name = "Buildings";

        for (int i = 0; i < cellBuildings.Count; i++)
        {
            // Only instantiate buildings with at least 4 points
            if (cellBuildings[i].Count > 3)
            {
                GameObject buildingObject = InstantiateBuilding(buildings, cellBuildings[i], i);
                buildingObject.GetComponent<Building>().CreateBuildingMesh();
            }
        }
    }

    // Creates game object instance of building 
    public GameObject InstantiateBuilding(GameObject buildingListObject, List<Vector3> cellBuildingPlot, int buildingNum)
    {
        instanceCellBuilding = Instantiate(cellBuilding);
        instanceCellBuilding.transform.parent = buildingListObject.transform;
        instanceCellBuilding.name = "building " + buildingNum;
        instanceCellBuilding.transform.position = new Vector3();

        instanceCellBuilding.GetComponent<Building>().SetVertices(cellBuildingPlot);
        instanceCellBuilding.GetComponent<Building>().ShrinkBuilding();

        buildingsList.Add(instanceCellBuilding);
        return instanceCellBuilding;
    }

    public void CreateCellMesh()
    {
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sharedMaterial = Resources.Load("Material/GroundMaterial", typeof(Material)) as Material;

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

        Mesh mesh = new Mesh();

        // Look into relative position and rotation ?
        meshRenderer.transform.position = new Vector3(0,0,.1f);
        meshRenderer.transform.rotation = new Quaternion();
        meshRenderer.transform.Rotate(90, 0, 0);

        List<Vector3> meshVertices = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();


        FormatCellMesh(meshVertices, tris, normals, uv);

        
        mesh.vertices = meshVertices.ToArray();

        mesh.triangles = tris.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uv.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    public void FormatCellMesh(List<Vector3> meshVertices, List<int> tris, List<Vector3> normals, List<Vector2> uv)
    {
        int startIndex = meshVertices.Count;

        for (int i = 0; i < vertices.Count; i++)
        {
            int currIndex = meshVertices.Count;

            meshVertices.Add(vertices[i]);

            // Determine triangles based on if it is first triangle or not
            if (i == 2)
            {
                tris.Add(startIndex);
                tris.Add(startIndex + 1);
                tris.Add(startIndex + 2);
            }
            else if (i > 2)
            {
                tris.Add(startIndex);
                tris.Add(currIndex - 1);
                tris.Add(currIndex);
            }

            normals.Add(-Vector3.forward);

            uv.Add((Vector2)vertices[i]);
        }
    }

    // Gizmos to draw out all the buildings in red
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < buildingsList.Count; i++)
    //    {
    //        List<Vector3> vertices = buildingsList[i].GetComponent<Building>().GetFloorVertices();
    //        for (int j = 0; j < vertices.Count; j++)
    //        {
    //            int k = (j + 1) % vertices.Count;

    //            Vector3 startVector = vertices[j];
    //            Vector3 endVector = vertices[k];

    //            Gizmos.DrawLine(startVector, endVector);
    //        }
    //    }
    //}
}
