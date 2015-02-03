using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonPause : MonoBehaviour, IPointerClickHandler
{
    GUIController guiController;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GUIController");

        if (obj)
            guiController = obj.GetComponent<GUIController>();
        else
            Debug.LogError("ButtonPause: GUIController wasn't found.");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (guiController != null)
            guiController.ChangeToPause();
    }
}
