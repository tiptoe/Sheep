using UnityEngine;
using System.Collections;

public class InnerTrigger : MonoBehaviour {

    SunflowerEventManager eventManager;

    void Start()
    {
        eventManager = GetComponentInParent<SunflowerEventManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        eventManager.OnInnerTriggerEnter(other);
    }
}
