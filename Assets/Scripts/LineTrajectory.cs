using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineTrajectory : MonoBehaviour
{
    public LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint, bool dragBack)
    {
        lr.positionCount = 2;
        Vector3[] points = new Vector3[2];
        if (dragBack)
        {
            points[0] = startPoint;
            points[1] = endPoint;
        } else
        {
            points[0] = endPoint;
            points[1] = startPoint;
        }
        

        lr.SetPositions(points);
    }

    public void EndLine()
    {
        lr.positionCount = 0;
    }

}
