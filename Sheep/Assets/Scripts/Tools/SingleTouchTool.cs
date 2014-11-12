using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public abstract class SingleTouchTool : Tool {

    public GameObject toolPrefab;
    public int amount;
    public bool infinityAmount = false;
    public int score;

    Toggle toggle;
    Text amountText;
    LevelController levelController;

    void Awake()
    {
        toggle = GetComponent<Toggle>();
        amountText = GetComponentInChildren<Text>();
        //levelController = GameObject.FindGameObjectWithTag("LevelController").GetComponent<LevelController>();

        if (toggle == null)
            throw new System.NullReferenceException("toggle");

        UpdateAmount();
        OnValueChanged();
    }

    void Listening(bool value)
    {
        if (value)
            ActiveAreaEventManager.OnTouched += InstantiateTool;
        else
            ActiveAreaEventManager.OnTouched -= InstantiateTool;
    }

    void UpdateAmount()
    {
        if (infinityAmount)
            amountText.text = "∞";
        else
            amountText.text = amount.ToString();

        if (amount <= 0 && !infinityAmount)
            DeactivateToggle();
    }

    public void OnValueChanged()
    {
        Listening(toggle.isOn);
    }

    void OnDisable()
    {
        Listening(false);
    }

    void InstantiateTool(Vector3 position)
    {
        if ((amount > 0 || infinityAmount) && toolPrefab != null)
        {
            Instantiate(toolPrefab, position, Quaternion.identity);
            amount--;
            UpdateAmount();
            //levelController.AddScore(score);
        }      
    }

    void DeactivateToggle()
    {
        toggle.interactable = false;
    }
}
