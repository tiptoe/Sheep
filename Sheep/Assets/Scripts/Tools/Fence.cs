using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Fence : DragTool {

    public float minLength = 1;
    public float maxLength = 4;

    Vector3 startPosition;
    Vector3 endPosition;

    GameObject fenceSolid;
    GameObject fenceGhost;
    ChangeFenceColor fenceGhostScript;

    public GameObject fenceObstacle;
    List<GameObject> obstacles = new List<GameObject>();
    GameObject g;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        fenceSolid = (GameObject)Instantiate(toolPrefab);
        HideFence(fenceSolid);
        //Debug.Log("Solid fence created.");

        fenceGhost = (GameObject)Instantiate(toolPrefab);
        //fenceGhostScript = fenceGhost.transform.GetChild(0).GetComponent<ChangeFenceColor>();
        fenceGhostScript = fenceGhost.transform.GetComponentInChildren<ChangeFenceColor>();
        fenceGhostScript.SetGhostFence(minLength, maxLength);
        HideFence(fenceGhost);
        //Debug.Log("Ghost fence created.");
    }

    protected override void OnBeginDrag(Vector3 position)
    {
        startPosition = endPosition = new Vector3(position.x, 0, position.z);
        UpdatePosition(fenceGhost); 
    }

    protected override void OnDrag(Vector3 position)
    {
        endPosition = new Vector3(position.x, 0, position.z);
        UpdatePosition(fenceGhost);
    }

    protected override void OnEndDrag(Vector3 position)
    {
        HideFence(fenceGhost);

        if (!fenceGhostScript.IsRed)
        {
            UpdatePosition(fenceSolid);
            CreatePrecisionObstacle();
            levelController.AddScore(score);
            //audio.Play();
        }
    }

    void UpdatePosition(GameObject fence)
    {
        //Debug.Log("start:" + startPosition);
        //Debug.Log("end:" + endPosition);
        fence.transform.position = startPosition;
        fence.transform.LookAt(endPosition);
        // TODO: performance optimalization
        fence.transform.GetChild(0).transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(startPosition, endPosition));
        fence.transform.GetChild(2).transform.position = new Vector3(endPosition.x, 0.9f, endPosition.z);
        //fence.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(startPosition, endPosition));
    }

    void HideFence(GameObject fence)
    {
        fence.transform.position = new Vector3(100, 100, 100);
    }

    /// <summary>
    /// místo jedné velké překážky vytvoří více malých
    /// je to trochu pomalejší, ale rozhodně přesnější
    /// volá se vždy když je dokreslen a umístěn plot
    /// </summary>
    void CreatePrecisionObstacle()
    {
        //smažu předchozí překážky
        foreach (GameObject gO in obstacles)
        {
            Destroy(gO);
        }
        if (g == null)
        {
            g = new GameObject("obstacles parent");
        }

        /* foreach (GameObject go in obstacles)
         {
             go.transform.parent = g.transform;

         }*/
        GameObject created;
        //vzdálenost, počet malých překážek které je potřeba umístit
        float dinstance = Vector3.Distance(startPosition, endPosition);
        float smallerObstaclesCount = dinstance / (0.5f);
        Vector3 startObstaclesPosition = startPosition;
        //první bude vždy na začátku
        created = (GameObject)GameObject.Instantiate(fenceObstacle, startObstaclesPosition, new Quaternion());
        created.GetComponent<MeshRenderer>().enabled = false;
        obstacles.Add(created);
        created.transform.parent = g.transform;

        startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
        for (int i = 0; i < smallerObstaclesCount - 1; i++)
        {
            //několik mezitim
            created = (GameObject)GameObject.Instantiate(fenceObstacle, startObstaclesPosition, new Quaternion());
            created.GetComponent<MeshRenderer>().enabled = false;
            obstacles.Add(created);
            created.transform.parent = g.transform;
            startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
        }
        //poslední bude vždy na konci
        created = (GameObject)GameObject.Instantiate(fenceObstacle, endPosition, new Quaternion());
        created.GetComponent<MeshRenderer>().enabled = false;
        obstacles.Add(created);
        created.transform.parent = g.transform;
        startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
    }
}
