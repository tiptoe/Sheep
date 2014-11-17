using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fence : DragTool {

    Vector3 startPosition;
    Vector3 endPosition;

    GameObject fenceSolid;
    GameObject fenceGhost;
    ChangeFenceColor fenceGhostScript;

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        fenceSolid = (GameObject)Instantiate(toolPrefab);
        HideFence(fenceSolid);
        Debug.Log("Solid fence created.");

        fenceGhost = (GameObject)Instantiate(toolPrefab);
        fenceGhostScript = fenceGhost.transform.GetChild(0).GetComponent<ChangeFenceColor>();
        fenceGhostScript.SetGhostFence();
        HideFence(fenceGhost);
        Debug.Log("Ghost fence created.");
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
            //CreatePrecisionObstacle();
        }
    }

    void UpdatePosition(GameObject fence)
    {
        //Debug.Log("start:" + startPosition);
        //Debug.Log("end:" + endPosition);
        fence.transform.position = startPosition;
        fence.transform.LookAt(endPosition);
        fence.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, Vector3.Distance(startPosition, endPosition));
    }

    void HideFence(GameObject fence)
    {
        fence.transform.position = new Vector3(100, 100, 100);
    }
}
