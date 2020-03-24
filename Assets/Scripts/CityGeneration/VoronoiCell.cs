using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector3> cellVertices = new List<Vector3>();
    private List<GameObject> buildingsList = new List<GameObject>();

    private Vector3 centroid = new Vector3();

    public GameObject cellBuilding;
    private GameObject instanceCellBuilding;

    public List<Vector3> GetCellVertices()
    {
        return this.cellVertices;
    }

    // Sets vertices of voronoi cell and calculates centroid
    public void SetCellVertices(List<Vector2f> regionVertices)
    {

        float xSum = 0;
        float ySum = 0;

        for (int i = 0; i < regionVertices.Count; i++)
        {
            // Convert to Vector3 here
            Vector3 vertex = new Vector3(regionVertices[i].x, regionVertices[i].y, 0);

            cellVertices.Add(vertex);
            xSum += regionVertices[i].x;
            ySum += regionVertices[i].y;
        }

        centroid.x = xSum / regionVertices.Count;
        centroid.y = ySum / regionVertices.Count;
    }

    // Returns list of all buildings contained in cell
    public List<GameObject> GetBuilding()
    {
        return buildingsList;
    }

    public void CreateRoads(int roadWidth)
    {
        CellShrink(roadWidth);
    }

    // Pulls in all edges of polygons to create room for roads
    public void CellShrink(int roadWidth)
    {
        Vector2f diffVector = new Vector2f();

        // Uses centroid to move vertices closer to centroid
        for (int i = 0; i < cellVertices.Count; i++)
        {
            diffVector.x = cellVertices[i].x - centroid.x;
            diffVector.y = cellVertices[i].y - centroid.y;
            diffVector.Normalize();

            cellVertices[i] = new Vector3(cellVertices[i].x - roadWidth * diffVector.x, cellVertices[i].y - roadWidth * diffVector.y, 0);
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

    public Vector3 GetCentroid()
    {
        return centroid;
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
