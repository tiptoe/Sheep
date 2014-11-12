using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ActiveAreaEventManager : MonoBehaviour, IPointerClickHandler {

    public delegate void TouchAction(Vector3 position);
    public static event TouchAction OnTouched;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnTouched != null)
            OnTouched(eventData.worldPosition);
    }
}
