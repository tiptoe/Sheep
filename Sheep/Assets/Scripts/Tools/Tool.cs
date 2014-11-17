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

    Toggle toggle;
    Text amountText;
    protected LevelController levelController;

    protected abstract void Listening(bool value);

    void Start()
    {
        toggle = GetComponent<Toggle>();
        amountText = GetComponentInChildren<Text>();
        levelController = GameObject.Find("LevelController").GetComponent<LevelController>();

        if (toggle == null)
            throw new System.NullReferenceException("toggle component missing");

        UpdateAmount();
        OnValueChanged();
    }

    protected void UpdateAmount()
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

    public void ActivateToggle()
    {
        toggle.interactable = true;
        if (toggle.isOn)
            Listening(true);
        UpdateAmount();  
    } 

    public void DeactivateToggle()
    {
        toggle.interactable = false;
        Listening(false);
    } 
}
