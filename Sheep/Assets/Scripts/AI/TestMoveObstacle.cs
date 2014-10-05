using UnityEngine;
using System.Collections;

public class TestMoveObstacle : MonoBehaviour {


    int counter = 90;
    Vector3 direction = new Vector3(1, 0, 0);
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        counter--;
        if (counter == 0)
        {
            direction = direction * -1;
            counter = 90;
        }
        this.transform.Translate(direction * Time.deltaTime);
	}
}
