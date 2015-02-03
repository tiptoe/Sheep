using UnityEngine;
using System.Collections;

public class SunflowerPrefab : MonoBehaviour, ITriggerZoneOwner 
{
    public float eatingTime = 2f;
    bool eating = false;

    void Update()
    {
        if (eating)
        {
            eatingTime -= Time.deltaTime;

            if (eatingTime < 0)
                Destroy(transform.gameObject);
        }

    }

    public void OnTriggerZoneEnter(Collider other, TriggerZone triggerZone)
    {
        // outer trigger zone
        if (triggerZone.id == 1)
        {
            if (!other.transform.parent || !other.transform.parent.CompareTag("Sheep"))
                return;
            SheepAI sheep = other.gameObject.GetComponentInParent<SheepAI>();
            sheep.SetTarget(transform.position, 1);
            sheep.ChangeMood(AIStates.Interested);  
        }

        // inner trigger zone
        else if (triggerZone.id == 0)
        {
            if (!other.transform.parent.CompareTag("Sheep") || eating)
                return;
            SheepAI sheep = other.gameObject.GetComponentInParent<SheepAI>();
            sheep.SetTarget(transform.position, 1);
            sheep.ChangeMood(AIStates.Eat);
            eating = true;
        }  
    }

    public void OnTriggerZoneExit(Collider other, TriggerZone triggerZone)
    {
    }
}
