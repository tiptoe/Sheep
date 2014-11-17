using UnityEngine;
using System.Collections;

public class ChangeFenceColor : MonoBehaviour
{
    public float minLength = 1;
    public float maxLength = 8;
    public Color red = Color.red;
    public Color green = Color.green;

    public bool IsRed { get; private set; }
    int collisions = 0;
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
        if (isGhostFence) 
        {
            float length = transform.parent.localScale.z;
            if (length < minLength || length > maxLength || collisions != 0)
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
            collisions++;
            Debug.Log("collisions: " + collisions);
            SetRed();
        }
            
    }

    void OnTriggerExit(Collider other) 
    {
        //if (transform.collider.isTrigger)
		if (isGhostFence)
        {
            collisions--;
            Debug.Log("collisions: " + collisions);
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
