using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

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
		Application.LoadLevel("VerticalSlice");
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
}
