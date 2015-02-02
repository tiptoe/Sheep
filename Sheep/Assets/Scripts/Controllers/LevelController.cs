using UnityEngine;
using System.Collections;

public class LevelController : MonoBehaviour {
	public int Id = 0;
	public float Length = 60;
	public float SpawnWolvesAt = 30;
    public int intervalPoints = 10;
    public float intervalLength = 5f;
    
	public GameObject[] Sheeps;
	public GameObject[] Wolves;
	private int aliveSheeps = 0;
	private int aliveWolves = 0;
	public int Score = 0;

	public GUIController _GUIController;
	public GameController _GameController;

    private float lastIntervalTime;
    private float lengthMax;
    private int timePercentage;

	void Awake()
	{
		_GameController = FindGameController();
        lengthMax = Length;
	}

	void Start()
	{
		aliveSheeps = Sheeps.Length;
		aliveWolves = Wolves.Length;
		Time.timeScale = 1;
        lastIntervalTime = Length;
        timePercentage = 0;
	}

	void Update()
	{
		if(Length <= 0 || aliveSheeps == 0)
		{
			Time.timeScale = 0;
			EndRound();
		}

		if(Length <= SpawnWolvesAt)
		{
			SpawnWolves();
			SpawnWolvesAt = -1;
		}

		Length -= Time.deltaTime;

        // calculate percantage of elapsed time and send it to gui
        int tempPercentage = 100 - (int) ((Length / lengthMax) * 100);
        if (timePercentage != tempPercentage)
        {
            timePercentage = tempPercentage;
            _GUIController.timeHorizon.ChangeState(timePercentage);
        }
        

        // add interval points
        if (lastIntervalTime - Length > intervalLength)
        {
            AddScore(intervalPoints);
            lastIntervalTime = Length;
        }
	}

    public void AddScore(int value)
    {
		Score += value;
        _GUIController.SetScore(Score);
    }

    public void AnimalDied(Animals animal)
    {
        switch(animal)
		{
			case Animals.Wolf:
			{
				--aliveWolves;
				break;
			}
			case Animals.Sheep:
			{
				--aliveSheeps;
				break;
			}
		}
    }

	public GameController FindGameController()
	{
		GameObject gameControllerObject = GameObject.Find("GameController");
		if(gameControllerObject != null)
		{
			return gameControllerObject.GetComponent<GameController>();
		}else
		{
			gameControllerObject = new GameObject();
			gameControllerObject.name = "GameController";
			gameControllerObject.AddComponent<GameController>();
			//Instantiate(gameControllerObject);

			return gameControllerObject.GetComponent<GameController>();
		}
	}

	private void EndRound()
	{
		_GUIController.EndRound(aliveSheeps);
	}

	private void SpawnWolves()
	{
		foreach (var wolf in Wolves)
		{
			wolf.SetActive(true);
		}
	}
}
