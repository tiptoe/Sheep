using UnityEngine;
using System.Collections;

public class ChangeFenceColor : MonoBehaviour
{
    public float minLength = 1;
    public float maxLength = 8;
    public Color red = Color.red;
    public Color green = Color.green;

    public bool IsRed { get; private set; }
    bool inCollision = false;
    bool isGhostFence = false;

    public void SetGhostFence()
    {
		SetGreen();
        isGhostFence = true;
        transform.collider.isTrigger = true;
        transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = false;
    }

    void Update()
    {
        if (isGhostFence && !inCollision) 
        {
            float length = transform.parent.localScale.z;
            if (length < minLength || length > maxLength)
                SetRed();
            else
                SetGreen();
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        // if it is a ghost fence
        if (isGhostFence && !other.CompareTag("Background")
		    			 && !other.CompareTag("Interest"))
        {
            inCollision = true;
            SetRed();
        }
            
    }

    void OnTriggerExit(Collider other) 
    {
        //if (transform.collider.isTrigger)
		if (isGhostFence)
        {
            inCollision = false;
            SetGreen();
        }
            
    }

    void SetGreen()
    {
        transform.renderer.material.color = green;
        IsRed = false;
        
    }

    void SetRed()
    {
        transform.renderer.material.color = red;
        IsRed = true;
    }


}
