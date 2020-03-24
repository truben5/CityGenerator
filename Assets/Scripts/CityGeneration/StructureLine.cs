using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureLine {

    private Vector3 _start;
    private Vector3 _end;
    private float length;

    public StructureLine(Vector3 start, Vector3 end)
    {
        _start = start;
        _end = end;
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
