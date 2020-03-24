using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    private List<Vector3> buildingsPoints = new List<Vector3>();
    private List<StructureLine> buildingLines = new List<StructureLine>();

    // Checks if any sides of plot are longer than max length. If so splits the polygon
    // until all sides are smaller or equal to max length
    public List<List<Vector3>> MakeBuildings(List<List<Vector3>> cellPlot, int maxLength)
    {
        cellPlot = MakeBuildingShapes(cellPlot, maxLength, 0);

        for (int i=0; i < cellPlot.Count; i++)
        {
            for (int j = 0; j < cellPlot[i].Count; j++)
            {
                int k = (j + 1) % cellPlot[i].Count;
                StructureLine line = new StructureLine(cellPlot[i][j], cellPlot[i][k]);
                buildingLines.Add(line);
            }
        }
        return cellPlot;
    }

    public List<List<Vector3>> MakeBuildingShapes(List<List<Vector3>> cellPlot, int maxLength, int ptr)
    {
        for (int i = ptr; i < cellPlot.Count; i++)
            for (int j = 0; j < cellPlot[i].Count; j++)
            {
                int k = (j + 1) % cellPlot[i].Count;
                if (TooBig(cellPlot[i][j], cellPlot[i][k], maxLength))
                {
                    List<List<Vector3>> splitPlot = BisectCell(cellPlot[i], maxLength, 0, j, k);
                    cellPlot[i] = splitPlot[0];
                    cellPlot.Insert(i+1, splitPlot[1]);
                    //Recursive Call
                    cellPlot = MakeBuildingShapes(cellPlot, maxLength, i);
                }
            }
        return cellPlot;
    }

    // Splits cell into two shapes and returns the list of shapes
    private List<List<Vector3>> BisectCell(List<Vector3> buildingShape, int maxLength,
        int plotInd, int segStartInd, int segEndInd)
    {
        List<Vector3> myPlot = buildingShape;

        float slope = Slope(myPlot[segStartInd].x, myPlot[segStartInd].y, myPlot[segEndInd].x,
            myPlot[segEndInd].y);

        float invSlope = InvSlope(slope);

        Vector3 midPoint = Midpoint(myPlot[segStartInd].x, myPlot[segStartInd].y,
            myPlot[segEndInd].x, myPlot[segEndInd].y);

        Vector3 nextLinePoint;
        if (double.IsInfinity(invSlope))
        {
            nextLinePoint = new Vector3(midPoint.x, midPoint.y + 1);
        }
        else
        {
            nextLinePoint = new Vector3(midPoint.x + 1, midPoint.y + invSlope);
        }
        List<int> intersectingSeg = LineIntersection(segStartInd, midPoint, invSlope, myPlot);

        bool intersectIsVertex = false;
        Vector3 intersection;
        if (intersectingSeg.Count == 1)
        {
            intersection = myPlot[intersectingSeg[0]];
            intersectIsVertex = true;
        }
        else
        {
            intersection = Intersection(midPoint, nextLinePoint, myPlot[intersectingSeg[0]], 
            myPlot[intersectingSeg[1]]);
        }

        List<List<Vector3>> resultShapes = SplitPlotArray(buildingShape, midPoint, segStartInd, intersection, intersectingSeg[0], intersectIsVertex);

        return resultShapes;

    }

    // Splits array of vertices into two separate arrays representing individual buildings. Splits along perpendicular bisector
    public List<List<Vector3>> SplitPlotArray(List<Vector3> vertices, Vector3 bisector, int bisectorInd, Vector3 intersection, int intersectionInd, bool intersectIsVertex)
    {
        List<List<Vector3>> newBuildings = new List<List<Vector3>>();
        List<Vector3> plot1 = new List<Vector3>();
        List<Vector3> plot2 = new List<Vector3>();
        bool first = true;
        for (int i = 0; i < vertices.Count; i++)
        {
            if (first && i != bisectorInd && i != intersectionInd)
            {
                plot1.Add(vertices[i]);
            }
            else if (first && i == bisectorInd)
            {
                plot1.Add(vertices[i]);
                plot1.Add(bisector);
                plot1.Add(intersection);

                plot2.Add(bisector);
                first = false;
            }
            else if (first && i == intersectionInd && !intersectIsVertex)
            {
                plot1.Add(vertices[i]);
                plot1.Add(intersection);
                plot1.Add(bisector);
                first = false;
            }
            else if (first && i == intersectionInd && intersectIsVertex)
            {
                plot1.Add(intersection);
                plot1.Add(bisector);
                first = false;
            }
            else if (!first && i != bisectorInd && i != intersectionInd)
            {
                plot2.Add(vertices[i]);
            }
            else if (!first && i == bisectorInd)
            {
                plot2.Add(vertices[i]);
                plot2.Add(bisector);
                plot2.Add(intersection);
                first = true;
            }
            else if (!first && i == intersectionInd && !intersectIsVertex)
            {
                plot2.Add(vertices[i]);
                plot2.Add(intersection);
                first = true;
            }
            else if (!first && i == intersectionInd && intersectIsVertex)
            {
                plot2.Add(intersection);
            }
            else
            {
                throw new System.Exception("Unthought of case");
            }
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

    public bool TooBig(Vector3 start, Vector3 end, int maxLength)
    {
        float dist = Distance(start.x, start.y, end.x, end.y);

        if (dist > maxLength)
        {
            return true;
        }
        return false;
    }

    // Calculates midpoint between two points
    public Vector3 Midpoint(float x1, float y1, float x2, float y2)
    {
        Vector3 mid = new Vector3((x1 + x2)/2, (y1 + y2)/2, 0);
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

    // For the line segment the bisector intersects, it returns the indices of those vertices
    // Finds equation for perpendicular bisector. Plugs in x value of vertex in polygon to see if 
    // the point lies above or below the line.  When the orientation changes between above to below or 
    // vice versa, we know the intersection is between the 2 segments we were testing
    public List<int> LineIntersection(int startInd,Vector3 midPoint, float invSlope, List<Vector3> plot)
    {
        List<int> plotIntersection = new List<int>();

        //float segmentAngle = Mathf.Rad2Deg * Mathf.Atan(slope);
        int trend = -1;
        int location = -1;
        for (int i = startInd; i < plot.Count + startInd; i++)
        {

            int j = (i) % plot.Count;
            int l = (i + 1) % plot.Count;

            float b = midPoint.y - invSlope * midPoint.x;

            location = LocationToLine(invSlope, b, plot[l], midPoint.x);
            if (trend == 2)
            {
                plotIntersection.Add(j);
                return plotIntersection;
            }
            else if(trend != location && trend != -1)
            {
                if (location == 2)
                {
                    plotIntersection.Add(l);
                    return plotIntersection;
                }

                plotIntersection.Add(j);
                plotIntersection.Add(l);
                return plotIntersection;
            }
            trend = location;
        }
        return plotIntersection;
    }

    // Finds point of intersection two lines
    public Vector3 Intersection(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
    {
        // Line represented as a1x + b1y = c1
        float a1 = e1.y - s1.y;
        float b1 = s1.x - e1.x;
        float c1 = a1 * s1.x + b1 * s1.y;

        // Line represented as a2x + b2y = c2
        float a2 = e2.y - s2.y;
        float b2 = s2.x - e2.x;
        float c2 = a2 * s2.x + b2 * s2.y;

        float determinant = a1 * b2 - a2 * b1;

        if(determinant == 0)
        {
            throw new System.Exception("Determinant is 0, lines are parallel");
        }

        float x = (b2 * c1 - b1 * c2) / determinant;
        float y = (a1 * c2 - a2 * c1) / determinant;
        return new Vector3(x,y,0);
    }

    // Takes in the slope and y intercept for a line equation.  Determines if point is below, above, or on line 
    // If the slope is infinite (vertical line) uses the x values to determine if point is right or left of line
    public int LocationToLine(float m, float b, Vector3 p, float midPointX)
    {
        float resultY;
        if (double.IsInfinity(m))
        {
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

    //void OnDrawGizmos()
    //{
    //    //Debug.Log(buildingVertices);
    //    Gizmos.color = Color.red;
    //    for (int i = 0; i < buildingLines.Count; i++)
    //    {
    //        Gizmos.DrawLine(buildingLines[i].getStart(), buildingLines[i].getEnd());
    //    }
    //}
}

