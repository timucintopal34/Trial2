using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    private bool isGameStarted = false;
    public bool ground = false;
    private bool isRaycasting = false;
    public Transform box;

    public Vector3 lastSafePosition;

    private bool startTimer = false;
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

    private void Update()
    {
        if (startTimer)
        {
            time += Time.deltaTime;
            if(time >= 1.0f)
            {
                startTimer = false;
                isRaycasting = false;
                transform.position = lastSafePosition;
                Debug.Log($"Moved To {lastSafePosition}");
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
            
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, Vector3.down ,out hit, 1000, layerMask))
            {
                    Debug.Log($"Layer Hit: {hit.collider.gameObject.layer}");
                    Debug.DrawRay(transform.position, Vector3.down * 1000, Color.yellow);
                    startTimer = false;
                    ground = true;
                    box.position = hit.point;
                    lastSafePosition = new Vector3(hit.point.x - 10 , hit.point.y + 5);
               
            }
            else
            {
                startTimer = true;
                ground = false;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    public void MeshOff()
    {
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var mesh in meshes)
            mesh.enabled = false;
    }
    
    public void MeshOn()
    {
        var meshes = transform.GetComponentsInChildren<MeshRenderer>().ToList();
        foreach (var mesh in meshes)
            mesh.enabled = true;
    }

    public void ColliderOn()
    {
        var cols = transform.GetComponentsInChildren<MeshCollider>().ToList();
        Debug.Log($"Mesh Colliders {cols.Count}");
        foreach (var col in cols)
        {
            col.enabled = true;
            col.convex = true;
        }
    }
}
