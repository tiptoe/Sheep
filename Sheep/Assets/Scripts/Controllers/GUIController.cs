using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

	public LevelController LevelController;

	public GameObject TimeLeft;
	public GameObject Score;

	// Use this for initialization
	void Start () {
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)LevelController.Length;
		Score.GetComponent<Text>().text = "SCORE: " + LevelController.Score;
	}
	
	// Update is called once per frame
	void Update () {
		TimeLeft.GetComponent<Text>().text = "TIME LEFT: " + (int)LevelController.Length;
		Score.GetComponent<Text>().text = "SCORE: " + LevelController.Score;
	}
}
