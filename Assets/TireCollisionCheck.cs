using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireCollisionCheck : MonoBehaviour
{
    public bool isTriggered = false;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = FindObjectOfType<MeshController>().GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            isTriggered = true;
            _rigidbody.AddForce(Vector3.right * 5 , ForceMode.Force);
        }
    }

    
}
