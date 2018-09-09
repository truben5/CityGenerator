using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiCell : MonoBehaviour {

    private List<Vector2f> vertices = new List<Vector2f>();
    private List<GameObject> buildings = new List<GameObject>();

    public GameObject cellBuilding;

    public List<Vector2f> GetCellVertices()
    {
        return this.vertices;
    }

    // Sets vertices of voronoi cell
    public void SetCellVertices(List<Vector2f> regionVertices)
    {
        for (int i=0; i < regionVertices.Count; i++)
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
        for (int i = 0; i < cellBuildings[i].Count; i++)
        {
            cellBuilding = Instantiate(cellBuilding);
            cellBuilding.transform.parent = this.transform;
            cellBuilding.name = "building " + i;
            Debug.Log(cellBuildings[i]);
            Debug.Log(cellBuilding.GetComponent<Building>());
            Debug.Log(cellBuilding);
            //cellBuilding.GetComponent<Building>().SetVertices(cellBuildings[i]);

        }
    }
	
}
