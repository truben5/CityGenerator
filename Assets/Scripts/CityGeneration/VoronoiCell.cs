using System.Collections;
using System.Collections.Generic;
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
    public List<RoadSegment> MakeRoadSpace(float roadWidth)
    {
        List<RoadSegment> roadLines = new List<RoadSegment>();

        // Uses centroid to move vertices closer to centroid
        for (int i = 0; i < vertices.Count; i++)
        {
            RoadSegment roadSegment = CreateRoadSegment(vertices[i], vertices[(i + 1) % vertices.Count], roadWidth);

            roadLines.Add(roadSegment);

            vertices[i] = PullInPolygonVertex(vertices[i], roadWidth);
        }
        return roadLines;
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

    private RoadSegment CreateRoadSegment(Vector3 start, Vector3 end, float roadWidth)
    {
        Vector3 closerStart = PullInPolygonVertex(start, roadWidth);
        Vector3 closerEnd = PullInPolygonVertex(end, roadWidth);
        Vector3 furtherStart = PushOutPolygonVertex(start, roadWidth);
        Vector3 furtherEnd = PushOutPolygonVertex(end, roadWidth);

        RoadSegment segment = new RoadSegment(closerStart, closerEnd, furtherStart, furtherEnd);
        return segment;
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
