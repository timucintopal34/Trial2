using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushMovement : Singleton<BrushMovement>
{
    [SerializeField] private Transform brushTransform;
    [SerializeField] private LayerMask brushBoardMask;
    [SerializeField] private Camera brushCamera;
    [SerializeField] private DrawArea drawArea;
    
    // void Update()
    // {
    //     brushTransform.position = Position();
    // }

    public Vector3 Position()
    {
        RaycastHit hit;
        Ray ray = brushCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 100, layerMask: brushBoardMask))
        {
            return drawArea.GetPointOnArea(hit.point);
        }

        return default;
    }
}
