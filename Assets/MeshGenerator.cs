
using System;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private GameObject[] knives;
    private Vector3 _startPoint;
    private bool _startedDrawing = false;
    public List<Transform> nodes = new List<Transform>();

    private void Start()
    {
        Initialize();
    }

    [ContextMenu("Init")]
    private void Initialize()
    {
        nodes.Clear();
        var parent = FindObjectOfType<Spline>().transform.GetChild(1);
        foreach (Transform child in parent)
        {
            Debug.Log($"Name Child : {child.name}");
            nodes.Add(child);
        }
        Debug.Log($"Nodes amount {nodes.Count}");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnTapDown();
        }
        if (Input.GetMouseButton(0))
        {
            OnTap();
        }
        if (Input.GetMouseButtonUp(0))
        {
            OnTapUp();
        }
    }

    private void OnTap()
    {
        
    }
    
    private void OnTapDown()
    {
        trailRenderer.Clear();
        trailRenderer.enabled = true;
        Initialize();
    }
    
    private void OnTapUp()
    {
        if (trailRenderer.positionCount < 4)
        {
            trailRenderer.enabled = false;
            return;
        }
        Spawn3DMesh();
        trailRenderer.enabled = false;
        trailRenderer.Clear();
        if (!_startedDrawing)
        {
            _startedDrawing = true;
            // SetKnivesActive(true);
        }
    }

    private void Spawn3DMesh()
    {
        Vector3[] positions = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetPositions(positions);
        _startPoint = nodes[0].position;
        // Debug.Log($"Trail point amount : {trailRenderer.positionCount}");
        Debug.Log($"Ex Start :{_startPoint}");
        // Debug.Log(positions[0]);
        for (int i = 0; i < positions.Length; i++)
        {
            if(i == nodes.Count)
                break;
            positions[i] -= _startPoint;
            positions[i].z = 0;
            // nodes[i].position = new Vector3(positions[i].x, positions[i].y,0);
            nodes[i].position = positions[i];
        }
        Debug.Log($"Start :{positions[0]}");
    }
    
    

    public void SetKnivesActive(bool value)
    {
        for (int i = 0; i < knives.Length; i++)
        {
            knives[i].SetActive(value);
        }
    }
}
