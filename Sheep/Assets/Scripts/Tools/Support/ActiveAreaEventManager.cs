using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ActiveAreaEventManager : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public delegate void TouchAction(Vector3 position);

    public static event TouchAction OnTouched;
    public static event TouchAction OnBeginDragging;
    public static event TouchAction OnDragging;
    public static event TouchAction OnEndDragging;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnTouched != null)
            OnTouched(eventData.worldPosition);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragging != null)
            OnBeginDragging(eventData.worldPosition);

    }

    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragging != null)
            OnDragging(eventData.worldPosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDragging != null)
            OnEndDragging(eventData.worldPosition);
    }
}
