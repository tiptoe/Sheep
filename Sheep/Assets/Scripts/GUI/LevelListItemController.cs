using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelListItemController : MonoBehaviour, IPointerClickHandler
{
    public bool open = false;
    public string levelNum = "";
    public string levelName = "";
    public int protectorsNum = 0;
    public bool showProtectors = true;

    public Sprite protectorFull;
    public Sprite inactiveButton;

    public Image protector1;
    public Image protector2;
    public Image protector3;

    private MainMenuController mainMenuController;

    void Awake()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("MainMenuController");
        if (obj)
            mainMenuController = obj.GetComponent<MainMenuController>();
    }

    void Start()
    {
        SetItem();
    }

    public void SetItem() 
    {
        Text label = GetComponentInChildren<Text>();
        if (label)
            label.text = levelNum;

        if (!open)
        {
            Image objectImage = GetComponent<Image>();
            if (objectImage)
                objectImage.sprite = inactiveButton;
        }


        if (!protectorFull)
        {
            Debug.LogError("LevelItem: ProtectorSmallFull sprite missing.");
            return;
        }

        if (!protector1 || !protector2 || !protector3)
        {
            Debug.LogError("LevelItem: Protector prefab images missing.");
            return;
        }

        if (showProtectors && protectorsNum > 0)
        {
            protector1.transform.parent.gameObject.SetActive(true);
            protector1.sprite = protectorFull;

            if (protectorsNum > 1)
                protector2.sprite = protectorFull;
            if (protectorsNum > 2)
                protector3.sprite = protectorFull;
        }
        else
        {
            protector1.transform.parent.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!mainMenuController)
        {
            Debug.LogError("LevelItem: MainMenuController reference missing.");
            return;
        }

        if (open)
            mainMenuController.LoadLevel(levelName);
    }
}
