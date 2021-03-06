﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GUIController : MonoBehaviour
{
    // GUI prefabs
    public Text scorePrefab;
    public ScoreChange scoreChangePrefab;
    public TimeHorizon timeHorizonPrefab;
    public GameObject buttonPausePrefab;

    // GUI instances
    [HideInInspector]
    public TimeHorizon timeHorizon;
    private Text score;
    private GameObject buttonPause;
    private ScoreChange scoreChange;

    public GameObject PauseMenu;
    public GameObject EndRoundMenu;
    public GameObject blockingPane;

    public Sprite rewardON;
    public Sprite rewardOFF;
    public Image reward1;
    public Image reward2;
    public Image reward3;

    public Toggle AudioToggle;
    public Slider AudioBackgroundSlider;
    public Slider AudioSoundsSlider;

    private LevelController _LevelController;
	public GameController _GameController;

    private int scoreValue;
    private List<GameObject> inGameGUI;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("LevelController");
        if (obj)
		{
            _LevelController = obj.GetComponent<LevelController>();
			_GameController = _LevelController.FindGameController();
		}else
		{
			Debug.LogError("GUIController: LevelController wasn't found.");
		}

        InitializeGUI();
    }

    void Start()
    {
        scoreValue = 0;
        //ShowInGameGUI(true);
        AddAudioButtonsListeners();
    }

    public void EndRound(int rewards, int score)
    {
        reward1.sprite = rewardOFF;
        reward2.sprite = rewardOFF;
        reward3.sprite = rewardOFF;

        if (rewards > 0)
            reward1.sprite = rewardON;
        if (rewards > 1)
            reward2.sprite = rewardON;
        if (rewards > 2)
            reward3.sprite = rewardON;

        EndRoundMenu.SetActive(true);
        if (blockingPane)
            blockingPane.SetActive(true);

        Text scoreText = EndRoundMenu.GetComponentInChildren<Text>();
        if (scoreText)
            scoreText.text = "Score: " + score;
    }

    public void ChangeToPause()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        if (blockingPane)
            blockingPane.SetActive(true);
        //ShowInGameGUI(false);
    }

    public void ChangeToContinue()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        if (blockingPane)
            blockingPane.SetActive(false);
        //ShowInGameGUI(true);
    }

    public void ChangeToMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void ChangeToRetry()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

	public void ChangeToNext()
	{
		if(_LevelController.IsLevelDone || _GameController.gameProgress.Levels[_LevelController.Id].Enabled)
			Application.LoadLevel("Level " + (_LevelController.Id + 1).ToString());
	}

    public void SetScore(int newScoreValue)
    {
        int change = newScoreValue - scoreValue;
        scoreValue = newScoreValue;

        score.text = "" + scoreValue;

        ScoreChange scoreChange = Instantiate(scoreChangePrefab) as ScoreChange;
        scoreChange.transform.SetParent(score.transform);
        scoreChange.value = change;
    }

    private void AddAudioButtonsListeners()
    {
        //AudioToggle.isOn = _LevelController._GameController.IsAudio;
        //AudioBackgroundSlider.value = _LevelController._GameController.MusicVolume;
        //AudioSoundsSlider.value = _LevelController._GameController.SoundsVolume;
        //AudioToggle.onValueChanged.AddListener((x) => { _LevelController._GameController.IsAudio = AudioToggle.isOn; });
        //AudioBackgroundSlider.onValueChanged.AddListener((x) => { _LevelController._GameController.MusicVolume = (int)AudioBackgroundSlider.value; });
        //AudioSoundsSlider.onValueChanged.AddListener((x) => { _LevelController._GameController.SoundsVolume = (int)AudioSoundsSlider.value; });
    }

    private void InitializeGUI()
    {
        inGameGUI = new List<GameObject>();

        score = GameObject.Instantiate(scorePrefab) as Text;
        scoreChange = GameObject.Instantiate(scoreChangePrefab) as ScoreChange;
        timeHorizon = GameObject.Instantiate(timeHorizonPrefab) as TimeHorizon;
        buttonPause = GameObject.Instantiate(buttonPausePrefab) as GameObject;

        inGameGUI.Add(score.gameObject);
        inGameGUI.Add(scoreChange.gameObject);
        inGameGUI.Add(timeHorizon.gameObject);
        inGameGUI.Add(buttonPause.gameObject);

        GameObject canvas = GameObject.Find("Canvas");
        RectTransform itemTransform;
        foreach (var item in inGameGUI)
        {
            //item.SetActive(false);
            item.transform.SetParent(canvas.transform);
            item.transform.SetAsFirstSibling();
            itemTransform = item.transform as RectTransform;
            itemTransform.anchoredPosition = Vector2.zero;
        }

        GameObject toolsController = GameObject.FindGameObjectWithTag("ToolsController");
        if (toolsController)
        {
            inGameGUI.Add(toolsController);
            //toolsController.SetActive(false);
        }
        else
        {
            Debug.LogError("GUIController: ToolsController wasn't found.");
        }
    }

    private void ShowInGameGUI(bool value)
    {

        if (inGameGUI == null)
            InitializeGUI();

        foreach (var item in inGameGUI)
        {
            if (item)
                item.SetActive(value);
        }
    }
}
