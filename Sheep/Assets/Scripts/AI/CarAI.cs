using UnityEngine;
using System.Collections;

public class CarAI : MonoBehaviour {

	public GameObject carPrefab;
	public float velocity = 10;
	public enum Direction{North, East, South, West};
	public Direction direction;
	public int timeDelay = 10;

	private Vector3 vect = new Vector3();
	private GameObject porsche;

	// Use this for initialization
	void Start () {
	switch (direction) {
		case Direction.North:
				vect = Vector3.forward;
				break;
		case Direction.East:
			vect = Vector3.right;
				break;
		case Direction.South:
			vect = Vector3.back;
				break;
		case Direction.West:
			vect = Vector3.left;
				break;
				}
		InvokeRepeating ("CreatePorsche", timeDelay, timeDelay);

	}
	
	// Update is called once per frame
	void Update () {
		if (porsche != null) {
			porsche.transform.position = porsche.transform.position + vect * velocity * Time.deltaTime;
			if ((Camera.main.WorldToViewportPoint (porsche.transform.position).x > 1.1f) || (Camera.main.WorldToViewportPoint (porsche.transform.position).y > 1.1f) || 
				(Camera.main.WorldToViewportPoint (porsche.transform.position).x < -0.1f) || (Camera.main.WorldToViewportPoint (porsche.transform.position).y < -0.1f)) {
				Destroy (porsche);
			}
		}
	}

	void CreatePorsche(){
		porsche = (GameObject)Instantiate (carPrefab, this.transform.position, Quaternion.identity);
		porsche.transform.Rotate (Vector3.right, 0, Space.World); 
	}
}
