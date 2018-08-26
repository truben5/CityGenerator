using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    //List<Vector2f> shapeToFill = new List<Vector2f>();
    private List<List<Vector2f>> buildings = new List<List<Vector2f>>();
    // to debug
    private List<StructureLine> buildingLines = new List<StructureLine>();

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
                Debug.Log("Line Segment between " + i + " and " + j + " is greater than: " + maxLength);
                buildings = splitPlot(buildings, maxLength, 0, i, j);
            }
        }
    }

    private List<List<Vector2f>> splitPlot(List<List<Vector2f>> buildings, int maxLength,
        int plotInd, int segStartInd, int segEndInd)
    {
        List<Vector2f> myPlot = buildings[plotInd];

        float slope = Slope(myPlot[segStartInd].x, myPlot[segStartInd].y, myPlot[segEndInd].x,
            myPlot[segEndInd].y);

        float invSlope = InvSlope(slope);

        Vector2f midPoint = Midpoint(myPlot[segStartInd].x, myPlot[segStartInd].y,
            myPlot[segEndInd].x, myPlot[segEndInd].y);

        List<int> intersectingSegInd = LineIntersection(segEndInd, midPoint, slope, myPlot);
        Vector2f nextLinePoint;
        if (double.IsInfinity(invSlope))
        {
            Debug.Log("infinity is invslope");
            nextLinePoint = new Vector2f(midPoint.x, midPoint.y + 1);
        }
        else
        {
            nextLinePoint = new Vector2f(midPoint.x + 1, midPoint.y + invSlope);
        }
        Debug.Log("Next line point:" + nextLinePoint.x + " " + nextLinePoint.y);
        Debug.Log("ind of line segment intersected:" + intersectingSegInd[0] + " " +  intersectingSegInd[1]);
        Vector2f intersection = Intersection(midPoint, nextLinePoint, myPlot[intersectingSegInd[0]], 
            myPlot[intersectingSegInd[1]]);

        StructureLine wall = new StructureLine(midPoint, intersection);
        buildingLines.Add(wall);

        return buildings;

    }


    //private float largestDist = 0;

    //// Finds largest distance between two vertices of cell and returns those vectors
    //public List<Vector2f> LargestLineSegment(List<Vector2f> cellVertices)
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

    // Calculates distance between two points
    private float Distance(float x1, float y1, float x2, float y2)
    {
        float x = Mathf.Abs(x2 - x1);
        float y = Mathf.Abs(y2 - y1);
        float dist = Mathf.Sqrt(x * x + y * y);
        return dist;
    }

    // Calculates midpoint between two points
    private Vector2f Midpoint(float x1, float y1, float x2, float y2)
    {
        Vector2f mid = new Vector2f((x1 + x2)/2.0, (y1 + y2)/2.0);
        Debug.Log("Midpoint :" + mid.x + ", " + mid.y);
        return mid;
    } 

    // Calculates slope between two points
    private float Slope(float x1, float y1, float x2, float y2)
    {
        float slope = (y2 - y1)/(x2 - x1);
        Debug.Log("slope: " + slope);
        return slope;
    }

    // Calculates inverse slope of line
    private float InvSlope(float slope)
    {
        float invSlope = -1 * (1 / slope);
        Debug.Log("inv slope: " + invSlope);
        return invSlope;
    }

    // For the line segment the bisector intersects, it returns the indices of those vertices
    private List<int> LineIntersection(int startInd,Vector2f midPoint, float slope, List<Vector2f> plot)
    {
        List<int> plotIntersectionInd = new List<int>();
        int j = startInd + 1;
        for (int i=startInd; i < plot.Count; i++)
        {
            if (j > plot.Count - 1)
            {
                j = j - plot.Count;
            }
            float segmentAngle = Mathf.Rad2Deg * Mathf.Atan(slope);

            float newLineSlope = Slope(midPoint.x, midPoint.y, plot[i].x, plot[i].y);
            float newLineDegrees = Mathf.Rad2Deg * Mathf.Atan(newLineSlope);
            Debug.Log("180 - " + newLineDegrees + " - " + segmentAngle);
            float degrees = 180 - Mathf.Abs(newLineDegrees - segmentAngle);
            Debug.Log("Degrees of line to vertex are: " + degrees);
            if (degrees > 90 && degrees < 180)
            {
                Debug.Log("Sweet spot degrees");
                plotIntersectionInd.Add(i);
                if (i == 0)
                {
                    i = plot.Count - 1;
                    plotIntersectionInd.Add(i);

                }
                else
                {
                    plotIntersectionInd.Add(i - 1);
                }
                return plotIntersectionInd;
            }
            j++;
        }

        if (plotIntersectionInd.Count != 2)
        {
            throw new System.Exception("No intersecting line for bisector");
        }
        return plotIntersectionInd;
    }

    // Finds point of intersection between a line and line segment
    private Vector2f Intersection(Vector2f s1, Vector2f e1, Vector2f s2, Vector2f e2)
    {
        Debug.Log("intersection of: " + s1.x + ", " + s1.y + " and " + e1.x + ", " + e2.y + " with " + 
            s2.x + ", " + s2.y + " and " + e2.x + ", " + e2.y);

        // Line represented as a1x + b1y = c1
        float a1 = e1.y - s1.y;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.y;

        // Line represented as a2x + b2y = c2
        float a2 = e2.y - s2.y;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.y;

        Debug.Log("a1: " + a1 + " b1: " + b1 + " c1: " + c1 + " a2: " + a2 + " b2: " + b2 + " c2: " + c2);

        float determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
        {
            throw new System.Exception("Lines to make buildings are parallel");
        }
        else
        {
            double x = (b2 * c1 - b1 * c2) / determinant;
            double y = (a1 * c2 - a2 * c1) / determinant;
            Debug.Log("Intersection point is: " + x + ", " + y);
            return new Vector2f(x,y);
        }
    }

    void OnDrawGizmos()
    {
        //Debug.Log(buildingVertices);
        Gizmos.color = Color.red;
        for (int i = 0; i < buildingLines.Count; i++)
        {
            Gizmos.DrawLine(buildingLines[i].getStart(), buildingLines[i].getEnd());

        }
    }
}

