using UnityEngine;
using System.Collections;

public class Pond : MonoBehaviour {

	// Use this for initialization
    void OnTriggerEnter(Collider other)
    {
         SheepAI sheep = other.gameObject.GetComponent<SheepAI>();
         if (sheep != null)
         {
             LevelController controller = FindObjectOfType<LevelController>();
             if (controller != null)
             {

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

                 Debug.Log("utopena ovečka");
                 sheep.SetDead();
                 controller.AnimalDied(Animals.Sheep);
                 Destroy(sheep.gameObject);
               
             }
         }
    }
}
