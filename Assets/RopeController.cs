using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    private void Start()
    {
        CloseColliders();
    }

    [ContextMenu("CloseCol")]
    public void CloseColliders()
    {
        
        var colliders = transform.GetComponentsInChildren<Collider>().ToList();

        foreach (var col in colliders)
            col.enabled = false;

        var rbs = transform.GetComponentsInChildren<Rigidbody>().ToList();

        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
        
        Debug.Log($"Close Colliders {colliders.Count} rb {rbs.Count}");
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
