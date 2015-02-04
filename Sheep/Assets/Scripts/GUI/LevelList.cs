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
        foreach (var levelInfo in levelsInfo)
        {
            LevelListItemController levelItem = GameObject.Instantiate(levelListItemPrefab) as LevelListItemController;
            levelItem.open = levelInfo.open;
            levelItem.levelNum = levelInfo.levelNum;
            levelItem.levelName = levelInfo.levelName;
            levelItem.protectorsNum = levelInfo.protectorsNum;
            levelItem.transform.parent = transform;
        }
    }
}
