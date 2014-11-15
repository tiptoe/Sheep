using UnityEngine;
using System.Collections;

public class TestAI : MonoBehaviour {

     NavMeshAgent agent;
     public GameObject dest;
     private GameObject instance;

     int counter = 60;
	// Use this for initialization
	void Start () {
	 agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        counter--;
        if (counter == 0)
        {
            
            counter = 150;
            if (instance != null)
            {
                Destroy(instance);
            }
            Vector3 destination =new Vector3( Random.Range(0,10)-5,0,Random.Range(0,10)-5);
            instance = (GameObject)Instantiate(dest, destination, new Quaternion());
            //Debug.Log(destination);
            //agent.destination = (destination);
            agent.SetDestination(destination);
        }
        //agent.Move
	}
}
