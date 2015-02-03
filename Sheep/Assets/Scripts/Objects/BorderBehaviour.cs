using UnityEngine;
using System.Collections;

public class BorderBehaviour : MonoBehaviour {

	public LevelController _LevelController;

	void OnTriggerEnter(Collider collision)
	{
		if(collision.gameObject.tag == "Sheep")
		{
			collision.gameObject.GetComponent<SheepAI>().SetDead();
			collision.gameObject.SetActive(false);
			_LevelController.AnimalDied(Animals.Sheep);
		}
	}
}
