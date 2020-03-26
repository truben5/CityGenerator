using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Structure
{
    private Vector3 centroid;
    private List<Vector3> vertices;

    public Structure(List<Vector3> structureVertices)
    {
        vertices = structureVertices;
    }

    public GetVertices()
    {
        return vertices;
    }

    public void PullInStructureVertex()
    {

    }

    public void PushOutStructureVertex()
    {

    }

    //protected abstract Vector3 ExtractVertices();


    private Vector3 CalculateDistanceVector(Vector3 vertex)
    {
        Vector3 diffVector = new Vector3();

        diffVector.x = vertex.x - centroid.x;
        diffVector.y = vertex.y - centroid.y;
        diffVector.Normalize();

        return diffVector;
    }

    private void CalculateCentroid()
    {
        float xSum = 0;
        float ySum = 0;

        for (int i=0; i < vertices.Count; i++)
        {
            xSum += vertices[i].x;
            ySum += vertices[i].y;
        }

        centroid.x = xSum / vertices.Count;
        centroid.y = ySum / vertices.Count;
    }
}
