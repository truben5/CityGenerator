using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();
    private List<GameObject> buildings = new List<GameObject>();

    public GameObject cellBuilding;
    private GameObject instanceCellBuilding;

    public List<Vector2f> GetCellVertices()
    {
        return this.vertices;
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
        return buildings;
    }

    // Takes in array of points and creates the building objects within a cell
    public void SetBuildings(List<List<Vector2f>> cellBuildings)
    {
        for (int i = 0; i < cellBuildings.Count; i++)
        {
            instanceCellBuilding = Instantiate(cellBuilding);
            instanceCellBuilding.transform.parent = this.transform;
            instanceCellBuilding.name = "building " + i;
            instanceCellBuilding.GetComponent<Building>().SetVertices(cellBuildings[i]);
            // Debugging
            //List<Vector2f> vertices = instanceCellBuilding.GetComponent<Building>().GetVertices();
            //for (int j = 0; j < vertices.Count; j++)
            //{
            //    Debug.Log(vertices[j].x + ", " + vertices[j].y);
            //}
            buildings.Add(instanceCellBuilding);
        }
    }

    //void OnDrawGizmos(){
    //    //Debug.Log("Draw Gizmos");
    //    Gizmos.color = Color.red;
    //    //Debug.Log(buildings.Count);
    //    for (int i = 0; i < buildings.Count; i++)
    //    {
    //        //Debug.Log("Outer Loop");
    //        List<Vector2f> vertices = buildings[i].GetComponent<Building>().GetVertices();
    //        for (int j = 0; j < vertices.Count; j++)
    //        {
    //            //Debug.Log("Inner loop");
    //            int k = (j + 1) % vertices.Count;
    //            Vector3 startVector = new Vector3(vertices[j].x, vertices[j].y, 0);
    //            Vector3 endVector = new Vector3(vertices[k].x, vertices[k].y, 0);
    //            Gizmos.DrawLine(startVector, endVector);
    //        }
            
    //    }
    //}

}
