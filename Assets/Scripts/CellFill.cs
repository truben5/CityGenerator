using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    private float largestDist = 0;

    // Finds largest distance between two vertices of cell and returns those vectors
    public List<Vector2f> LargestLineSegment(List<Vector2f> cellVertices)
    {
        List<Vector2f> largestSegment = new List<Vector2f>();

        Vector2f largestStart = new Vector2f();
        Vector2f largestEnd = new Vector2f();

        for (int i = 0; i < cellVertices.Count; i++)
        {
            int z = i + 1;
            if (z > cellVertices.Count - 1)
            {
                z = 0;
            }

            Vector2f start = cellVertices[i];
            Vector2f end = cellVertices[z];

            float d = Distance(start.x, start.y, end.x, end.y);
            
            if(d > largestDist)
            {
                largestStart = start;
                largestEnd = end;
                largestDist = d;
            }
        }

        largestSegment.Add(largestStart);
        largestSegment.Add(largestEnd);

        return largestSegment;

    }

    public Rect MakeLargestSquare(List<Vector2f> largestSegment)
    {
        Rect square = new Rect();

        return square;
    }

    // Calculates distance between two points
    public float Distance(float x1, float y1, float x2, float y2)
    {
        float x = Mathf.Abs(x2 - x1);
        float y = Mathf.Abs(y2 - y1);
        float dist = Mathf.Sqrt(x * x + y * y);
        return dist;
    }
}

