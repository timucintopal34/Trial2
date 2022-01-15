using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    private void Start()
    {
        ColliderOn();
    }

    public void ColliderOn()
    {
        
        var cols = transform.GetComponentsInChildren<Collider>().ToList();
        Debug.Log($"Mesh Colliders {cols.Count}");
        foreach (var col in cols)
            col.enabled = true;
    }
}
