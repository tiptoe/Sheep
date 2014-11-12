using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FenceDraw : MonoBehaviour
{
    public GameObject fencePrefab;

    Vector3 startPosition;
    Vector3 endPosition;

    public GameObject fenceObstacle;
    List<GameObject> obstacles = new List<GameObject>();

    GameObject fenceSolid;
    GameObject fenceGhost;
    ChangeFenceColor fenceGhostScript;

    void Awake()
    {
        Initialize();
    }

	void Update () 
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(mouseRay, out hit, 100))
            {
                if (Input.GetMouseButtonDown(0))
                    startPosition = new Vector3(hit.point.x, 0, hit.point.z);

                endPosition = new Vector3(hit.point.x, 0, hit.point.z);
                UpdatePosition(fenceGhost, startPosition, endPosition);
            }  
        }

        if (Input.GetMouseButtonUp(0))
        {
            HideFence(fenceGhost);

            if (!fenceGhostScript.IsRed)
            {
                UpdatePosition(fenceSolid, startPosition, endPosition);
                CreatePrecisionObstacle();
            }
 
        }
	}

    void UpdatePosition(GameObject fence, Vector3 startPosition, Vector3 endPosition)
    {
        Debug.Log("start:" + startPosition);
        Debug.Log("end:" + endPosition);
        fence.transform.position = startPosition;
        fence.transform.LookAt(endPosition);
        fence.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(startPosition, endPosition));
    }

    void Initialize()
    {
        fenceSolid= (GameObject)Instantiate(fencePrefab);
        HideFence(fenceSolid);
        Debug.Log("Solid fence created.");

        fenceGhost = (GameObject)Instantiate(fencePrefab);
        fenceGhostScript = fenceGhost.transform.GetChild(0).GetComponent<ChangeFenceColor>();
        fenceGhostScript.SetGhostFence();
        HideFence(fenceGhost);
        Debug.Log("Ghost fence created.");
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
        //vzdálenost, počet malých překážek které je potřeba umístit
        float dinstance = Vector3.Distance(startPosition, endPosition);
        float smallerObstaclesCount = dinstance / (0.5f);
        Vector3 startObstaclesPosition = startPosition;
        //první bude vždy na začátku
        obstacles.Add((GameObject)GameObject.Instantiate(fenceObstacle, startObstaclesPosition, new Quaternion()));
        startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
        for (int i = 0; i < smallerObstaclesCount-1; i++)
        {
           //několik mezitim
            obstacles.Add((GameObject)GameObject.Instantiate(fenceObstacle, startObstaclesPosition, new Quaternion()));
           startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
        }
        //poslední bude vždy na konci
        obstacles.Add((GameObject)GameObject.Instantiate(fenceObstacle, endPosition, new Quaternion()));
        startObstaclesPosition = Vector3.MoveTowards(startObstaclesPosition, endPosition, 0.5f);
    }
}
