using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireCollisionCheck : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"I collided with {collision.gameObject.name} boss");
    }
}
