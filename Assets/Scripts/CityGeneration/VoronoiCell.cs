using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : Structure {

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
    public List<Line> MakeRoadSpace(int roadWidth)
    {
        Vector2f diffVector = new Vector2f();
        List<Line> roadLines = new List<Line>();

        // Uses centroid to move vertices closer to centroid
        for (int i = 0; i < vertices.Count; i++)
        {
            Line roadSegment = new Line(vertices[i], vertices[(i + 1) % vertices.Count]);
            roadLines.Add(roadSegment);

            diffVector.x = vertices[i].x - centroid.x;
            diffVector.y = vertices[i].y - centroid.y;
            diffVector.Normalize();

            vertices[i] = new Vector3(vertices[i].x - roadWidth * diffVector.x, vertices[i].y - roadWidth * diffVector.y, 0);
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
            GameObject buildingObject = InstantiateBuilding(buildings, cellBuildings[i], i);
            buildingObject.GetComponent<Building>().CreateBuildingMesh();
        }
    }

    // Creates game object instance of building and sets position to the center of the building vertices
    public GameObject InstantiateBuilding(GameObject buildingListObject, List<Vector3> cellBuildingPlot, int buildingNum)
    {
        instanceCellBuilding = Instantiate(cellBuilding);
        instanceCellBuilding.transform.parent = buildingListObject.transform;
        instanceCellBuilding.name = "building " + buildingNum;
        instanceCellBuilding.GetComponent<Building>().SetVertices(cellBuildingPlot);

        // Sets position to the center of the building polygon
        Vector3 center = instanceCellBuilding.GetComponent<Building>().GetCenter();

        instanceCellBuilding.transform.position = new Vector3();

        buildingsList.Add(instanceCellBuilding);
        return instanceCellBuilding;
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
