using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class LevelListItemInfo
{
    public bool open = false;
    public string levelNum = "";
    public string levelName = "";
    public int protectorsNum = 0;
}

public class LevelList : MonoBehaviour
{
    public List<LevelListItemInfo> levelsInfo;
    public LevelListItemController levelListItemPrefab;

	public GameController _GameController;

	void Awake()
	{
		_GameController = FindGameController();
	}

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (levelsInfo == null)
        {
            Debug.LogError("Levels info missing.");
            return;
        }

		int i = 1;
		foreach (var level in _GameController.gameProgress.Levels)
		{
			LevelListItemInfo levelItem = new LevelListItemInfo();
			levelItem.open = level.Enabled;
			levelItem.protectorsNum = level.Stars;
			levelItem.levelNum = i.ToString();
			levelItem.levelName = "Level " + i.ToString();
			levelsInfo.Add(levelItem);
			++i;
		}

        foreach (var levelInfo in levelsInfo)
        {
            LevelListItemController levelItem = GameObject.Instantiate(levelListItemPrefab) as LevelListItemController;
            levelItem.open = levelInfo.open;
            levelItem.levelNum = levelInfo.levelNum;
            levelItem.levelName = levelInfo.levelName;
            levelItem.protectorsNum = levelInfo.protectorsNum;
            levelItem.transform.SetParent(transform);
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
}
