using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {
	

	void OnCollisionEnter(Collision col){
		if (col.gameObject.CompareTag("Sheep") || col.gameObject.CompareTag("Wolf")){
			Destroy(col.gameObject);
			Destroy(this.gameObject);
		}
	}
}
