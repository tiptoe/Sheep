﻿using UnityEngine;
using System.Collections;

public class TrapPrefab : MonoBehaviour 
{
    public float activateTime = 3.0f;

    // Update is called once per frame
    void Update()
    {
        if (activateTime <= 0)
        {
            if (!collider.enabled)
            {
                collider.enabled = true;
            }
        }
        else
        {
            activateTime = activateTime - Time.deltaTime;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sheep") || other.gameObject.CompareTag("Wolf"))
        {
            LevelController controller = FindObjectOfType<LevelController>();
            if (controller != null)
            {
                controller.AnimalDied(Animals.Sheep);
                foreach (GameObject sheepGO in controller.Sheeps)
                {
                    if (sheepGO == null) { continue; }
                    SheepAI sheepAI = sheepGO.GetComponent<SheepAI>();
                    if (sheepAI != null)
                    {
                        sheepAI.DeadOccurs(other.gameObject.transform.position);
                    }
                }

                foreach (GameObject wolfGO in controller.Wolves)
                {
                    if (wolfGO == null) { continue; }
                    WolfAI wolfAI = wolfGO.GetComponent<WolfAI>();
                    if (wolfAI != null)
                    {
                        wolfAI.DeadOccurs(other.gameObject.transform.position);
                    }
                }
            }
            if (other.gameObject.CompareTag("Sheep"))
            {
                other.gameObject.GetComponent<SheepAI>().SetDead();
            }
            else
            {
                other.gameObject.GetComponent<WolfAI>().SetDead();
            }

            Destroy(other.gameObject);
            Destroy(this.gameObject);

            
        }
    }
}
