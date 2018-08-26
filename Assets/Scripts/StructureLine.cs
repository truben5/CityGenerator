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
        setLength(start,end);
    }

    public Vector3 getStart()
    {
        return _start;
    }

    public Vector3 getEnd()
    {
        return _end;
    }

    public void setLength(Vector2f start, Vector2f end)
    {
        float x = Mathf.Abs(end.x - start.x);
        float y = Mathf.Abs(end.y - start.y);
        length = Mathf.Sqrt(x * x + y * y);
    }

    public float getLength()
    {
        return length;
    } 
}
