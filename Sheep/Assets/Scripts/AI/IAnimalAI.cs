using UnityEngine;
using System.Collections;

public interface IAnimalAI {

    AIStates State { get; set; }

    /// <summary>
    /// Set AI target.</summary>
    /// <param name="priority"> If some other target with higher priority is already set, method calling is ignored.</param>
    void SetTarget(Transform target, int priority);
}
