using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool applyForce = false;
    public float forceAmount = 1;

    private Rigidbody _rigidbody;

    private Vector3 previousPosition;

    [SerializeField] private float xVelocity;
    [SerializeField] private float speed;

    private List<TireCollisionCheck> tires = new List<TireCollisionCheck>();

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        tires = FindObjectsOfType<TireCollisionCheck>().ToList();
    }

    private void Start()
    {
        UIManager.Instance.OnLevelStart += () =>
        {
            previousPosition = transform.position;
            applyForce = true;
            // StartCoroutine(ApplyForce());
        };
    }

    private void Update()
    {
        if (applyForce)
        {
            xVelocity = _rigidbody.velocity.x;
            speed = transform.position.x - previousPosition.x;
            previousPosition = transform.position;
        }
    }


    IEnumerator ApplyForce()
    {
        var cnt = 0;
        while (applyForce)
        {
            
            cnt = 0;
            foreach (var tire in tires)
            {
                if (tire.isTriggered)
                    cnt++;
            }
            
            if (cnt == 1)
            {
                Debug.Log("Force 1");
                _rigidbody.AddForce(Vector3.right * forceAmount/2, ForceMode.Force);
            }
            else if(cnt ==2)
            {
                Debug.Log("Force 2");
                _rigidbody.AddForce(Vector3.right * forceAmount , ForceMode.Force);
            }
            Debug.Log($"Apply Force! {cnt}");
            yield return new WaitForSeconds(.05f);
        }
    }
}
