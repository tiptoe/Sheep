using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public bool IsAudio = true;
	[Range(0,100)]
	public int MusicVolume = 75;
	[Range(0,100)]
	public int SoundsVolume = 75;

	void Awake()
	{
		DontDestroyOnLoad(this);
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

    public void SaveScore(int score)
    {
        throw new System.NotImplementedException();
    }
    public void RestartLevel()
    {
        throw new System.NotImplementedException();
    }
}
