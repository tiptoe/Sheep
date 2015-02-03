using UnityEngine;
using System.Collections;

public class MeatPrefab : MonoBehaviour, ITriggerZoneOwner
{

    void Update()
    {
    }

    public void OnTriggerZoneEnter(Collider other, TriggerZone triggerZone)
    {
        // outer trigger zone
        if (triggerZone.id == 1)
        {
        }

        // inner trigger zone
        else if (triggerZone.id == 0)
        {
        }
    }

    public void OnTriggerZoneExit(Collider other, TriggerZone triggerZone)
    {
    }
}
