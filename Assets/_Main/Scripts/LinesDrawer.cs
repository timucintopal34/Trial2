using System;
using System.Collections;
using System.Collections.Generic;
using SplineMesh;
using UnityEngine;

public class LinesDrawer : MonoBehaviour {

    public GameObject linePrefab;
    public LayerMask cantDrawOverLayer;
    int cantDrawOverLayerIndex;

    [Space ( 30f )]
    public Gradient lineColor;
    public float linePointsMinDistance;
    public float lineWidth;
    
    Line currentLine;

    public Camera cam;

    private List<Vector2> points = new List<Vector2>();
    private RopeBuilder _ropeBuilder;
    
    public List<Transform> nodes = new List<Transform>();

    private void Awake()
    {
        _ropeBuilder = FindObjectOfType<RopeBuilder>();
    }
    
    private void Initialize()
    {
        nodes.Clear();
        var parent = FindObjectOfType<Spline>().transform.GetChild(1);
        foreach (Transform child in parent)
        {
            // Debug.Log($"Name Child : {child.name}");
            nodes.Add(child);
        }
        Debug.Log($"Nodes amount {nodes.Count}");
    }

    void Start ( ) {
        // cam = Camera.main;
        cantDrawOverLayerIndex = LayerMask.NameToLayer ( "CantDrawOver" );
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
        
        //Set line properties
        // currentLine.transform.localPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        currentLine.UsePhysics ( false );
        currentLine.SetLineColor ( lineColor );
        currentLine.SetPointsMinDistance ( linePointsMinDistance );
        currentLine.SetLineWidth ( lineWidth );

    }
    // Draw ----------------------------------------------------
    void Draw ( ) {
        Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );
        //Check if mousePos hits any collider with layer "CantDrawOver", if true cut the line by calling EndDraw( )
        // RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

        // if ( hit )
        // {
        //     Debug.Log("End Draw");
        //     EndDraw();
        // }
        // else
        // {
            currentLine.AddPoint(mousePosition);
        // }
    }
    // End Draw ------------------------------------------------
    void EndDraw ( ) {
        if ( currentLine != null ) {
            if ( currentLine.pointsCount < 2 ) {
                //If line has one point
                Destroy ( currentLine.gameObject );
            } else {
                //Add the line to "CantDrawOver" layer
                // currentLine.gameObject.layer = cantDrawOverLayerIndex;

                //Activate Physics on the line
                currentLine.UsePhysics ( false );
                StartCoroutine(Spawn3DMesh());

                Destroy(currentLine.gameObject);
                currentLine = null;
            }
        }
        
        
    }
    
    IEnumerator Spawn3DMesh()
    {
        points = currentLine.points;
        _ropeBuilder.ChangeSegmentCount(points.Count);
        var startPoint = points[0];
        yield return new WaitForSeconds(.1f);
        Initialize();
        Debug.Log($"Point Count : {points.Count}");
        Debug.Log($"Start : {startPoint}");
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= startPoint;
            
            nodes[i].localPosition = points[i];
        }
        Debug.Log($"Start new: {nodes[0].localPosition}");
        // if(nodes.Count>points.Count)
        //     for (int n = points.Count; n < nodes.Count ; n++)
        //     {
        //         nodes[n].gameObject.SetActive(false);
        //     }
    }
}