using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public GameController _GameController;

	public RectTransform Start;
	public RectTransform Play;
	public RectTransform Options;
	public RectTransform About;

	public void ChangeToStart(RectTransform activePage)
	{
		activePage.gameObject.SetActive(false);
		Start.gameObject.SetActive(true);
	}

	public void ChangeToPlay(RectTransform activePage)
	{
		_GameController.LoadLevel("VerticalSlice");
		//activePage.gameObject.SetActive(false);
		//Play.gameObject.SetActive(true);
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
