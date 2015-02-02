using UnityEngine;
using System.Collections;

public class SunflowerEventManager : MonoBehaviour {

    public delegate void TriggerAction(GameObject animal);

    public static event TriggerAction OuterTrigger;
    public static event TriggerAction InnerTrigger;


    public float eatingTime = 2f;
    bool eating = false;

    public void OnOuterTriggerEnter(Collider other)
    {
        /*Debug.Log("outer trigger");
        if (OuterTrigger != null)
            OuterTrigger(other.gameObject);*/

        if (!other.transform.parent || !other.transform.parent.CompareTag("Sheep"))
            return;
        SheepAI sheep = other.gameObject.GetComponentInParent<SheepAI>();
        sheep.SetTarget(transform.position, 1);
        sheep.ChangeMood(AIStates.Interested);  
    }

    public void OnInnerTriggerEnter(Collider other)
    {
        /*Debug.Log("inner trigger");
        if (InnerTrigger != null)
            InnerTrigger(other.gameObject);*/

        if (!other.transform.parent.CompareTag("Sheep") || eating)
            return;
        SheepAI sheep = other.gameObject.GetComponentInParent<SheepAI>();
        sheep.SetTarget(transform.position, 1);
        sheep.ChangeMood(AIStates.Eat);
        eating = true;
    }

    void Update()
    {
        if (eating)
        {
            eatingTime -= Time.deltaTime;

            if (eatingTime < 0)
                Destroy(transform.gameObject);
        }

    }
}
