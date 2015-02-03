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

    private GameObject g;

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
                LevelController controller = FindObjectOfType<LevelController>();
                if (controller != null)
                {
                   
                    foreach (GameObject sheep in controller.Sheeps)
                    {
                        SheepAI sheepAI = sheep.GetComponent<SheepAI>();
                        if (sheepAI != null)
                        {
                            sheepAI.FenceBuild();
                        }
                    }

                    foreach (GameObject wolf in controller.Wolves)
                    {
                        WolfAI wolfAI = wolf.GetComponent<WolfAI>();
                        if (wolfAI != null)
                        {
                            wolfAI.FenceBuild();
                        }
                    }
                }

            }
 
        }
	}

    void UpdatePosition(GameObject fence, Vector3 startPosition, Vector3 endPosition)
    {
        //Debug.Log("start:" + startPosition);
        //Debug.Log("end:" + endPosition);
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
        fenceGhostScript.SetGhostFence(1f,4f);
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
		audio.Play();
        
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
        for (int i = 0; i < smallerObstaclesCount-1; i++)
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
