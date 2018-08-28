﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellFillTest : MonoBehaviour {

    public GameObject voronoiCell;

    // Use this for initialization
    void Start()
    {
        voronoiCell = Instantiate(voronoiCell);
        Debug.Log("Beginning Tests...");
        Debug.Log("Testing Intersection Function");
        IntersectionTests();
        Debug.Log("Testing LineIntersection Function");
        LineIntersectionTests();

    }

    public void IntersectionTests()
    {
        Debug.Log("Test 1");
        Vector2f s1 = new Vector2f(10, 20);
        Vector2f e1 = new Vector2f(10, 30);
        Vector2f s2 = new Vector2f(30, 40);
        Vector2f e2 = new Vector2f(20, 20);
        Vector2f intersection = voronoiCell.GetComponent<CellFill>().Intersection(s1, e1, s2, e2);
        if (intersection.x != 10 && intersection.y != 0)
        {
            throw new System.Exception("Expected (10,0) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 2");

        s1 = new Vector2f(10, 10);
        e1 = new Vector2f(20, 10);
        s2 = new Vector2f(20, 30);
        e2 = new Vector2f(30, 20);
        intersection = voronoiCell.GetComponent<CellFill>().Intersection(s1, e1, s2, e2);
        if (intersection.x != 40 && intersection.y != 10)
        {
            throw new System.Exception("Expected (40,10) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 3");
        s2 = new Vector2f(30, 30);
        e2 = new Vector2f(30, 20);
        intersection = voronoiCell.GetComponent<CellFill>().Intersection(s1, e1, s2, e2);
        if (intersection.x != 30 && intersection.y != 10)
        {
            throw new System.Exception("Expected (10,30) but received: " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 4");

        s1 = new Vector2f(10, 10);
        e1 = new Vector2f(10, 40);
        try
        {
            intersection = voronoiCell.GetComponent<CellFill>().Intersection(s1, e1, s2, e2);
            Debug.Log("ERROR: Intersection should have thrown an exception for parallel lines");
        }
        catch
        {
            Debug.Log("All Intersecion tests pass");
        }
    }

    public void LineIntersectionTests()
    {
        List<Vector2f> plot = new List<Vector2f>();
        plot.Add(new Vector2f(10, 40));
        plot.Add(new Vector2f(10,30));
        plot.Add(new Vector2f(20, 20));
        plot.Add(new Vector2f(30,40));

        // Slope between (10,40) and (10,30)
        float slope = voronoiCell.GetComponent<CellFill>().Slope(plot[0].x, plot[0].y, plot[1].x, plot[1].y);
        float invSlope = voronoiCell.GetComponent<CellFill>().InvSlope(slope);
        Vector2f midPoint = voronoiCell.GetComponent<CellFill>().Midpoint(plot[0].x,plot[0].y, plot[1].x, plot[1].y);
        // Line segment bisector intersects with
        List<Vector2f> lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(1, new Vector2f(10,35), slope, plot);
        if (lineSeg[0] != plot[2] && lineSeg[0] != plot[3])
        {
            throw new System.Exception("Extpected (20,20) and (30,40) but received " + lineSeg[0] + " and " + lineSeg[1]);
        }

        Vector2f nextPoint;
        // Point of intersection between bisector and line segment
        Vector2f intersection = voronoiCell.GetComponent<CellFill>().Intersection(plot[0], plot[1], plot[2], plot[3]);
        if (intersection.x != 27.5 && intersection.y != 35)
        {
            throw new System.Exception("Extpected (27.5,35) but received " + intersection.x + ", " + intersection.y);
        }

        slope = voronoiCell.GetComponent<CellFill>().Slope(plot[1].x, plot[1].y, plot[2].x, plot[2].y);
        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(2, new Vector2f(15, 25), slope, plot);
        Debug.Log(lineSeg[0] + " " + lineSeg[1]);
    }


}
