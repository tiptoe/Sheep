using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameController : MonoBehaviour {

	public bool IsAudio = true;
	[Range(0,100)]
	public int MusicVolume = 75;
	[Range(0,100)]
	public int SoundsVolume = 75;

	[Range(0,100)]
	private int levels = 15;

	private const string SAVEFILENAME = "/progressInfo.dat";

	private GameProgress gameProgress;

	//prevent doubling
	static GameController instance;

	void Awake()
	{
		//GameObject otherGameController = GameObject.Find("GameController");

		if(instance)
		{
			Destroy(gameObject);
		}else
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
	}

	void Start()
	{
		LoadProgress();
	}

	public void LoadLevel(string levelName)
	{
		Application.LoadLevel(levelName);
	}

    public void NextLevel()
    {
        throw new System.NotImplementedException();
    }

    public void LoadMenu()
    {
        throw new System.NotImplementedException();
    }

	public void RestartLevel()
	{
		throw new System.NotImplementedException();	
	}
    
	public void SaveProgress(int levelId, int score, int stars)
    {
		if(levelId > 0 && (stars >= 0 && stars <= 3))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(Application.persistentDataPath + SAVEFILENAME);

			if(score > gameProgress.Levels[levelId-1].Score)
				gameProgress.Levels[levelId-1].Score = score;

			if(stars > gameProgress.Levels[levelId-1].Stars)
				gameProgress.Levels[levelId-1].Stars = stars;

			if(stars > 0 && levelId < levels)
				gameProgress.Levels[levelId].Enabled = true;

			bf.Serialize(file, gameProgress);
			file.Close();
		}
    }

	public void LoadProgress()
	{
		if(File.Exists(Application.persistentDataPath + SAVEFILENAME))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + SAVEFILENAME, FileMode.Open);

			gameProgress = (GameProgress)bf.Deserialize(file);
			file.Close();
		}else
		{
			ResetProgress();
		}
	}

	public void ResetProgress()
	{
		gameProgress = new GameProgress();

		for(int i = 0; i < levels; ++i)
		{
			gameProgress.Levels.Add(new LevelProgress());
		}
		gameProgress.Levels[0].Enabled = true;

		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + SAVEFILENAME);

		bf.Serialize(file, gameProgress);	
		file.Close();

	}
}

[Serializable]
class GameProgress {
	public List<LevelProgress> Levels;

	public GameProgress()
	{
		Levels = new List<LevelProgress>();
	}
}

[Serializable]
class LevelProgress {	
	public int Score;
	public int Stars;
	public bool Enabled;

	public LevelProgress()
	{
		Score = 0;
		Stars = 0;
		Enabled = false;
	}
}