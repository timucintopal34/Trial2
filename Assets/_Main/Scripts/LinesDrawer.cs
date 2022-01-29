using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SplineMesh;
using UnityEngine;

public class LinesDrawer : MonoBehaviour
{
    public float timeScale = 2;
    public float slowMotionTimeScale = .4f;
    
    private bool isGameStarted = false;
    private bool isGameEnded = false;
    
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

    public Transform rearTire;
    public Transform frontTire;

    private void Awake()
    {
        _ropeBuilder = FindObjectOfType<RopeBuilder>();
        _meshController = FindObjectOfType<MeshController>();
        rb = _meshController.GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Time.timeScale = timeScale;
        UIManager.Instance.OnLevelStart += () =>
        {
            isGameStarted = true;
        };

        UIManager.Instance.OnLevelEnd += () =>
        {
            isGameEnded = true;
        };
    }

    //Get the list of the rope so we can move the 
    private void Initialize()
    {
        nodes.Clear();
        var parent = FindObjectOfType<Spline>().transform.GetChild(1);
        
        foreach (Transform child in parent)
            nodes.Add(child);
    }

    void Update ( ) {
        if(isGameStarted && !isGameEnded)
        {
            if (Input.GetMouseButtonDown(0))
                BeginDraw();

            if (currentLine != null)
                Draw();

            if (Input.GetMouseButtonUp(0))
                EndDraw();
        }
    }

    void BeginDraw ( ) {
        
        currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );
        
        //Give Slowmotion while drawin on action as your game has
        Time.timeScale = slowMotionTimeScale;
        
        currentLine.SetPointsMinDistance ( linePointsMinDistance );
        
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        var layerMask = 1 << 3;
        //If our first click didn't touch the draw plane then we end drawing
        if (!Physics.Raycast(ray, out hit, 100, layerMask: layerMask))
            EndDraw();

    }
    
    void Draw ( ) {
        Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );
        
        //We get the each point and keep it in the boundary of the draw plane
        currentLine.AddPoint(DrawArea.Instance.GetPointOnArea(mousePosition));
    }
    
    void EndDraw ( ) {
        Time.timeScale = timeScale;
        if ( currentLine != null ) {
            if ( currentLine.pointsCount < 2 ) {
                //If line has one point
                Destroy ( currentLine.gameObject );
            } else {
                //We create the car end erase the line renderer
                StartCoroutine(Spawn3DMesh());
                Destroy(currentLine.gameObject);
                currentLine = null;
                if(!rearTire.gameObject.activeSelf)
                    rearTire.gameObject.SetActive(true);
                if(!frontTire.gameObject.activeSelf)
                    frontTire.gameObject.SetActive(true);
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
        
        //We let rope builder to build the amount of points in line renderer
        _ropeBuilder.ChangeSegmentCount(points.Count);
        var startPoint = points[0];
        yield return new WaitForSeconds(.01f);
        
        Initialize();
        
        //On Colliders on Car 
        _meshController.ColliderOn();
        
        for (int i = 0; i < points.Count; i++)
        {
            points[i] -= startPoint;

            nodes[i].localPosition = new Vector3(0, points[i].y ,points[i].x);
        }

        _meshController.transform.position = new Vector3(_meshController.transform.position.x,
            _meshController.transform.position.y + 3, 0);

        if (!rb.useGravity)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
        
        rearTire.SetParent(_meshController.transform);
        frontTire.SetParent(_meshController.transform);
        
        //To position the tires right place on car meshes - which is the start of our line renderer and end of it
        rearTire.localPosition = nodes[0].localPosition;
        frontTire.localPosition = nodes[nodes.Count - 1].localPosition;
        
        yield return new WaitForSeconds(.01f);
        
        //To view the mesh of the car build
        _meshController.MeshOn();
        TireController.Instance.ReUseSpeed();
        


    }
}