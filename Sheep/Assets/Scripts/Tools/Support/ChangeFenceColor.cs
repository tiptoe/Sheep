using UnityEngine;
using System.Collections;

public class ChangeFenceColor : MonoBehaviour
{
    
    public Color red = Color.red;
    public Color green = Color.green;

    float minLength;
    float maxLength;

    public bool IsRed { get; private set; }
    int collisions = 0;
    bool isGhostFence = false;

    //TODO: remake ghost fence
    public SpriteRenderer fencePlank;
    public SpriteRenderer fenceStart;
    public SpriteRenderer fenceEnd;

    public void SetGhostFence(float minLength, float maxLength)
    {
		SetGreen();
        isGhostFence = true;
        transform.collider.isTrigger = true;
        this.minLength = minLength;
        this.maxLength = maxLength;
        //transform.GetChild(0).GetComponent<NavMeshObstacle>().enabled = false;
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
        if (isGhostFence && !other.CompareTag("Background") && !other.CompareTag("Interest"))
        {
            collisions++;
            Debug.Log("collisions: " + collisions);
            SetRed();
        }      
    }

    void OnTriggerExit(Collider other) 
    {
        //if (transform.collider.isTrigger)
        if (isGhostFence && !other.CompareTag("Background") && !other.CompareTag("Interest"))
        {
            collisions--;
            Debug.Log("collisions: " + collisions);
            SetGreen();
        }       
    }

    void SetGreen()
    {
        fenceStart.color = fenceEnd.color = fencePlank.color = green;
        IsRed = false; 
    }

    void SetRed()
    {
        fenceStart.color = fenceEnd.color = fencePlank.color = red;
        IsRed = true;
    }


}
