﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFillTest : MonoBehaviour {

    public GameObject voronoiCell;
    private List<Vector3> plot1 = new List<Vector3>();
    private List<Vector3> plot2 = new List<Vector3>();
    private Vector3 midPoint;
    private Vector3 intersection;
    private List<List<Vector3>> newShapes = new List<List<Vector3>>();

    // Use this for initialization
    void Start()
    {
        voronoiCell = Instantiate(voronoiCell);
        Debug.Log("Beginning Tests...");
        Debug.Log("Testing Intersection Function");
        IntersectionTests();
        Debug.Log("Testing LineIntersection Function");
        LineIntersectionTests();
        Debug.Log("Testing SpltPlotArray Function");
        SplitPlotArrayTest();
        Debug.Log("Testing Make Building Shapes");
        MakeBuidlingShapes();
    }

    public void IntersectionTests()
    {
        Debug.Log("Test 1");
        Vector3 s1 = new Vector3(10, 20);
        Vector3 e1 = new Vector3(10, 30);
        Vector3 s2 = new Vector3(30, 40);
        Vector3 e2 = new Vector3(20, 20);
        Vector3 intersection = Geometry.Intersection(s1, e1, s2, e2);
        if (intersection.x != 10 && intersection.y != 0)
        {
            throw new System.Exception("Expected (10,0) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 2");

        s1 = new Vector3(10, 10);
        e1 = new Vector3(20, 10);
        s2 = new Vector3(20, 30);
        e2 = new Vector3(30, 20);
        intersection = Geometry.Intersection(s1, e1, s2, e2);
        if (intersection.x != 40 && intersection.y != 10)
        {
            throw new System.Exception("Expected (40,10) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 3");
        s2 = new Vector3(30, 30);
        e2 = new Vector3(30, 20);
        intersection = Geometry.Intersection(s1, e1, s2, e2);
        if (intersection.x != 30 || intersection.y != 10)
        {
            throw new System.Exception("Expected (10,30) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 4");

        s1 = new Vector3(10, 10);
        e1 = new Vector3(10, 40);
        try
        {
            intersection = Geometry.Intersection(s1, e1, s2, e2);
            Debug.Log("ERROR: Intersection should have thrown an exception for parallel lines");
        }
        catch
        {
            Debug.Log("All Intersecion tests pass");
        }
    }

    public void LineIntersectionTests()
    {
        plot1 = new List<Vector3>();
        plot1.Add(new Vector3(10, 40));
        plot1.Add(new Vector3(10,30));
        plot1.Add(new Vector3(20, 20));
        plot1.Add(new Vector3(30,40));

        Debug.Log("Test 1");
        // Slope between (10,40) and (10,30)
        float slope = Geometry.Slope(plot1[0], plot1[1]);
        float invSlope = Geometry.InvSlope(slope);
        midPoint = Geometry.Midpoint(plot1[0], plot1[1]);
        if (midPoint.x != 10 || midPoint.y != 35)
        {
            throw new System.Exception("Incorrect Midpoint");
        }
        // Line segment bisector intersects with
        List<int> lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(1, midPoint, invSlope, plot1);
        if (lineSeg[0] != 2 || lineSeg[1] != 3)
        {
            throw new System.Exception("Extpected intersection between (20,20) and (30,40) but received " + lineSeg[0] + " and " + lineSeg[1]);
        }

        Vector3 nextPoint = new Vector3(midPoint.x + 1, midPoint.y + invSlope);
        // Point of intersection between bisector and line segment
        intersection = Geometry.Intersection(midPoint, nextPoint, plot1[lineSeg[0]], plot1[lineSeg[1]]);
        if (intersection.x != 27.5 || intersection.y != 35)
        {
            throw new System.Exception("Extpected intersection to be (27.5,35) but received " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 2");
        lineSeg = null;
        // Between (10,30) and (20,20)
        slope = Geometry.Slope(plot1[1], plot1[2]);
        invSlope = Geometry.InvSlope(slope);
        midPoint = Geometry.Midpoint(plot1[1], plot1[2]);

        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(2, midPoint, invSlope, plot1);
        if (lineSeg.Count > 1 || lineSeg[0] != 3)
        {
            throw new System.Exception("Extpected intersection between (20,20) and (30,40) but received " + lineSeg[0] + " and " + lineSeg[1]);
        }
        //nextPoint = new Vector3(midPoint.x + 1, midPoint.y + invSlope);
        //intersection = voronoiCell.GetComponent<CellFill>().Intersection(midPoint, nextPoint, plot1[lineSeg[0]], plot1[lineSeg[1]]);
        //if (intersection.x != 30 || intersection.y != 40)
        //{
        //    throw new System.Exception("Extpected intersection to be (30,40) but received " + intersection.x + ", " + intersection.y);
        //}


        Debug.Log("Test 3");
        // Slope between (20,20) and (30,40)
        slope = Geometry.Slope(plot1[2], plot1[3]);
        //Debug.Log(slope);
        invSlope = Geometry.InvSlope(slope);
        midPoint = Geometry.Midpoint(plot1[2], plot1[3]);
        //Debug.Log(invSlope);
        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(3, midPoint, invSlope, plot1);
        if (lineSeg[0] != 0 || lineSeg[1] != 1)
        {
            throw new System.Exception("Extpected (10,40) and (10,30) but received " + lineSeg[0] + " and " + lineSeg[1]);
        }

        Debug.Log("Test 4");
        lineSeg = null;
        // Between (30,40) and (10,40)
        slope = Geometry.Slope(plot1[3], plot1[0]);
        invSlope = Geometry.InvSlope(slope);
        midPoint = Geometry.Midpoint(plot1[3], plot1[0]);
        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(0, midPoint, invSlope, plot1);
        if (lineSeg.Count > 1 || lineSeg[0] != 2)
        {
            throw new System.Exception("Extpected (20,20) and (30,40) but received " + lineSeg[0] + " and " + lineSeg[1]);
        }

        Debug.Log("Line intersection tests passed");

    }

    public void SplitPlotArrayTest()
    {
        Debug.Log("Test 1");
        /*
         * Testing split plot array for when bisector is hit first and intersection is not an existing vertex
         */
        midPoint = Geometry.Midpoint(plot1[0], plot1[1]);
        //Debug.Log("midpoint is: " + midPoint + " and intersection is " + intersection);
        List<List<Vector3>> newShapes = voronoiCell.GetComponent<CellFill>().SplitPlotArray(plot1, midPoint, 0, intersection, 2, false);

        if (newShapes[0].Count != 4 || newShapes[1].Count != 4)
        {
            throw new System.Exception("Expected plot 1 and plot 2 to have 4 vertices each but 1: " + newShapes[0].Count + " and 2: " + newShapes[1].Count);
        }
        if (newShapes[0][0] != plot1[0] || newShapes[0][1] != midPoint || newShapes[0][2] != intersection || newShapes[0][3] != plot1[3])
        {
            throw new System.Exception("Error in vertices for new plot 1");
        }
        if (newShapes[1][0] != midPoint || newShapes[1][1] != plot1[1] || newShapes[1][2] != plot1[2] || newShapes[1][3] != intersection)
        {
            throw new System.Exception("Error in vertices for new plot 2");
        }

        Debug.Log("Test 2");
        /*
        * Testing split plot array for when intersection is hit first and intersection is not an existing vertex
         */
        plot2.Add(new Vector3(0, 10));
        plot2.Add(new Vector3(0, 0));
        plot2.Add(new Vector3(10, 0));
        plot2.Add(new Vector3(10, 10));
        midPoint = new Vector3(10,5);
        intersection = new Vector3(0, 5);
        newShapes = voronoiCell.GetComponent<CellFill>().SplitPlotArray(plot2, midPoint, 2, intersection, 0, false);

        if (newShapes[0].Count!= 4 || newShapes[1].Count != 4) 
        {
            throw new System.Exception("Expected plot 1 and plot 2 to have 3 vertices each but 1: " + newShapes[0].Count + " and 2: " + newShapes[1].Count);
        }
        if (newShapes[0][0] != plot2[0] || newShapes[0][1] != intersection || newShapes[0][2] != midPoint || newShapes[0][3] != plot2[3])
        {
            throw new System.Exception("Error in vertices for new plot 1");
        }
        if (newShapes[1][0] != plot2[1] || newShapes[1][1] != plot2[2] || newShapes[1][2] != midPoint || newShapes[1][3] != intersection)
        {
            throw new System.Exception("Error in vertices for new plot 2");
        }

        plot2.Clear();
        Debug.Log("Test 3");
        /*
         * Testing for when bisector is hit first before intersection and intersection is an existing vertex
         */
        plot2.Add(new Vector3(10, 30));
        plot2.Add(new Vector3(10, 10));
        plot2.Add(new Vector3(20, 20));
        midPoint = new Vector3(10, 20);
        intersection = new Vector3(20, 20);

        newShapes = voronoiCell.GetComponent<CellFill>().SplitPlotArray(plot2, midPoint, 0, intersection, 2, true);

        if (newShapes[0].Count != 3 || newShapes[1].Count != 3)
        {
            throw new System.Exception("Expected plot 1 and plot 2 to have 3 vertices each but 1: " + newShapes[0].Count + " and 2: " + newShapes[1].Count);
        }

        if (newShapes[0][0] != plot2[0] || newShapes[0][1] != midPoint || newShapes[0][2] != plot2[2])
        {
            throw new System.Exception("Error in vertices for new plot 1");
        }
        if (newShapes[1][0] != midPoint || newShapes[1][1] != plot2[1] || newShapes[1][2] != intersection)
        {
            throw new System.Exception("Error in vertices for new plot 2");
        }

        Debug.Log("Test 4");
        /*
         * Testing for when intersection is hit before bisector and intersection in an existing vertex
         */
        plot2.Clear();
        plot2.Add(new Vector3(10, 20));
        plot2.Add(new Vector3(20, 10));
        plot2.Add(new Vector3(20, 30));
        midPoint = new Vector3(20, 20);
        intersection = new Vector3(10, 20);

        newShapes = voronoiCell.GetComponent<CellFill>().SplitPlotArray(plot2, midPoint, 1, intersection, 0, true);

        if (newShapes[0].Count != 3 || newShapes[1].Count != 3)
        {
            for (int i = 0; i < newShapes[0].Count; i++)
                Debug.Log("plot 1 " + newShapes[0][i]);
            for (int i = 0; i < newShapes[1].Count; i++)
            {
                Debug.Log("plot 2 " + newShapes[1][i]);
            }
            throw new System.Exception("Expected plot 1 and plot 2 to have 3 vertices each but 1: " + newShapes[0].Count + " and 2: " + newShapes[1].Count);
        }

        if (newShapes[0][0] != plot2[0] || newShapes[0][1] != midPoint || newShapes[0][2] != plot2[2])
        {
            throw new System.Exception("Error in vertices for new plot1");
        }

        if (newShapes[1][0] != plot2[1] || newShapes[1][1] != midPoint || newShapes[1][2] != plot2[0])
        {
            throw new System.Exception("Error in vertices for new plot2");
        }

        Debug.Log("Split Plot Array Tests passed");
    }



    public void MakeBuidlingShapes()
    {
        newShapes.Add(plot1);

        newShapes = voronoiCell.GetComponent<CellFill>().MakeBuildingShapes(newShapes, 16, 0);
        
        //for (int i = 0; i < newShapes.Count;i++)
        //{
        //    Debug.Log("Shape " + i);
        //    for (int j = 0; j < newShapes[i].Count; j++)
        //    {
        //        Debug.Log("Vertex " + j);
        //        Debug.Log(newShapes[i][j]);
        //    }
        //}
        voronoiCell.GetComponent<VoronoiCell>().SetBuildings(newShapes);
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    for (int i = 0; i < plot1.Count; i++)
    //    {
    //        int j = (i + 1) % plot1.Count;
    //        Vector3 startVector = new Vector3(plot1[i].x, plot1[i].y, 0);
    //        Vector3 endVector = new Vector3(plot1[j].x, plot1[j].y, 0);
    //        Gizmos.DrawLine(startVector, endVector);
    //    }

    //    Gizmos.color = Color.green;
    //    for (int i = 0; i < newShapes.Count; i++)
    //    {
    //        for (int j = 0; j < newShapes[i].Count; j++)
    //        {
    //            int k = (j + 1) % newShapes[i].Count;
    //            Vector3 startVector = new Vector3(newShapes[i][j].x, newShapes[i][j].y, 0);
    //            Vector3 endVector = new Vector3(newShapes[i][k].x, newShapes[i][k].y, 0);
    //            Gizmos.DrawLine(startVector, endVector);
    //        }
    //    }
    //}
}
