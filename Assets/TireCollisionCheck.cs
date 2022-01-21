using System;
using UnityEngine;

public class TireCollisionCheck : MonoBehaviour
{
    public bool isTriggered = false;
    private bool isGameEnded = false;
    public float forceAmount = 5;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = FindObjectOfType<MeshController>().GetComponent<Rigidbody>();
    }

    private void Start()
    {
        UIManager.Instance.OnLevelEnd += () =>
        {
            isGameEnded = true;
        };
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7 && !isGameEnded)
        {
            Debug.Log("Force!");
            isTriggered = true;
            _rigidbody.AddForce(Vector3.right * forceAmount , ForceMode.Force);
        }
    }

    
}
