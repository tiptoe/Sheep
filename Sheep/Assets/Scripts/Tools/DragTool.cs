using UnityEngine;
using System.Collections;

public abstract class DragTool : Tool {
    protected override void Listening(bool value)
    {
        if (value)
        {
            ActiveAreaEventManager.OnBeginDragging += OnBeginDrag;
            ActiveAreaEventManager.OnDragging += OnDrag;
            ActiveAreaEventManager.OnEndDragging += OnEndDrag;
        }
            
        else
        {
            ActiveAreaEventManager.OnBeginDragging -= OnBeginDrag;
            ActiveAreaEventManager.OnDragging -= OnDrag;
            ActiveAreaEventManager.OnEndDragging -= OnEndDrag;
        }
    }
    /*
    void InstantiateTool(Vector3 position)
    {
        if ((amount > 0 || infinityAmount) && toolPrefab != null)
        {
            Instantiate(toolPrefab, position, Quaternion.identity);
            amount--;
            UpdateAmount();
            //levelController.AddScore(score);
        }
    }*/

    protected abstract void OnBeginDrag(Vector3 position);
    protected abstract void OnDrag(Vector3 position);
    protected abstract void OnEndDrag(Vector3 position);
}
