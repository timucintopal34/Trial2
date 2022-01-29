using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    private bool isGameStarted = false;
    private bool isRaycasting = false;
    private bool startTimer = false;
    [SerializeField] 
    private bool ground = false;
    
    public Vector3 lastSafePosition;
    
    public float hideLimit = 3.0f;
    public float time;
    
    private void Start()
    {
        ColliderOn();

        UIManager.Instance.OnLevelStart += () =>
        {
            isGameStarted = true;
            isRaycasting = true;
            StartCoroutine(CheckGround());
        };
    }

    private void LateUpdate()
    {
        // transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0, transform.localEulerAngles.z);

    }

    private void Update()
    {
        //
        if (startTimer)
        {
            time += Time.deltaTime;
            //If the time reached to the time given then we stop raycasting
            //and move to the last position where we raycasted in road
            if(time >= hideLimit)
            {
                startTimer = false;
                isRaycasting = false;
                transform.position = lastSafePosition;
                // Debug.Log($"Moved To {lastSafePosition}");
                isRaycasting = true;
            }
        }
        else if (time != 0)
        {
            time = 0;
        }
    }
    
    
    IEnumerator CheckGround()
    {
        while (isGameStarted && isRaycasting)
        {
            int layerMask = 1 << 7;
            //The roads has a layer which is :7
            RaycastHit hit;
            
            //I raycast to the ground if we can hit Road layer then we record its position to be used in future
            if (Physics.Raycast(transform.position, Vector3.down ,out hit, 1000, layerMask))
            {
                    //Debug.DrawRay(transform.position, Vector3.down * 1000, Color.yellow);
                    startTimer = false;
                    ground = true;
                    //To see where is the way point
                    lastSafePosition = new Vector3(hit.point.x - 20f , hit.point.y +4f);
            }
            else //If we dont hit ground then we start to count the time
            {
                startTimer = true;
                ground = false;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    //To close car while switching to new car model
    public void MeshOff()
    {
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var mesh in meshes)
            mesh.enabled = false;
    }
    
    //To open the new car model after 2d/linerender to 3d mesh creation
    public void MeshOn()
    {
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var mesh in meshes)
            mesh.enabled = true;
    }
    
    
    //Open the new colliders of the car which are created
    public void ColliderOn()
    {
        var cols = transform.GetComponentsInChildren<MeshCollider>().ToList();
        foreach (var col in cols)
        {
            col.enabled = true;
            col.convex = true;
        }
    }
}
