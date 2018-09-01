﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    //List<Vector2f> shapeToFill = new List<Vector2f>();
    private List<List<Vector2f>> buildings = new List<List<Vector2f>>();
    private List<StructureLine> buildingLines = new List<StructureLine>();
    // to debug
    //private List<StructureLine> buildingLines = new List<StructureLine>();

    // Checks if any sides of plot are longer than max length. If so splits the polygon
    // until all sides are smaller or equal to max length
    public void MakeBuildings(List<List<Vector2f>> cellPlot, int maxLength)
    {
        Debug.Log("Starting MakeBuildings");
        cellPlot = MakeBuildingShapes(cellPlot, maxLength);
        for (int i=0; i < cellPlot.Count; i++)
        {
            for (int j = 0; j < cellPlot[i].Count; j++)
            {
                int k = j + 1;
                if (k > cellPlot.Count - 1)
                {
                    k = 0;
                }
                StructureLine line = new StructureLine(cellPlot[i][j], cellPlot[i][k]);
                buildingLines.Add(line);
            }
        }
        
    }

    public List<List<Vector2f>> MakeBuildingShapes(List<List<Vector2f>> cellPlot, int maxLength)
    {
        for (int i = 0; i < cellPlot.Count; i++)
            for (int j = 0; j < cellPlot[i].Count; j++)
            {
                int k = j + 1;
                if (k > cellPlot[i].Count - 1)
                {
                    k = 0;
                }
                if (TooBig(cellPlot[i][j], cellPlot[i][k], maxLength))
                {
                    cellPlot = BisectCell(cellPlot[i], maxLength, 0, j, k);

                    //Recursive Call
                    cellPlot = MakeBuildingShapes(cellPlot, maxLength);
                 
                }
            }
        return cellPlot;
    }

    private List<List<Vector2f>> BisectCell(List<Vector2f> buildingShapes, int maxLength,
        int plotInd, int segStartInd, int segEndInd)
    {
        List<Vector2f> myPlot = buildingShapes;

        float slope = Slope(myPlot[segStartInd].x, myPlot[segStartInd].y, myPlot[segEndInd].x,
            myPlot[segEndInd].y);

        float invSlope = InvSlope(slope);

        Vector2f midPoint = Midpoint(myPlot[segStartInd].x, myPlot[segStartInd].y,
            myPlot[segEndInd].x, myPlot[segEndInd].y);

        Vector2f nextLinePoint;
        if (double.IsInfinity(invSlope))
        {
            //Debug.Log("infinity is invslope");
            nextLinePoint = new Vector2f(midPoint.x, midPoint.y + 1);
        }
        else
        {
            //Debug.Log("Next point of bisector is: midPoint.x + 1, midPoint.y + " + invSlope);
            nextLinePoint = new Vector2f(midPoint.x + 1, midPoint.y + invSlope);
        }
        List<int> intersectingSeg = LineIntersection(segEndInd, midPoint, invSlope, myPlot);
        //Debug.Log("Next line point:" + nextLinePoint.x + " " + nextLinePoint.y);
        //Debug.Log("line segment intersected:" + intersectingSeg[0] + " " +  intersectingSeg[1]);
        //StructureLine wall;
        bool intersectIsVertex = false;
        Vector2f intersection;
        if (intersectingSeg.Count == 1)
        {
            //wall = new StructureLine(midPoint, myPlot[intersectingSeg[0]]);
            intersection = myPlot[intersectingSeg[0]];
            intersectIsVertex = true;
        }
        else
        {
            intersection = Intersection(midPoint, nextLinePoint, myPlot[intersectingSeg[0]], 
            myPlot[intersectingSeg[1]]);

            //wall = new StructureLine(midPoint, intersection);
        }

        List<List<Vector2f>> resultShapes = SplitPlotArray(buildingShapes, midPoint, segStartInd, intersection, intersectingSeg[0], intersectIsVertex);

        //buildings.Add(wall);
        //buildingLines.Add(wall);

        return resultShapes;

    }

    // Splits array of vertices into two separate arrays representing individual buildings. Splits along perpendicular bisector
    public List<List<Vector2f>> SplitPlotArray(List<Vector2f> vertices, Vector2f bisector, int bisectorInd, Vector2f intersection, int intersectionInd, bool intersectIsVertex)
    {
        List<List<Vector2f>> newBuildings = new List<List<Vector2f>>();
        List<Vector2f> plot1 = new List<Vector2f>();
        List<Vector2f> plot2 = new List<Vector2f>();
        bool first = true;
        for (int i = 0; i < vertices.Count; i++)
        {
            //Debug.Log("vertex list is: " + vertices[i]);
            if (i == intersectionInd && intersectIsVertex)
            {
                plot2.Add(vertices[i]);
                plot2.Add(bisector);
                plot1.Add(vertices[i]);
            }

            else if (i == intersectionInd && !intersectIsVertex)
            {
                plot2.Add(vertices[i]);
                plot2.Add(intersection);
                plot2.Add(bisector);
                plot1.Add(intersection);
                first = true;
            }

            else if (i == bisectorInd)
            {
                plot1.Add(vertices[i]);
                plot1.Add(bisector);
                first = false;
            }

            else if (first && i != bisectorInd)
            {
                plot1.Add(vertices[i]);
            }
            else if (!first && i != bisectorInd && i != intersectionInd )
            {
                plot2.Add(vertices[i]);
            }

        }
        //for debugging
        for (int i = 0; i < plot1.Count; i++)
            {
                Debug.Log("plot1 ind " + i + " is: " + plot1[i]);
            }
        for (int j = 0; j < plot2.Count; j++)
        {
            Debug.Log("Plot2 ind " + j + " is " + plot2[j]);
        }
        newBuildings.Add(plot1);
        newBuildings.Add(plot2);
        return newBuildings;
    }

    // Calculates distance between two points
    public float Distance(float x1, float y1, float x2, float y2)
    {
        float x = Mathf.Abs(x2 - x1);
        float y = Mathf.Abs(y2 - y1);
        float dist = Mathf.Sqrt(x * x + y * y);
        return dist;
    }

    public bool TooBig(Vector2f start, Vector2f end, int maxLength)
    {
        float dist = Distance(start.x, start.y, end.x, end.y);
        Debug.Log("Distance is: " + dist);
        if (dist > maxLength)
        {
            return true;
        }
        return false;
    }

    // Calculates midpoint between two points
    public Vector2f Midpoint(float x1, float y1, float x2, float y2)
    {
        Vector2f mid = new Vector2f((x1 + x2)/2.0, (y1 + y2)/2.0);
        Debug.Log("Midpoint :" + mid.x + ", " + mid.y);
        return mid;
    } 

    // Calculates slope between two points
    public float Slope(float x1, float y1, float x2, float y2)
    {
        float slope = (y2 - y1)/(x2 - x1);
        //Debug.Log("slope: " + slope);
        return slope;
    }

    // Calculates inverse slope of line
    public float InvSlope(float slope)
    {
        float invSlope = -1 * (1 / slope);
        //Debug.Log("inv slope: " + invSlope);
        return invSlope;
    }

    // For the line segment the bisector intersects, it returns the indices of those vertices
    public List<int> LineIntersection(int startInd,Vector2f midPoint, float invSlope, List<Vector2f> plot)
    {
        List<int> plotIntersection = new List<int>();

        //float segmentAngle = Mathf.Rad2Deg * Mathf.Atan(slope);
        //Debug.Log("In LineIntersection invSlope is: " + invSlope);
        int trend = -1;
        int location = -1;
        for (int i = startInd; i < plot.Count + startInd; i++)
        {
            int l = (i + 1) % plot.Count;
            //Debug.Log("l = " + l);
            int j = (i) % plot.Count;
            //Debug.Log("Checking between points: " + plot[l] + " and " + plot[j]);

            float b = midPoint.y - invSlope * midPoint.x;
            location = locationToLine(invSlope, b, plot[l], midPoint.x);
            //Debug.Log("trend is: " + trend + " and location is: " + location);
            if (trend == -1)
            {
                trend = location;
            }
            else if(trend != location)
            {
                plotIntersection.Add(j);
                plotIntersection.Add(l);
                return plotIntersection;
            }
            else if(trend == 2)
            {
                plotIntersection.Add(j);
                return plotIntersection;
            }
            
            //Debug.Log("Trend and location are: " + trend + " and " + location);
        }
        return plotIntersection;
    }

    // Finds point of intersection two lines
    public Vector2f Intersection(Vector2f s1, Vector2f e1, Vector2f s2, Vector2f e2)
    {
        //Debug.Log("intersection of: " + s1 + " and " + e1 + " with " + 
          //  s2 + " and " + e2);

        // Line represented as a1x + b1y = c1
        float a1 = e1.y - s1.y;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.y;

        // Line represented as a2x + b2y = c2
        float a2 = e2.y - s2.y;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.y;

        //Debug.Log("a1: " + a1 + " b1: " + b1 + " c1: " + c1 + " a2: " + a2 + " b2: " + b2 + " c2: " + c2);

        float determinant = a1 * b2 - a2 * b1;

        if(determinant == 0)
        {
            throw new System.Exception("Determinant is 0, lines are parallel");
        }

        double x = (b2 * c1 - b1 * c2) / determinant;
        double y = (a1 * c2 - a2 * c1) / determinant;
        Debug.Log("Intersection point is: " + x + ", " + y);
        return new Vector2f(x,y);
    }

    // Takes in the slope and y intercept for a line equation.  Determines if point is below, above, or on line 
    // If the slope is infinite (vertical line) uses the x values to determine if point is right or left of line
    public int locationToLine(float m, float b, Vector2f p, float midPointX)
    {
        //Debug.Log("Equation of line is: y = " + m + "x + " + b);
        float resultY;
        if (double.IsInfinity(m))
        {
          //  Debug.Log("b is infinity");
            if (midPointX < p.x)
            {
                return 0;
            }
            else if (midPointX > p.x)
            {
                return 1;
            }
            else if (midPointX == p.x)
            {
                return 2;
            }
            else
            {
                throw new System.Exception("Unresolved case");
            }
        }
        else
        {
            resultY = m * p.x + b;
        }

        if (resultY < p.y)
        {
            return 0;
        }
        else if (resultY > p.y)
        {
            return 1;
        }
        else if (resultY == p.y)
        {
            return 2;
        }
        else{
            throw new System.Exception("unthought of case");
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

