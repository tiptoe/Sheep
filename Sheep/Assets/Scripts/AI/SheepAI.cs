﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SheepAI : MonoBehaviour, IAnimalAI
{

    public AIStates sheepState = AIStates.Scared;
    
    /// <summary>
    /// dává info když objekt umře, .. nebo kdyby se potřeboval informovat o něčem
    /// </summary>
    public LevelController controler;
    
    /// <summary>
    /// jak daleko se ovečka dívá při situacích jako, někdo umřel,byl postaven most, atd, jde nastavit pro každou klidně jinak
    /// </summary>
    public float observableArea = 15.0f;

    /// <summary>
    /// jak dlouho vydrží být ovečka v jedné náladě, po tom se zkusí nálada změnit, muže se změnit ale nemusí
    /// </summary>
    public float moodChange;
    /// <summary>
    /// random rozmezí pro změnu nálady
    /// </summary>
    public float moodChangeRange;
   


    private NavMeshAgent aiAgent;
    /// <summary>
    /// vector target ovečka je scared,normal,curious
    /// </summary>
    private Vector3 vectorTarget;
  

    private float lastMoodChange;

    /// <summary>
    /// jak daleko je od cíle
    /// </summary>
    private float remainingDistance;

    public float speed = 2.0f;

    public float interestSpeedChange = 1.1f;
    public float scaredSpeedChange = 1.5f;
    /// <summary>
    /// používá se pokud například chceme zpomali při průchodu vodou, vysokovu trávou ...
    /// </summary>
    public float defSpeedChange = 1.0f;

    public AIStates State { get { return sheepState; } set { sheepState = value; } }

   
   // private Vector3 rotationHelp;
    /// <summary>
    /// složí k tomu aby se neměli cíle moc brzy po sobě
    /// </summary>
    private float helpMoodChangeCounter = 1.0f;

	//  private Animator anim;
	/// <summary>
	/// složí na animace
	/// </summary>
	private Animator anim;

	public GameObject blood;
  

    // Use this for initialization
    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator> ();
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2 +0.05f) - moodChangeRange;
        ChangeTargetNormal();
    
    }


    void Update()
    {
        if (lastMoodChange <= 0)
        {
           
            ChangeMood();
        }
        //FenceBuild();
        //tohle pro učely testování
    /*    if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ChangeMood(AIStates.Scared);
                ChangeTargetScared(hit.point);
            }

        }*/

        
        switch (sheepState)
        {
            case AIStates.Normal: UpdateNormal(); break;
            case AIStates.Interested: UpdateInterested(); break;
            case AIStates.Scared: UpdateScared(); break;
            case AIStates.Eat: UpdateEat(); break;
        }

    }


    void UpdateNormal()
    {
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter- Time.deltaTime;
       // Vector3 targetDir =  new Vector3 (aiAgent.nextPosition.x,0,aiAgent.nextPosition.z) -  new Vector3 (transform.position.x,0,transform.position.z);
        //Vector3 forward = transform.forward;
       // Vector3 targetDir = aiAgent.nextPosition - transform.position;
        if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) < 0.5f)
        {
            //transform.rotation.eulerAngles;
            aiAgent.updatePosition = true;
        }
        else
        {
           // Vector3.Angle(cubeDir, player.forward)
          //  rotationHelp = transform.rotation.eulerAngles;
            /*Debug.Log(Vector3.RotateTowards(this.transform.forward, (aiAgent.nextPosition - transform.position), Mathf.Deg2Rad * 360.0f, 0.0f));
            Debug.Log("---");
            Debug.Log(this.transform.rotation.eulerAngles);
            Debug.Log("---------------");*/

        }

        if (aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpMoodChangeCounter <=0)
        {
           ChangeTargetNormal();
        }
        remainingDistance = aiAgent.remainingDistance;
    }

    void ChangeTargetNormal()
    {
        Vector3 newTarget;
        ///4.5f je bulharská konstanta na testování (v tomto případě jen chodit okolo)
        do {
        float r = Random.Range(2.5f, 4.5f);
        float angle = Random.Range(0, 90.0f);
        angle = transform.rotation.eulerAngles.y + angle;
        float x = r * Mathf.Cos(angle);
        float y = r * Mathf.Sin(angle);
        // Debug.Log(x);
        //  Debug.Log(y);
        newTarget = new Vector3(transform.position.x + x, 0, transform.position.z + y);
        vectorTarget = newTarget;
        }while (!aiAgent.SetDestination(newTarget));
        
        // aiAgent.speed = 0.5f;
        aiAgent.updatePosition = false;
        NewtargetAquired();
    }

    void UpdateInterested()
    {

        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;

         if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) <0.5f)
        {
           // Debug.Log("star moving - rotating complete 2");
            aiAgent.updatePosition = true;
        }
        else
        {
          //  rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.remainingDistance <0.5f||(aiAgent.updatePosition && aiAgent.remainingDistance > remainingDistance && helpMoodChangeCounter <= 0))
        {
            //dosáhl cíle tak se zastaví
            aiAgent.Stop();
           // ChangeTargetInterested();
        }
        remainingDistance = aiAgent.remainingDistance;
    }
    
    void ChangeTargetInterested()
    {
       // Debug.Log("curious target");
        Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        /// zde doplnit nějaký algoritmus na hledání  objektů
        /// tent prozatím ignoruje vzdálenosti
        List<GameObjectInfo> interestingObjects = new List<GameObjectInfo>();
        int count =0;
      
        foreach (Collider col in colliders)
        {
            GameObjectInfo interestedGameObject = col.gameObject.GetComponent<GameObjectInfo>();
            if (interestedGameObject != null && interestedGameObject.gameObject != this.gameObject)
            {
                count =count +interestedGameObject.sheepInterest;
                interestingObjects.Add(interestedGameObject);
            }
        }
        if (count == 0)
        {
            Debug.Log("no interesting objects");
            ChangeMood();
            return;
        }
      
        GameObjectInfo[] arrayShuffle = new GameObjectInfo[count];
        int position = 0;
        foreach (GameObjectInfo InterObject in interestingObjects)
        {
            for (int i = 0; i < InterObject.sheepInterest; i++)
            {
                arrayShuffle[position] = InterObject;
                position++;
            }
        }
        for (int i = 0; i < arrayShuffle.Length; i++)
        {
            var r = Random.Range(0, arrayShuffle.Length);
            var temp = arrayShuffle[r];
            arrayShuffle[r] = arrayShuffle[i];
            arrayShuffle[i] = temp;
        }
         int random = Random.Range(0,arrayShuffle.Length-1);
         vectorTarget = arrayShuffle[random].transform.position;
         count = 200;
         while (vectorTarget == arrayShuffle[random].gameObject.transform.position)
         {
             random = Random.Range(0, arrayShuffle.Length-1);
             count--;
             if (count == 0)
             {
                 ChangeMood();
             }
         }
       
        //ObjectTarget = arrayShuffle[random].gameObject;
        vectorTarget = arrayShuffle[random].transform.position;
        aiAgent.SetDestination(vectorTarget);
        aiAgent.speed = speed* defSpeedChange* interestSpeedChange;
        NewtargetAquired();
      
    }

    
    void UpdateScared()
    {

     
        lastMoodChange = lastMoodChange - Time.deltaTime*1.5f;
        if (aiAgent.remainingDistance < 0.1f)
        {
            aiAgent.Stop();
        }
    }

    void UpdateEat()
    {

        if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) < 0.5f)
        {
            // Debug.Log("star moving - rotating complete 2");
            aiAgent.updatePosition = true;
        }
        lastMoodChange = lastMoodChange - Time.deltaTime * 1.0f;
       
    }


    void ChangeTargetScared( Vector3 scaredThing)
    {
       // Debug.Log("scared target");
		Vector3 dir = this.transform.position - scaredThing;
        dir = (dir.normalized) * 80.0f;
        List<Vector3> possibleRuntargets = new List<Vector3>();
        float maxDistance =0;
        Vector3 maxVector = Vector3.forward;
        for (int i = 10; i <= 170; i = i + 5)
        {
            NavMeshHit hit;
            if (NavMesh.Raycast(transform.position,  Quaternion.Euler(0,-90 + 5 * i,0)*dir,out hit, -1))
            {
                if(!hit.hit) {
                    possibleRuntargets.Add(transform.position);
                }else if (hit.distance > 20)
                {
                    possibleRuntargets.Add(hit.position);
                }
                else if (hit.distance > maxDistance)
                {
                    maxDistance = hit.distance;
                    maxVector = hit.position;
                }
            }
        }
        if (possibleRuntargets.Count == 0)
        {
            vectorTarget = maxVector;
            aiAgent.SetDestination(maxVector);
        }
        else
        {
            int r = Random.Range(0, possibleRuntargets.Count);
            vectorTarget = possibleRuntargets[r];
            aiAgent.SetDestination(possibleRuntargets[r]);
        }
        Debug.Log("runn");
        RunningSheepAnimation();
        aiAgent.Resume();
        aiAgent.speed = defSpeedChange * speed * scaredSpeedChange;
       // helpNormalCounter = 1.0f;
       // NewtargetAquired();
    }

   
    
    private void NewtargetAquired()
    {
        aiAgent.Resume();
        aiAgent.updatePosition = false;
      //  rotationHelp = transform.rotation.eulerAngles;
        remainingDistance = aiAgent.remainingDistance;
        helpMoodChangeCounter = 1.0f;
    }

    /// <summary>
    /// zde otestovat a dodělat nějaké komplikovanější přechody?
    /// </summary>
    public void ChangeMood()
    {
        aiAgent.Resume();
        aiAgent.speed = defSpeedChange*speed;
        lastMoodChange = moodChange + Random.Range(0.1f, moodChangeRange * 2+0.05f) - moodChangeRange;
        if (sheepState == AIStates.Interested)
        {
            //Debug.Log("normal state");
            sheepState = AIStates.Normal;
            CalmSheepAnimation();
        }
        else if (sheepState == AIStates.Normal)
        {
            //Debug.Log("Interest state");
            sheepState = AIStates.Interested;
            ChangeTargetInterested();
            RunningSheepAnimation();
        }
        else if (sheepState == AIStates.Scared)
        {
           // Debug.Log("now not scared");
            sheepState = AIStates.Normal;
            CalmSheepAnimation();
        }
        else if (sheepState == AIStates.Eat)
        {
            //Debug.Log("end eating");
            sheepState = AIStates.Normal;
            CalmSheepAnimation();
        }
        //   aiAgent.Resume();
        // ChangeTargetInterested();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeMood(AIStates newState, float moodTime=0)
    {
        if (newState == AIStates.Eat)
        {
           // aiAgent.Stop();
        }
        if (moodTime == 0)
        {
            lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        }
        else
        {
            lastMoodChange = moodTime;
        }
        sheepState = newState;
    }


  

    /// <summary>
    /// nepoužívá se vubec
    /// </summary>
    /// <param name="target"></param>
    /// <param name="priority"></param>
    public void SetTarget(Transform target, int priority)
    {
        //throw new System.NotImplementedException();
    }


    public void FenceBuild()
    {
        if (aiAgent == null || aiAgent.pathEndPosition == null)
        {
            return;
        }
        NavMeshPath path = aiAgent.path;
        if (path.corners.Length > 1)
        {
           Vector3 start, end;
            RaycastHit[] hits;

            start = new Vector3(this.transform.position.x, 0.5f, this.transform.position.y);
            end = new Vector3(path.corners[0].x, 0.5f, path.corners[0].z);
            hits = Physics.RaycastAll(start, start - end, Vector3.Distance(start, end));
            foreach (RaycastHit hittedObj in hits)
            {
                if (hittedObj.collider.gameObject.tag.Equals("Tool"))
                {
                    ChangeTargetScared(hittedObj.point);
                   Debug.Log("kolize na první pozici: " + hittedObj.point + "::" + hittedObj.transform.position);
                }
            }
            // jak moc dopředu kontrolovat cestu čim víc dopředu tím víc vyděšených ovcí i zbytečně, 
            for (int i = 0; i < Mathf.Min(path.corners.Length-1,5); i++)
            {
                start=new Vector3( path.corners[i].x, 0.5f, path.corners[i].z);
                end = new Vector3(path.corners[i+1].x, 0.5f, path.corners[i+1].z);
                hits = Physics.RaycastAll(start, start - end,Vector3.Distance(start,end));
                foreach (RaycastHit hittedObj in hits)
                {
                    if (hittedObj.collider.gameObject.tag.Equals("Tool"))
                    {
                        ChangeTargetScared(hittedObj.point);
                       Debug.Log("kolize na pozici: " + hittedObj.point + "::" + hittedObj.transform.position);
                    }
                }

            }
        }
        //Debug.Log("curious target");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        // pokud najdu plot
        //vezmu si jeho pozici a zavolám
        //ChangeTargetScared(pozice)
    }

    public void SetTarget(Vector3 position, float priority)
    {
        
        if (priority >= 1.0f)
        {
            vectorTarget = position;
            aiAgent.SetDestination(position);
            NewtargetAquired();
        }
        else if (Random.value >= priority)
        {
            
            vectorTarget = position;
            aiAgent.SetDestination(position);
            NewtargetAquired();
        }
    }

    public void SetTarget(GameObject goal, float priority)
    {
        SetTarget(goal.transform.position, priority);
    }

    public void DeadOccurs(Vector3 position)
    {
        if (Vector3.Distance(this.transform.position, position) > observableArea)
        {
            ChangeMood(AIStates.Scared);
            ChangeTargetScared(position);
        }
        //volám levem manager že je mrtvo
    }

    public void SetDead()
    {
		AudioSource.PlayClipAtPoint(audio.clip,transform.position,audio.volume);
        DeathSheepAnimation();
        InstantiateBlood();
    }

	//Animation methods

  	void CalmSheepAnimation(){
		anim.SetBool ("Running", false);
		}

	void RunningSheepAnimation(){
		anim.SetBool ("Running", true);
		}

	void DeathSheepAnimation(){
		anim.SetTrigger ("Death");
		InstantiateBlood ();
		}

	IEnumerator InstantiateBlood(){
		yield return new WaitForSeconds (2);
		Instantiate (blood, transform.position, transform.rotation);
		}

}
