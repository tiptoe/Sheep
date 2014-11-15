using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonRetryBehaviour : MonoBehaviour {

	void Start()
	{
		Button button = GetComponent<Button>();
		button.onClick.AddListener(() => Application.LoadLevel(Application.loadedLevel));
	}
}
