using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawArea : Singleton<DrawArea>
{
    [SerializeField] private Transform upperBound;
    [SerializeField] private Transform lowerBound;
    [SerializeField] private Transform leftBound;
    [SerializeField] private Transform rightBound;
    
    public Vector2 GetPointOnArea(Vector3 position)
    {
        if (position.z < leftBound.position.z)
        {
            position.z = leftBound.position.z;
        }
        if (position.y < lowerBound.position.y)
        {
            position.y = lowerBound.position.y;
        }
        if (position.z > rightBound.position.z)
        {
            position.z = rightBound.position.z;
        }
        if (position.y > upperBound.position.y)
        {
            position.y = upperBound.position.y;
        }
        
        return new Vector2(position.z, position.y);
    }
}
