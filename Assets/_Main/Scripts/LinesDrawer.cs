using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SplineMesh;
using UnityEngine;

public class LinesDrawer : MonoBehaviour {

    public GameObject linePrefab;

    [Space ( 30f )]
    public Gradient lineColor;
    
    public float linePointsMinDistance;
    
    Line currentLine;

    public Camera cam;

    private List<Vector2> points = new List<Vector2>();
    public List<Transform> nodes = new List<Transform>();
    
    private RopeBuilder _ropeBuilder;

    private MeshController _meshController;
    private Rigidbody rb;

    private CarController _carController;

    public Transform rearTire;
    public Transform frontTire;

    private void Awake()
    {
        _ropeBuilder = FindObjectOfType<RopeBuilder>();
        _meshController = FindObjectOfType<MeshController>();
        rb = _meshController.GetComponent<Rigidbody>();
        _carController = FindObjectOfType<CarController>();
    }
    
    private void Initialize()
    {
        nodes.Clear();
        var parent = FindObjectOfType<Spline>().transform.GetChild(1);
        foreach (Transform child in parent)
        {
            nodes.Add(child);
        }
        // Debug.Log($"Nodes amount {nodes.Count}");
    }

    void Update ( ) {
        if ( Input.GetMouseButtonDown ( 0 ) )
            BeginDraw();

        if ( currentLine != null )
            Draw ( );

        if ( Input.GetMouseButtonUp ( 0 ) )
            EndDraw ( );
    }

    // Begin Draw ----------------------------------------------
    void BeginDraw ( ) {
        currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );
        
        Time.timeScale = .5f;
        // currentLine.SetLineColor ( lineColor );
        currentLine.SetPointsMinDistance ( linePointsMinDistance );
        
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        var layerMask = 1 << 3;
        if (!Physics.Raycast(ray, out hit, 100, layerMask: layerMask))
        {
            EndDraw();
        }

    }
    // Draw ----------------------------------------------------
    void Draw ( ) {
        Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );
        //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
        // RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );
        
        // if ( !hit )
        // {
        //     Debug.Log("End Draw");
        //     EndDraw();
        // }
        // else
        // {
        // DrawArea.Instance.GetPointOnArea(mousePosition);        
        
        currentLine.AddPoint(DrawArea.Instance.GetPointOnArea(mousePosition));
        // currentLine.AddPoint(mousePosition);
        // }
    }
    
    // End Draw ------------------------------------------------
    void EndDraw ( ) {
        Time.timeScale = 1;
        if ( currentLine != null ) {
            if ( currentLine.pointsCount < 2 ) {
                //If line has one point
                Destroy ( currentLine.gameObject );
            } else {

                StartCoroutine(Spawn3DMesh());
                Destroy(currentLine.gameObject);
                currentLine = null;
            }
        }
        
        
    }
    
    IEnumerator Spawn3DMesh()
    {
        
        _meshController.MeshOff();
        rearTire.SetParent(null);
        frontTire.SetParent(null);
        points = currentLine.points;
        
        _meshController.transform.localEulerAngles = Vector3.zero;
        // yield return new WaitForEndOfFrame();
        _ropeBuilder.ChangeSegmentCount(points.Count);
        var startPoint = points[0];
        yield return new WaitForSeconds(.01f);
        // yield return new WaitForEndOfFrame();
        Initialize();
        // Debug.Log($"Point Count : {points.Count}");
        // Debug.Log($"Start : {startPoint}");
        _meshController.ColliderOn();
        
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= startPoint;

            nodes[i].localPosition = new Vector2(points[i].x, points[i].y );
        }

        _meshController.transform.position = new Vector3(_meshController.transform.position.x,
            _meshController.transform.position.y + 1, 0);

        if (!rb.useGravity)
        {
            _carController.applyForce = true;
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        
        rearTire.SetParent(_meshController.transform);
        frontTire.SetParent(_meshController.transform);
        
        rearTire.localPosition = nodes[0].localPosition;
        frontTire.localPosition = nodes[nodes.Count - 1].localPosition;
        
        yield return new WaitForSeconds(.01f);
        
        _meshController.MeshOn();
        


    }
}