using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelListItem : MonoBehaviour, IPointerClickHandler
{
    public string levelNum = "";
    public string levelName = "";
    public int hatsNum = 0;
    public bool showProtectors = true;

    public Sprite protectorFull;

    public Image protector1;
    public Image protector2;
    public Image protector3;

    void Start()
    {
        Text label = GetComponentInChildren<Text>();
        if (label)
            label.text = levelNum;

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

        if (showProtectors && hatsNum > 0)
        {
            protector1.transform.parent.gameObject.SetActive(true);
            protector1.sprite = protectorFull;

            if (hatsNum > 1)
                protector2.sprite = protectorFull;
            if (hatsNum > 2)
                protector3.sprite = protectorFull;
        }
        else
        {
            protector1.transform.parent.gameObject.SetActive(false);
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
