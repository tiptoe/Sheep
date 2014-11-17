using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class SingleTouchTool : Tool {

    protected override void Listening(bool value)
    {
        if (value)
            ActiveAreaEventManager.OnTouched += InstantiateTool;
        else
            ActiveAreaEventManager.OnTouched -= InstantiateTool;
    }

    void InstantiateTool(Vector3 position)
    {
        if ((amount > 0 || infinityAmount) && toolPrefab != null)
        {
            Instantiate(toolPrefab, position, Quaternion.identity);
            amount--;
            UpdateAmount();
            //levelController.AddScore(score);
        }
    }
}
