using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();
    private List<Vector2f> verticesForBuildings = new List<Vector2f>();
    private List<GameObject> buildingsList = new List<GameObject>();


    public GameObject cellBuilding;
    private GameObject instanceCellBuilding;

    public List<Vector2f> GetCellVertices()
    {
        return this.vertices;
    }

    public List<Vector2f> GetVerticesForBuildings()
    {
        return this.verticesForBuildings;
    }

    // Sets vertices of voronoi cell
    public void SetCellVertices(List<Vector2f> regionVertices)
    {
        for (int i = 0; i < regionVertices.Count; i++)
        {
            vertices.Add(regionVertices[i]);
        }
        //Debug.Log(vertices[0]);
    }

    // Returns list of all buildings contained in cell
    public List<GameObject> GetBuilding()
    {
        return buildingsList;
    }

    // Pulls in all edges of polygons to create room for roads
    public void CellShrink()
    {
        Vector2f centroid = CalculateCentroid();
        Vector2f diffVector = new Vector2f();

        int roadWidth = 3;
        // Uses centroid to move vertices 1 closer to centroid
        for (int i = 0; i < vertices.Count; i++)
        {
            diffVector.x = vertices[i].x - centroid.x;
            diffVector.y = vertices[i].y - centroid.y;
            diffVector.Normalize();

            //vertices[i] = new Vector2f(vertices[i].x - roadWidth*diffVector.x, vertices[i].y - roadWidth*diffVector.y);
            verticesForBuildings.Add(new Vector2f(vertices[i].x - roadWidth * diffVector.x, vertices[i].y - roadWidth * diffVector.y));
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
            instanceCellBuilding = Instantiate(cellBuilding);
            instanceCellBuilding.transform.parent = buildings.transform;
            instanceCellBuilding.name = "building " + i;
            instanceCellBuilding.GetComponent<Building>().SetVertices(cellBuildings[i]);

            // Debugging
            //List<Vector2f> vertices = instanceCellBuilding.GetComponent<Building>().GetVertices();
            //for (int j = 0; j < vertices.Count; j++)
            //{
            //    Debug.Log(vertices[j].x + ", " + vertices[j].y);
            //}

            buildingsList.Add(instanceCellBuilding);
        }
    }
    // Find the average x and y value from the cell to find the centroid
    private Vector2f CalculateCentroid()
    {
        float xSum = 0;
        float ySum = 0;

        for (int i = 0; i < vertices.Count; i++)
        {
            xSum += vertices[i].x;
            ySum += vertices[i].y;
        }

        return new Vector2f(xSum / vertices.Count, ySum / vertices.Count);
    }

    void OnDrawGizmos()
    {
        //Debug.Log("Draw Gizmos");
        Gizmos.color = Color.red;
        //Debug.Log(buildings.Count);
        for (int i = 0; i < buildingsList.Count; i++)
        {
            //Debug.Log("Outer Loop");
            List<Vector2f> vertices = buildingsList[i].GetComponent<Building>().GetVertices();
            for (int j = 0; j < vertices.Count; j++)
            {
                //Debug.Log("Inner loop");
                int k = (j + 1) % vertices.Count;
                Vector3 startVector = new Vector3(vertices[j].x, vertices[j].y, 0);
                Vector3 endVector = new Vector3(vertices[k].x, vertices[k].y, 0);
                Gizmos.DrawLine(startVector, endVector);
            }

        }
    }
}
