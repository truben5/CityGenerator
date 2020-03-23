﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector2f> cellVertices = new List<Vector2f>();
    private List<GameObject> buildingsList = new List<GameObject>();

    private Vector2f centroid = new Vector2f();

    public GameObject cellBuilding;
    private GameObject instanceCellBuilding;

    public List<Vector2f> GetCellVertices()
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
            cellVertices.Add(regionVertices[i]);
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

            cellVertices[i] = new Vector2f(cellVertices[i].x - roadWidth * diffVector.x, cellVertices[i].y - roadWidth * diffVector.y);
        }
    }

    // Takes in array of points and creates the building objects within a cell
    public void SetBuildings(List<List<Vector2f>> cellBuildings)
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
    public GameObject InstantiateBuilding(GameObject buildingListObject, List<Vector2f> cellBuildingPlot, int buildingNum)
    {
        instanceCellBuilding = Instantiate(cellBuilding);
        instanceCellBuilding.transform.parent = buildingListObject.transform;
        instanceCellBuilding.name = "building " + buildingNum;
        instanceCellBuilding.GetComponent<Building>().SetVertices(cellBuildingPlot);

        // Sets position to the center of the building polygon
        Vector2f center = instanceCellBuilding.GetComponent<Building>().GetCenter();

        instanceCellBuilding.transform.position = new Vector3();

        buildingsList.Add(instanceCellBuilding);
        return instanceCellBuilding;
    }

    public Vector2f GetCentroid()
    {
        return centroid;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < buildingsList.Count; i++)
        {
            List<Vector3> vertices = buildingsList[i].GetComponent<Building>().GetFloorVertices();
            for (int j = 0; j < vertices.Count; j++)
            {
                int k = (j + 1) % vertices.Count;

                Vector3 startVector = vertices[j];
                Vector3 endVector = vertices[k];

                Gizmos.DrawLine(startVector, endVector);
            }
        }
    }
}
