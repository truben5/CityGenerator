using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSegment : MonoBehaviour
{
    public Vector3 x1;
    public Vector3 x2;
    public Vector3 y1;
    public Vector3 y2;

    public RoadSegment(Vector3 x_1, Vector3 x_2, Vector3 y_1, Vector3 y_2)
    {
        x1 = x_1;
        x2 = x_2;
        y1 = y_1;
        y2 = y_2;
    }
}
