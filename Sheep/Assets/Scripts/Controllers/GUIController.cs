using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	public LevelController _LevelController;

	public GameObject TimeLeft;
	public GameObject Score;
	public GameObject PauseMenu;
	public GameObject EndRoundMenu;	

	public Sprite RewardON;
	public Sprite RewardOFF;
	public GameObject Reward1;
	public GameObject Reward2;
	public GameObject Reward3;

	public Toggle AudioToggle;
	public Slider AudioBackgroundSlider;
	public Slider AudioSoundsSlider;

	// Use this for initialization
	void Start ()
	{
		AddAudioButtonsListeners();
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)_LevelController.Length;
		//Score.GetComponent<Text>().text = "SCORE: " + _LevelController.Score;
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)_LevelController.Length;
		//Score.GetComponent<Text>().text = "SCORE: " + _LevelController.Score;
	}

	public void EndRound(int rewards = 3)
	{
		if(rewards > 0)
		{
			Reward1.GetComponent<Image>().sprite = RewardON;
			if(rewards > 1)
			{
				Reward2.GetComponent<Image>().sprite = RewardON;
				if(rewards > 2)
				{
					Reward3.GetComponent<Image>().sprite = RewardON;
				}
			}
		}

		EndRoundMenu.SetActive(true);
	}

	public void ChangeToPause()
	{
		Time.timeScale = 0;
		PauseMenu.SetActive(true);
	}

	public void ChangeToContinue()
	{
		Time.timeScale = 1;
		PauseMenu.SetActive(false);
	}

	public void ChangeToMenu()
	{
		Application.LoadLevel("MainMenu");
	}

	public void ChangeToRetry()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}

	private void AddAudioButtonsListeners()
	{
		AudioToggle.isOn = _LevelController._GameController.IsAudio;
		AudioBackgroundSlider.value = _LevelController._GameController.MusicVolume;
		AudioSoundsSlider.value = _LevelController._GameController.SoundsVolume;
		AudioToggle.onValueChanged.AddListener((x) => { _LevelController._GameController.IsAudio = AudioToggle.isOn; });
		AudioBackgroundSlider.onValueChanged.AddListener((x) => { _LevelController._GameController.MusicVolume = (int)AudioBackgroundSlider.value; });
		AudioSoundsSlider.onValueChanged.AddListener((x) => { _LevelController._GameController.SoundsVolume = (int)AudioSoundsSlider.value; });
	}
}
