using UnityEngine;
using System.Collections;

public class SheepAI : MonoBehaviour, IAnimalAI {

    public AIStates State { get; set; }
    
    public void SetTarget(Transform target, int priority)
    {
        throw new System.NotImplementedException();
    }
}
