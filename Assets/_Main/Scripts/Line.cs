using UnityEngine;
using System.Collections.Generic;

public class Line : MonoBehaviour {

    public LineRenderer lineRenderer;

    [HideInInspector] public List<Vector2> points = new List<Vector2> ( );
    [HideInInspector] public int pointsCount = 0;

    //The minimum distance between line's points.
    float pointsMinDistance = 0.1f;

    //Circle collider added to each line's point
    float circleColliderRadius;

    public void AddPoint ( Vector2 newPoint ) {
        //If distance between last point and new point is less than pointsMinDistance do nothing (return)
        if ( pointsCount >= 1 && Vector2.Distance ( newPoint, GetLastPoint ( ) ) < pointsMinDistance )
            return;

        // newPoint = DrawArea.Instance.GetPointOnArea(new Vector3(0, newPoint.y, newPoint.x));

        points.Add ( newPoint );
        pointsCount++;

        //Line Renderer
        lineRenderer.positionCount = pointsCount;
        lineRenderer.SetPosition ( pointsCount - 1, newPoint );

    }

    public Vector2 GetLastPoint ( ) {
        return ( Vector2 )lineRenderer.GetPosition ( pointsCount - 1 );
    }

    public void SetLineColor ( Gradient colorGradient ) {
        lineRenderer.colorGradient = colorGradient;
    }

    public void SetPointsMinDistance ( float distance ) {
        pointsMinDistance = distance;
    }

}