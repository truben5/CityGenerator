using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    //List<Vector2f> shapeToFill = new List<Vector2f>();
    private List<List<Vector2f>> buildings = new List<List<Vector2f>>();

    // Checks if any sides of plot are longer than max length. If so splits the polygon
    // until all sides are smaller or equal to max length
    public void MakeBuildings(List<Vector2f> plot, int maxLength)
    {
        buildings.Add(plot);

        for (int i=0; i < plot.Count; i++)
        {
            int j = i + 1;
            if (j > plot.Count - 1)
            {
                j = 0;
            }

            if (Distance(plot[i].x, plot[i].y, plot[j].x, plot[j].y) > maxLength)
            {
                buildings = splitPlot(buildings, maxLength, 0, i, j);
            }
        }
    }

    public List<List<Vector2f>> splitPlot(List<List<Vector2f>> buildings, int maxLength, 
        int plotInd, int lineStart, int lineEnd)
    {
        List<Vector2f> myPlot = buildings[plotInd];

        // NEEDED??????
        float invSlope = InvSlope(Slope(myPlot[lineStart].x, myPlot[lineStart].y, myPlot[lineEnd].x, 
            myPlot[lineEnd].y));

        Vector2f midPoint = Midpoint(myPlot[lineStart].x, myPlot[lineStart].y,
            myPlot[lineEnd].x, myPlot[lineEnd].y);

        List<Vector2f> intersectingSeg = LineIntersection(lineEnd, midPoint, myPlot);

    }


    //private float largestDist = 0;

    //// Finds largest distance between two vertices of cell and returns those vectors
    //public List<Vector2f> LargestLineSegment(List<Vector2f> cellVertices)
    //{
    //    List<Vector2f> largestSegment = new List<Vector2f>();

    //    Vector2f largestStart = new Vector2f();
    //    Vector2f largestEnd = new Vector2f();

    //    for (int i = 0; i < cellVertices.Count; i++)
    //    {
    //        int z = i + 1;
    //        if (z > cellVertices.Count - 1)
    //        {
    //            z = 0;
    //        }

    //        Vector2f start = cellVertices[i];
    //        Vector2f end = cellVertices[z];

    //        float d = Distance(start.x, start.y, end.x, end.y);

    //        if(d > largestDist)
    //        {
    //            largestStart = start;
    //            largestEnd = end;
    //            largestDist = d;
    //        }
    //    }

    //    largestSegment.Add(largestStart);
    //    largestSegment.Add(largestEnd);

    //    return largestSegment;

    //}

    //public Rect MakeLargestSquare(List<Vector2f> largestSegment)
    //{
    //    Rect square = new Rect();

    //    return square;
    //}

    // Calculates distance between two points
    public float Distance(float x1, float y1, float x2, float y2)
    {
        float x = Mathf.Abs(x2 - x1);
        float y = Mathf.Abs(y2 - y1);
        float dist = Mathf.Sqrt(x * x + y * y);
        return dist;
    }

    // Calculates midpoint between two points
    public Vector2f Midpoint(float x1, float y1, float x2, float y2)
    {
        Vector2f mid = new Vector2f((x1 + x2)/2.0, (y1 + y2)/2.0);
        return mid;
    } 

    // Calculates slope between two points
    public float Slope(float x1, float y1, float x2, float y2)
    {
        float slope = (y2 - y1)/(x2 - x1);
        return slope;
    }

    // Calculates inverse slope of line
    public float InvSlope(float slope)
    {
        float invSlope = -1 * (1 / slope);
        return invSlope;
    }

    // Finds line segment of plot that intersect bisector
    public List<Vector2f> LineIntersection(int startInd,Vector2f midPoint, List<Vector2f> plot)
    {
        List<Vector2f> plotIntersection = new List<Vector2f>();

        for (int i=startInd + 1; i < plot.Count + startInd; i++)
        {
            float m = Slope(midPoint.x, midPoint.y, plot[i].x, plot[i].y); 
            
            if (Mathf.Atan(m) > 90)
            {
                plotIntersection.Add(plot[i]);
                plotIntersection.Add(plot[i - 1]);
            }
        }
        return plotIntersection;
    }
}

