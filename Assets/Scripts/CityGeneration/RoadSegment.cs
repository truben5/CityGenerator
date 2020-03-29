using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment
{
    public Line closerLine;
    public Line furtherLine;

    private float slope;

    public RoadSegment(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
    {
        closerLine = new Line(v1, v2);
        furtherLine = new Line(v3, v4);

        slope = Geometry.Slope(v1, v2);
    }
}
