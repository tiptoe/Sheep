using UnityEngine;
using System.Collections;

public class BorderBehaviour : MonoBehaviour {

	public LevelController _LevelController;

	void OnCollisionEnter(Collision collision)
	{
		if(collision.gameObject.tag == "Sheep")
		{
			Debug.Log("SHEEEEEEEEEEEP");
			collision.gameObject.SetActive(false);
			_LevelController.AnimalDied(Animals.Sheep);
		}
	}
}
