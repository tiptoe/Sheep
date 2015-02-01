using UnityEngine;
using System.Collections;

public class TrapPrefab : MonoBehaviour 
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sheep") || other.gameObject.CompareTag("Wolf"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
