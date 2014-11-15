using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {


	public int Id = 0;
	public float Length = 120;
	public int SheepCount;
	public int WolfCount;
	public int Score = 0;

	private int aliveSheeps;
	private int aliveWolves;

	void Start()
	{
		aliveSheeps = SheepCount;
		aliveWolves = WolfCount;
	}

	void Update()
	{
		Length -= Time.deltaTime;
		Debug.Log((int)Length);

		if(Length <= 0)
		{
			Debug.Log("End!");
		}
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

	private void EndRound()
	{

	}
}
