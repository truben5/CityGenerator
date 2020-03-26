using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line {

    private Vector3 _start;
    private Vector3 _end;
    private float length;

    public Line(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
    }

    public Vector3 GetStart()
    {
        return _start;
    }

    public Vector3 GetEnd()
    {
        return _end;
    }
}
