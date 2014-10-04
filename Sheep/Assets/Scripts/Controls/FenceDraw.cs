using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FenceDraw : MonoBehaviour
{
    //bude nahrazeno prefabem plotu
    public GameObject fencePrefab;
    /// <summary>
    /// pamatuje si poslední vytvořený plot, už kvuli tomu že ho bude mazat
    /// </summary>
    List<GameObject> fence = new List<GameObject>();

    //kde začíná plot
    private Vector3 startDrawPosition;
    private bool draw = false;

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
    #if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (!draw)
            {
                Debug.Log("start");
                foreach (GameObject gO in fence)
                {
                    Destroy(gO);
                }
                RaycastHit data;
                Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(vRay, out data, 1000))
                {
                    Debug.Log("data recieve");
                    draw = true;
                    startDrawPosition = new Vector3(data.point.x,0, data.point.z);
                }
            }
        }
        else
        {
            if (draw)
            {
                Debug.Log("end");
                draw = false;
                RaycastHit data;
                Ray vRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                //podmínka prakticky zbytečná protože nepujde minout plochu
                if (Physics.Raycast(vRay, out data, 1000))
                {
                    Debug.Log("placing");
                    Vector3 endDrawPosition = new Vector3(data.point.x,0, data.point.z);
                    float distance = Vector3.Distance(startDrawPosition, endDrawPosition);
                    // pak místo toho se zde bude počítat uhel, kontrolovat kolize a řešit rozměry plotu
                    for (int i = 0; i < 10; i++)
                    {
                      fence.Add((GameObject)Instantiate(fencePrefab, startDrawPosition, new Quaternion()));
                       startDrawPosition = Vector3.MoveTowards(startDrawPosition, endDrawPosition, distance / 10.0f);
                    }
                }
            }

        }
    #endif
	}
}
