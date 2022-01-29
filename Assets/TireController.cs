using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TireController : Singleton<TireController>
{
    public bool isTriggered = false;
    private bool isGameEnded = false;
    public float forceAmount = 5;
    private Rigidbody _rigidbody;

    [SerializeField] private WheelCollider front;
    [SerializeField] private WheelCollider rear;
    
    [SerializeField] private GameObject frontModel;
    [SerializeField] private GameObject rearModel;

    private bool frontIsGrounded = false;
    private bool rearIsGrounded = false;

    public float wheelPower = 100;
    public int multiplier = 100;

    public float maxSpeed = 5;
    public Vector3 velocity = Vector3.zero;
    public Vector3 angularVelocity = Vector3.zero;

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

    private void LateUpdate()
    {
        AnimateWheels();

        // if (front.transform.position.x < rear.transform.position.x)
        // {
        //     Debug.Log($"Swap {front.transform.position.x} - {rear.transform.position.x}");
        //     var tempFront = front.transform.localPosition;
        //     var tempRear = rear.transform.localPosition;
        //     front.transform.localPosition = tempRear;
        //     rear.transform.localPosition = tempFront;
        // }
    }

    void FixedUpdate()
    {
        if (front.isGrounded && rear.isGrounded && !isGameEnded)
        {
            front.motorTorque = wheelPower *  multiplier * Time.deltaTime;
            rear.motorTorque = wheelPower * multiplier * Time.deltaTime;
        }
        ////////
        // if (front.isGrounded)
        // {
        //     Debug.Log("Front Grounded");
        //     front.motorTorque = wheelPower *  multiplier * Time.deltaTime;
        //     frontIsGrounded = true;
        // }
        // else
        //     frontIsGrounded = false;
        //
        // if (rear.isGrounded)
        // {
        //     Debug.Log("Rear Grounded");
        //     rear.motorTorque = wheelPower * multiplier * Time.deltaTime;
        // }
        // else
        //     rearIsGrounded = false;
        
        if(_rigidbody.velocity.magnitude > maxSpeed)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * maxSpeed;
        }
        
    }

    private void AnimateWheels()
    {
        Quaternion _rot;
        Vector3 _pos;
        
        // if(front.isGrounded)
        // {
            front.GetWorldPose(out _pos, out _rot);
            frontModel.transform.position = _pos;
            if(front.isGrounded )
                frontModel.transform.rotation = _rot;
        // }

        // if (rear.isGrounded)
        // {
            rear.GetWorldPose(out _pos,out _rot);
            rearModel.transform.position = _pos;
            if (rear.isGrounded )
                rearModel.transform.rotation = _rot;
        // }

        if ((rear.isGrounded || front.isGrounded) && !isGameEnded)
        {
            velocity = _rigidbody.velocity;
            angularVelocity = _rigidbody.angularVelocity;
        }
    }

    public void ReUseSpeed()
    {
        _rigidbody.velocity = velocity/2;
        // _rigidbody.angularVelocity = angularVelocity;
    }

    // private void ()
    // {
    //     
    // }

    private void OnTriggerStay(Collider other)
    {
        // if (other.gameObject.layer == 7 && !isGameEnded)
        // {
        //     isTriggered = true;
        //     _rigidbody.AddForce(Vector3.right * forceAmount , ForceMode.Force);
        // }
    }

    
}
