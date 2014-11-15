using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {


	public int Id = 0;
	public float Length = 120;
	public int SheepCount;
	public int WolfCount;
	public int Score = 0;

	public GUIController _GUIController;

	private int aliveSheeps;
	private int aliveWolves;

	void Start()
	{
		Time.timeScale = 1;
		aliveSheeps = SheepCount;
		aliveWolves = WolfCount;
	}

	void Update()
	{
		if(Length <= 0 || aliveSheeps == 0)
		{
			Time.timeScale = 0;
			_GUIController.EndRound();
		}

		Length -= Time.deltaTime;
		Debug.Log((int)Length);
	}

    public void AddScore(int value)
    {
		Score += value;
    }

    public void AnimalDied(Animals animal)
    {
        switch(animal)
		{
			case Animals.Wolf:
				--aliveWolves;
				break;
			case Animals.Sheep:
				--aliveSheeps;
				break;
		}
    }
}
