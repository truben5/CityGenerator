using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Geometry
{
    // Calculates distance between two points
    public static float Distance(float x1, float y1, float x2, float y2)
    {
        float x = Mathf.Abs(x2 - x1);
        float y = Mathf.Abs(y2 - y1);
        float dist = Mathf.Sqrt(x * x + y * y);
        return dist;
    }

    // Calculates midpoint between two points
    public static Vector3 Midpoint(float x1, float y1, float x2, float y2)
    {
        Vector3 mid = new Vector3((x1 + x2) / 2, (y1 + y2) / 2, 0);
        return mid;
    }


    // Calculates slope between two points
    public static float Slope(float x1, float y1, float x2, float y2)
    {
        float slope = (y2 - y1) / (x2 - x1);
        return slope;
    }

    // Calculates inverse slope of line
    public static float InvSlope(float slope)
    {
        float invSlope = -1 * (1 / slope);
        return invSlope;
    }

    // Finds point of intersection two lines
    public static Vector3 Intersection(Vector3 s1, Vector3 e1, Vector3 s2, Vector3 e2)
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

        if (determinant == 0)
        {
            throw new System.Exception("Determinant is 0, lines are parallel");
        }

        float x = (b2 * c1 - b1 * c2) / determinant;
        float y = (a1 * c2 - a2 * c1) / determinant;
        return new Vector3(x, y, 0);
    }

    // Takes in the slope and y intercept for a line equation.  Determines if point is below, above, or on line 
    // If the slope is infinite (vertical line) uses the x values to determine if point is right or left of line
    public static int RelationToLine(float m, float b, Vector3 p, float midPointX)
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
        else
        {
            throw new System.Exception("unthought of case");
        }
    }
}
