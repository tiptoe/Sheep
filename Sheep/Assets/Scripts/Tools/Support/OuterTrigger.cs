using UnityEngine;
using System.Collections;

public class OuterTrigger : MonoBehaviour {

    SunflowerEventManager eventManager;

    void Start()
    {
        eventManager = GetComponentInParent<SunflowerEventManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        eventManager.OnOuterTriggerEnter(other);
    }
}
