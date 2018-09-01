using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureLine {

    private Vector3 _start;
    private Vector3 _end;
    private float length;

    public StructureLine(Vector2f start, Vector2f end)
    {
        _start = new Vector3(start.x, start.y, 0);
        _end = new Vector3(end.x, end.y, 0);
    }

    public Vector3 getStart()
    {
        return _start;
    }

    public Vector3 getEnd()
    {
        return _end;
    }
}
