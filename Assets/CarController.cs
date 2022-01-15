using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool applyForce = false;
    public float forceAmount = 1;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (applyForce)
        {
            _rigidbody.AddForce(Vector3.right * forceAmount, ForceMode.Force);
        }
    }
}
