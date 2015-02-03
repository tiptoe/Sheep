using UnityEngine;
using System.Collections;

public interface ITriggerZoneOwner
{
    void OnTriggerZoneEnter(Collider other, TriggerZone triggerZone);
    void OnTriggerZoneExit(Collider other, TriggerZone triggerZone);
}

public class TriggerZone : MonoBehaviour
{

    public int id = 0;

    private ITriggerZoneOwner triggerZoneOwner;

    void Awake()
    {
        Collider collider = GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("Invalid TriggerZone. Some collider must exist on gameObject");
        }

        triggerZoneOwner = GetFirstTriggerZoneOwnerUpward(gameObject);
        if (triggerZoneOwner == null)
        {
            Debug.LogError("Invalid TriggerZone. Trigger Zone Owner must implement ITriggerZoneOwner interface.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggerZoneOwner != null)
            triggerZoneOwner.OnTriggerZoneEnter(other, this);
    }

    void OnTriggerExit(Collider other)
    {
        if (triggerZoneOwner != null)
            triggerZoneOwner.OnTriggerZoneExit(other, this);
    }

    private ITriggerZoneOwner GetFirstTriggerZoneOwnerUpward(GameObject obj)
    {

        foreach (Component comp in obj.GetComponents<Component>())
        {
            ITriggerZoneOwner t = comp as ITriggerZoneOwner;
            if (t != null)
                return t;
        }

        if (obj.transform.parent != null && obj.transform.parent.gameObject != null)
            return GetFirstTriggerZoneOwnerUpward(obj.transform.parent.gameObject);

        return null;
    }
}
