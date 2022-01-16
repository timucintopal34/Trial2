using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArea : Singleton<DrawArea>
{
    [SerializeField] private Transform upperBound;
    [SerializeField] private Transform lowerBound;
    [SerializeField] private Transform leftBound;
    [SerializeField] private Transform rightBound;
    
    public Vector3 GetPointOnArea(Vector3 position)
    {
        if (position.x < leftBound.position.x)
        {
            position.x = leftBound.position.x;
        }
        if (position.y < lowerBound.position.y)
        {
            position.y = lowerBound.position.y;
        }
        if (position.x > rightBound.position.x)
        {
            position.x = rightBound.position.x;
        }
        if (position.y > upperBound.position.y)
        {
            position.y = upperBound.position.y;
        }
        
        return position;
    }
}
