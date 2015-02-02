using UnityEngine;
using System.Collections;

public class TimeHorizon : MonoBehaviour
{

    private Animation sun;
    private Animation sunImage;

    public void Awake()
    {
        var animationComponents = gameObject.GetComponentsInChildren<Animation>();
        sun = animationComponents[0];
        sunImage = animationComponents[1];

        sun.animation["SunMovement"].speed = 0;
        sun.Play();

        sunImage.animation["SunScale"].speed = 0;
        sun.Play();
    }
    public void ChangeState(int percent)
    {
        sun.animation["SunMovement"].time = percent / 60f;
        sunImage.animation["SunScale"].time = percent / 60f;
    }
}
