using UnityEngine;
using System.Collections;

public class LagoonBehaviour : MonoBehaviour {

	public LevelController _LevelController;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Sheep")
		{
			collision.gameObject.GetComponent<SheepAI>().SetDead();
			collision.gameObject.SetActive(false);
			_LevelController.AnimalDied(Animals.Sheep);
		}
	}
}
