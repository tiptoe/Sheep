using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreChange : MonoBehaviour
{
    public int value;
    public Color red = new Color(0.843f, 0.247f, 0.133f, 1f);
    public Color green = new Color(0.294f, 0.529f, 0.164f, 1f);
    public float effectTime = 1f;
    public float positionShift = -35f;

    private Text text;
    private float t = 0;
    private Color mainColor;
    private Vector2 startPosition;

    void Start()
    {
        if (value > 0)
            mainColor = green;
        else if (value < 0)
            mainColor = red;
        else
            Destroy(gameObject);

        text = GetComponent<Text>();
        text.color = mainColor;
        text.text = "" + value;

        text.rectTransform.anchoredPosition = Vector2.zero;
        text.rectTransform.localScale = Vector2.one;
        startPosition = text.rectTransform.anchoredPosition;
    }

    void Update()
    {
        t += Time.deltaTime / effectTime;
        text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(2f, 0f, t));

        text.rectTransform.anchoredPosition = startPosition + new Vector2(0, Mathf.Lerp(0, positionShift, t));
        
        if (t > 1)
            Destroy(gameObject);
    }
}
