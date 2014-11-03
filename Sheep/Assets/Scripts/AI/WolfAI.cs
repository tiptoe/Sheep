using UnityEngine;
using System.Collections;

public class WolfAI : MonoBehaviour, IAnimalAI {
    public AIStates State { get; set; }

    public void Spawn()
    {
        throw new System.NotImplementedException();
    }

    public void SetTarget(Transform target, int priority)
    {
        throw new System.NotImplementedException();
    }
}
