using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFill : MonoBehaviour {

    private List<Vector3> buildingsPoints = new List<Vector3>();
    private List<Line> buildingLines = new List<Line>();

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
                Line line = new Line(cellPlot[i][j], cellPlot[i][k]);
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

        float slope = Geometry.Slope(myPlot[segStartInd], myPlot[segEndInd]);

        float invSlope = Geometry.InvSlope(slope);

        Vector3 midPoint = Geometry.Midpoint(myPlot[segStartInd], myPlot[segEndInd]);

        Vector3 nextLinePoint = Geometry.NextPointInLine(midPoint, invSlope);

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
            intersection = Geometry.Intersection(midPoint, nextLinePoint, myPlot[intersectingSeg[0]], 
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

    public bool TooBig(Vector3 start, Vector3 end, int maxLength)
    {
        float dist = Geometry.Distance(start, end);

        if (dist > maxLength)
        {
            return true;
        }
        return false;
    }

    // For the line segment the bisector intersects, it returns the indices of those vertices
    // Finds equation for perpendicular bisector. Plugs in x value of vertex in polygon to see if 
    // the point lies above or below the line.  When the orientation changes between above to below or 
    // vice versa, we know the intersection is between the 2 segments we were testing
    public List<int> LineIntersection(int startInd, Vector3 midPoint, float invSlope, List<Vector3> plot)
    {
        List<int> plotIntersection = new List<int>();

        //float segmentAngle = Mathf.Rad2Deg * Mathf.Atan(slope);
        int trend = -1;
        int location = -1;
        for (int i = startInd; i < plot.Count + startInd; i++)
        {

            int j = (i) % plot.Count;
            int l = (i + 1) % plot.Count;

            float b = Geometry.YIntercept(midPoint, invSlope);

            location = Geometry.RelationToLine(invSlope, b, plot[l], midPoint.x);
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

