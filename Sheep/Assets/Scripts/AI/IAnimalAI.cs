using UnityEngine;
using System.Collections;

public interface IAnimalAI {

    
    AIStates State { get; set; }
    /// <summary>
    /// Set AI target.</summary>
    /// <param name="priority"> If some other target with higher priority is already set, method calling is ignored.</param>
    void SetTarget(Transform target, int priority);




    /// <summary>
    /// say to ai that fence was build - sheep scared, wolf recalculating pah etc
    /// </summary>
    void FenceBuild();


    /// <summary>
    /// priority is 0-1 chance that target will be set , 1 is 100% 0 is 0%
    /// </summary>
    /// <param name="position"></param>
    /// <param name="priority"></param>
    void SetTarget(Vector3 position, float priority);
    /// <summary>
    /// priority is 0-1 chance that target will be set , 1 is 100% 0 is 0%
    /// </summary>
    /// <param name="position"></param>
    /// <param name="priority"></param>
    void SetTarget(GameObject goal, float priority);


    /// <summary>
    /// say to ai taht some dead occurs in that position, sheep scared
    /// </summary>
    /// <param name="position"></param>
    void DeadOccurs(Vector3 position);


    /// <summary>
    /// set object to dead
    /// </summary>
    void SetDead();


    /// <summary>
    /// try to change state from current co another
    /// </summary>
    void ChangeMood();

    /// <summary>
    /// change state to given state
    /// </summary>
    /// <param name="currentState"></param>
    void ChangeMood(AIStates newState);
}
