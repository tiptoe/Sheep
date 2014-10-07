using UnityEngine;
using System.Collections;

public class FenceDraw : MonoBehaviour
{
    public GameObject fencePrefab;

    Vector3 startPosition;
    Vector3 endPosition;

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
                UpdatePosition(fenceSolid, startPosition, endPosition);    
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
}
