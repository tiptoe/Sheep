using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameController _GameController;

	public RectTransform StartPanel;
	public RectTransform Play;
	public RectTransform Options;
	public RectTransform About;

	public Toggle AudioToggle;
	public Slider AudioBackgroundSlider;
	public Slider AudioSoundsSlider;

	void Start()
	{
		_GameController = GameObject.Find("GameController").GetComponent<GameController>();
		AudioToggle.isOn = _GameController.IsAudio;
		AudioBackgroundSlider.value = _GameController.MusicVolume;
		AudioSoundsSlider.value = _GameController.SoundsVolume;
	}

	public void ChangeToStart(RectTransform activePage)
	{
		activePage.gameObject.SetActive(false);
		StartPanel.gameObject.SetActive(true);
	}

	public void ChangeToPlay(RectTransform activePage)
	{
		_GameController.LoadLevel("prototype2");
		//activePage.gameObject.SetActive(false);
		//Play.gameObject.SetActive(true);
	}

    public void LoadLevel(string levelName)
    {
        _GameController.LoadLevel(levelName);
    }

	public void ChangeToOptions(RectTransform activePage)
	{
		activePage.gameObject.SetActive(false);
		Options.gameObject.SetActive(true);
	}

	public void ChangeToAbout(RectTransform activePage)
	{
		activePage.gameObject.SetActive(false);
		About.gameObject.SetActive(true);
	}

	public void SetAudio(Toggle toggle)
	{
		_GameController.IsAudio = toggle.isOn;
	}
	
	public void SetMusicVolume(Slider volumeSlider)
	{
		_GameController.MusicVolume = (int)volumeSlider.value;
	}
	
	public void SetSoundsVolume(Slider volumeSlider)
	{
		_GameController.SoundsVolume = (int)volumeSlider.value;
	}
}
