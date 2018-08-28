using System.Collections;
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
        if (intersection.x != 30 || intersection.y != 10)
        {
            throw new System.Exception("Expected (10,30) but received: " + intersection.x + ", " + intersection.y);
        }

        //Debug.Log("Test 4");

        //s1 = new Vector2f(10, 10);
        //e1 = new Vector2f(10, 40);
        //try
        //{
        //    intersection = voronoiCell.GetComponent<CellFill>().Intersection(s1, e1, s2, e2);
        //    Debug.Log("ERROR: Intersection should have thrown an exception for parallel lines");
        //}
        //catch
        //{
        //    Debug.Log("All Intersecion tests pass");
        //}
    }

    public void LineIntersectionTests()
    {
        List<Vector2f> plot = new List<Vector2f>();
        plot.Add(new Vector2f(10, 40));
        plot.Add(new Vector2f(10,30));
        plot.Add(new Vector2f(20, 20));
        plot.Add(new Vector2f(30,40));

        Debug.Log("Test 1");
        // Slope between (20,20) and (30,40)
        float slope = voronoiCell.GetComponent<CellFill>().Slope(plot[2].x, plot[2].y, plot[3].x, plot[3].y);
        Debug.Log(slope);
        float invSlope = voronoiCell.GetComponent<CellFill>().InvSlope(slope);
        Vector2f midPoint = voronoiCell.GetComponent<CellFill>().Midpoint(plot[2].x, plot[2].y, plot[3].x, plot[3].y);
        Debug.Log(invSlope);
        List<Vector2f> lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(3, midPoint, invSlope, plot);
        if (lineSeg[0] != plot[1] || lineSeg[1] != plot[0])
        {
            throw new System.Exception("Extpected (10,40) and (10,30) but received " + lineSeg[1] + " and " + lineSeg[0]);
        }

        Debug.Log("Test 2");
        // Slope between (10,40) and (10,30)
        slope = voronoiCell.GetComponent<CellFill>().Slope(plot[0].x, plot[0].y, plot[1].x, plot[1].y);
        invSlope = voronoiCell.GetComponent<CellFill>().InvSlope(slope);
        midPoint = voronoiCell.GetComponent<CellFill>().Midpoint(plot[0].x,plot[0].y, plot[1].x, plot[1].y);
        // Line segment bisector intersects with
        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(1, midPoint, invSlope, plot);
        if (lineSeg[1] != plot[2] || lineSeg[0] != plot[3])
        {
            throw new System.Exception("Extpected (20,20) and (30,40) but received " + lineSeg[1] + " and " + lineSeg[0]);
        }

        Vector2f nextPoint = new Vector2f(midPoint.x + 1, midPoint.y + invSlope);
        // Point of intersection between bisector and line segment
        Vector2f intersection = voronoiCell.GetComponent<CellFill>().Intersection(midPoint, nextPoint, lineSeg[0], lineSeg[1]);
        if (intersection.x != 27.5 || intersection.y != 35)
        {
            throw new System.Exception("Extpected (27.5,35) but received " + intersection.x + ", " + intersection.y);
        }

        Debug.Log("Test 3");
        // Between (30,40) and (10,40)
        slope = voronoiCell.GetComponent<CellFill>().Slope(plot[3].x, plot[3].y, plot[0].x, plot[0].y);
        Debug.Log(slope);
        invSlope = voronoiCell.GetComponent<CellFill>().InvSlope(slope);
        midPoint = voronoiCell.GetComponent<CellFill>().Midpoint(plot[3].x, plot[3].y, plot[0].x, plot[0].y);
        lineSeg = voronoiCell.GetComponent<CellFill>().LineIntersection(1, midPoint, invSlope, plot);
        if (lineSeg.Count > 1 && lineSeg[0] != plot[2])
        {
            throw new System.Exception("Extpected (20,20) and (30,40) but received " + lineSeg[1] + " and " + lineSeg[0]);
        }
    }

}
