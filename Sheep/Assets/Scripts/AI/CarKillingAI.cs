using UnityEngine;
using System.Collections;

public class CarKillingAI : MonoBehaviour {
	
	private LevelController _LevelController;

	void Start(){
		_LevelController = FindLevelController();
	}

	void OnTriggerEnter(Collider collision){
		if(collision.gameObject.tag == "Sheep")
		{
			collision.gameObject.GetComponent<SheepAI>().SetDead();
			collision.gameObject.SetActive(false);
			_LevelController.AnimalDied(Animals.Sheep);
		}else if(collision.gameObject.tag == "Wolf")
		{
			collision.gameObject.GetComponent<WolfAI>().SetDead();
			collision.gameObject.SetActive(false);
			_LevelController.AnimalDied(Animals.Wolf);
		}
	}

	public LevelController FindLevelController()
	{
		GameObject levelControllerObject = GameObject.Find("LevelController");
		if(levelControllerObject != null)
		{
			return levelControllerObject.GetComponent<LevelController>();
		}else{
			return null;
		}
	}
}
