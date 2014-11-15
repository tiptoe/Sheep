using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	public LevelController _LevelController;

	public GameObject TimeLeft;
	public GameObject Score;
	public GameObject EndRoundMenu;	

	// Use this for initialization
	void Start ()
	{
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)_LevelController.Length;
		//Score.GetComponent<Text>().text = "SCORE: " + _LevelController.Score;
	}
	
	// Update is called once per frame
	void Update ()
	{
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)_LevelController.Length;
		//Score.GetComponent<Text>().text = "SCORE: " + _LevelController.Score;
	}

	public void EndRound()
	{
		EndRoundMenu.SetActive(true);
	}
}
