using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class Tool : MonoBehaviour
{
    public GameObject toolPrefab;
    public int amount;
    public bool infinityAmount = false;
    public int score;
    Color deactivatedColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);

    Toggle toggle;
    Text amountText;
    GameObject outerRing;
    Image image;
    protected LevelController levelController;

    protected abstract void Listening(bool value);

    void Start()
    {
        toggle = GetComponent<Toggle>();
        amountText = GetComponentInChildren<Text>();

        // find GUI objects OuterRing and Image
        var imageObjects = GetComponentsInChildren<Image>();
        foreach (var imageObject in imageObjects)
        {
            if (imageObject.gameObject.name == "OuterRing")
            {
                outerRing = imageObject.gameObject;
                continue;
            }
            if (imageObject.gameObject.name == "Image")
            {
                image = imageObject;
            }
        }
        if (outerRing == null)
            Debug.LogError("GUI(" + toolPrefab.name + "): OuterRing has not been found.");
        if (image == null)
            Debug.LogError("GUI(" + toolPrefab.name + "): Tool image has not been found.");

        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        if (toggle == null)
            throw new System.NullReferenceException("toggle component missing");

        UpdateAmount();
        OnValueChanged();
    }

    protected void UpdateAmount()
    {
        if (infinityAmount)
        {
            amountText.text = "8";
            // fix rotation and position
            amountText.transform.Rotate(new Vector3(0, 0, 90f));
            amountText.rectTransform.anchoredPosition += new Vector2(0, -2f);
        }
        else
            amountText.text = amount.ToString();

        if (amount <= 0 && !infinityAmount)
            DeactivateToggle();
    }

    public void OnValueChanged()
    {
        Listening(toggle.isOn);

        if (outerRing != null)
            outerRing.SetActive(!toggle.isOn);
    }

    void OnDisable()
    {
        Listening(false);
    }

    public void ActivateToggle()
    {
        toggle.interactable = true;
        if (toggle.isOn)
            Listening(true);
        UpdateAmount();
        image.color = Color.white;
    }

    public void DeactivateToggle()
    {
        toggle.isOn = false;
        toggle.interactable = false;
        Listening(false);
        image.color = deactivatedColor;
    }
}
