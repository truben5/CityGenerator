﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ResizablePolygon : MonoBehaviour
{
    protected Vector3 centroid;
    protected List<Vector3> vertices;
    protected float perimeter;

    public List<Vector3> GetVertices()
    {
        return vertices;
    }

    public void SetVertices(List<Vector3> polygonVertices)
    {
        vertices = polygonVertices;
        CalculateProperties();
    }

    public Vector3 GetCentroid()
    {
        return centroid;
    }

    public float GetPerimeter()
    {
        return perimeter;
    }

    // Calculates position of the vertex that has been pulled in closer to the center of the polygon
    protected Vector3 PullInPolygonVertex(Vector3 vertex, float modifier)
    {
        Vector3 diffVector = CalculateCentroidDistanceVector(vertex);
        Vector3 newVertex = new Vector3(vertex.x - modifier * diffVector.x, vertex.y - modifier * diffVector.y, 0);

        return newVertex;
    }

    // Calculates the position of the vertex that has been pushed away from the center of the polygon
    protected Vector3 PushOutPolygonVertex(Vector3 vertex, float modifier)
    {
        Vector3 diffVector = CalculateCentroidDistanceVector(vertex);
        Vector3 newVertex = new Vector3(vertex.x + modifier * diffVector.x, vertex.y + modifier * diffVector.y, 0);
        return newVertex;
    }

    // Calculates difference vector between a vertex and the polygon centroid
    protected Vector3 CalculateCentroidDistanceVector(Vector3 vertex)
    {
        Vector3 diffVector = new Vector3();

        diffVector.x = vertex.x - centroid.x;
        diffVector.y = vertex.y - centroid.y;
        diffVector.Normalize();

        return diffVector;
    }

    // Calculate the centroid and perimeter of polygon
    protected void CalculateProperties()
    {
        float xSum = 0;
        float ySum = 0;

        for (int i=0; i < vertices.Count; i++)
        {
            xSum += vertices[i].x;
            ySum += vertices[i].y;

            perimeter +=Geometry.Distance(vertices[i], vertices[(i+1)%vertices.Count]);
        }

        centroid.x = xSum / vertices.Count;
        centroid.y = ySum / vertices.Count; 
    }
}
