using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ToolsController : MonoBehaviour {

    Tool[] tools;

    void OnEnable()
    {
        LoadTools();
    }

    void LoadTools()
    {
        tools = transform.GetComponentsInChildren<Tool>();
    }

    public void ActivateTools()
    {
        foreach (Tool tool in tools)
        {
            tool.ActivateToggle();
        }
    }

    public void DeactivateTools()
    {
        foreach (Tool tool in tools)
        {
            tool.DeactivateToggle();
        }
    }
}
