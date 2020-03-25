using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public List<StructureLine> vertices = new List<StructureLine>();

    public void CreateRoad(List<StructureLine> roadVertices)
    {
        vertices = roadVertices;
    }

    private void InstantiateRoad()
    {

    }
}
