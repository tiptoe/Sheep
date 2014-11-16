using UnityEngine;
using System.Collections;

public class SoundsBehaviour : MonoBehaviour {

	public LevelController _LevelController;
	private GameController _GameController;
	
	void Start()
	{
		_GameController = _LevelController.FindGameController();
	}

	void Update ()
	{
		if(_GameController.IsAudio)
		{
			audio.volume = _GameController.SoundsVolume/100.0f;
		}else
		{
			audio.volume = 0.0f;
		}
	}
}
