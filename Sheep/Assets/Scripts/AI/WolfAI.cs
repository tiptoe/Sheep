using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WolfAI : MonoBehaviour, IAnimalAI
{

    public AIStates wolfState = AIStates.Normal;

    /// <summary>
    /// dává info když objekt umře, .. nebo kdyby se potřeboval informovat o něčem
    /// </summary>
    public LevelController controler;

    /// <summary>
    /// jak daleko se vlk dívá při situacích jako, někdo umřel,byl postaven most, atd, jde nastavit pro každou klidně jinak
    /// </summary>
    public float observableArea = 15.0f;

    /// <summary>
    /// jak dlouho vydrží být vlk v jedné náladě, po tom se zkusí nálada změnit, muže se změnit ale nemusí
    /// </summary>
    public float moodChange;
    /// <summary>
    /// random rozmezí pro změnu nálady
    /// </summary>
    public float moodChangeRange;



    private NavMeshAgent aiAgent;

    /// <summary>
    /// ne stav hunting - jde za pozicí
    /// </summary>
    private Vector3 vectorTarget;

    /// <summary>
    /// hunting jde po gameobjectu
    /// </summary>
    public GameObject gameObjectTarget;


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

    public AIStates State { get { return wolfState; } set { wolfState = value; } }

  //  private Vector3 rotationHelp;
    /// <summary>
    /// složí k tomu aby se neměli cíle moc brzy po sobě
    /// </summary>
    private float helpMoodChangeCounter = 1.0f;

	//  private Animator anim;
	/// <summary>
	/// složí na animace
	/// </summary>
	private Animator anim;

    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
		anim = GetComponentInChildren<Animator> ();
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        ChangeMood();
    }


    void Update()
    {
       
        if (lastMoodChange <= 0)
        {
            //  ChangeTargetNormal();
            ChangeMood();
        }



        switch (wolfState)
        {
            case AIStates.Normal: UpdateNormal(); break;
            case AIStates.Tracking: UpdateTracking(); break;
            case AIStates.Hunting: UpdateHunting(); break;
            case AIStates.Eat: UpdateFull(); break;
        }
    }


    void UpdateNormal()
    {
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;

        if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) < 0.5f)
        {
           // Debug.Log("star moving - rotating complete");
            aiAgent.updatePosition = true;
        }
        else
        {
            //rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpMoodChangeCounter <= 0)
        {
            ChangeTargetNormal();
        }
        remainingDistance = aiAgent.remainingDistance;
    }

    void ChangeTargetNormal()
    {
        Vector3 newTarget;
        ///4.5f je bulharská konstanta na testování (v tomto případě jen chodit okolo)
        do
        {
            float r = Random.Range(2.5f, 4.5f);
            float angle = Random.Range(0, 90.0f);
            angle = transform.rotation.eulerAngles.y + angle;
            float x = r * Mathf.Cos(angle);
            float y = r * Mathf.Sin(angle);
            // Debug.Log(x);
            //  Debug.Log(y);
            newTarget = new Vector3(transform.position.x + x, 0, transform.position.z + y);
            vectorTarget = newTarget;
        } while (!aiAgent.SetDestination(newTarget));

        // aiAgent.speed = 0.5f;
        aiAgent.updatePosition = false;
        NewtargetAquired();
    }

    void UpdateTracking()
    {


        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;

        if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) < 0.5f)
        {
            //Debug.Log("star moving - rotating complete 2");
            aiAgent.updatePosition = true;
        }
        else
        {
           // rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.remainingDistance < 1.0f || (aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpMoodChangeCounter <= 0))
        {
            Debug.Log("target is lost");
            CalmWolfAnimation();
            ChangeTargetTracking();
        }
        remainingDistance = aiAgent.remainingDistance;
    }

    void ChangeTargetTracking()
    {
        //Debug.Log("curious target");
        Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        /// zde kdžtak vylepšit nějaký algoritmus na hledání  objektů
        /// tent prozatím ignoruje vzdálenosti
        List<GameObjectInfo> interestingObjects = new List<GameObjectInfo>();
        int count = 0;
         foreach (Collider col in colliders)
        {
            GameObjectInfo interestedGameObject = col.gameObject.GetComponent<GameObjectInfo>();
            if (interestedGameObject != null && interestedGameObject.gameObject != this.gameObject)
            {
                count = count + interestedGameObject.sheepInterest;
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
        int random = Random.Range(0, arrayShuffle.Length - 1);
        vectorTarget = arrayShuffle[random].transform.position;
        count = 200;
        while (vectorTarget == arrayShuffle[random].gameObject.transform.position)
        {
            random = Random.Range(0, arrayShuffle.Length - 1);
            count--;
            if (count == 0)
            {
                ChangeMood();
            }
        }

        aiAgent.speed = defSpeedChange*speed * 0.75f;
        //ObjectTarget = arrayShuffle[random].gameObject;
        vectorTarget = arrayShuffle[random].transform.position;
        aiAgent.SetDestination(vectorTarget);
        NewtargetAquired();
    }

    void UpdateHunting()
    {
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;
        if (gameObjectTarget == null)
        {
            ChangeMood();
           // Debug.Log("hunting lost");
        }
        else
        {
           // Vector3.MoveTowards(transform.position, 
            aiAgent.destination=gameObjectTarget.transform.position;
        }
    }


    void SetHuntingTarget(GameObject target)
    {
       
        Debug.Log("finded ovečka" + target.transform.position);
        aiAgent.Resume();
        aiAgent.speed = defSpeedChange * speed * 1.5f;
        vectorTarget = target.transform.position;
        gameObjectTarget = target;
        aiAgent.SetDestination(gameObjectTarget.transform.position);
    }


    void UpdateFull()
    {
        aiAgent.SetDestination(transform.position);
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;
        // dělá animaci?
    }


    private void NewtargetAquired()
    {
        aiAgent.updatePosition = false;
       // rotationHelp = transform.rotation.eulerAngles;
        remainingDistance = aiAgent.remainingDistance;
        helpMoodChangeCounter = 1.0f;
    }


    public void ChangeMood()
    {
        gameObjectTarget = null;
        CalmWolfAnimation();
        aiAgent.Resume();
        aiAgent.speed = defSpeedChange * speed;
       // Debug.Log("change mood??");
        lastMoodChange = moodChange + Random.Range(0.0f, moodChangeRange * 2+0.05f) - moodChangeRange;
        
        //tohle kontroluje zda je nějaké jídlo extrémněnlízko
        GameObject nearFood = FindFood(true);
        if (nearFood != null)
        {
            Debug.Log("prioritní jidlo");
            wolfState = AIStates.Hunting;
            RunningWolfAnimation();
            SetHuntingTarget(nearFood);

            return;
        }
        if (wolfState != AIStates.Eat && wolfState != AIStates.Hunting)
        {
            GameObject target = FindFood();
            if (target != null)
            {
                wolfState = AIStates.Hunting;
                RunningWolfAnimation();
                SetHuntingTarget(target);
                return;
            }
            
        }

       // Debug.Log("change mood");
        if (wolfState == AIStates.Tracking)
        {
          //  Debug.Log("normal state");
            CalmWolfAnimation();
            wolfState = AIStates.Normal;
        }
        else if (wolfState == AIStates.Normal)
        {
          //  Debug.Log("tracking state");
            wolfState = AIStates.Tracking;
            RunningWolfAnimation();
            ChangeTargetTracking();

        }
        else if (wolfState == AIStates.Hunting)
        {
           // Debug.Log("now not hunt");
            CalmWolfAnimation();
            wolfState = AIStates.Normal;
        }
        else if (wolfState == AIStates.Eat)
        {
            //Debug.Log("again hungry");
            CalmWolfAnimation();
            wolfState = AIStates.Normal;

        }
    }

    public void ChangeMood(AIStates newState, float moodTime = 0)
    {
        if (newState == AIStates.Eat)
        {
            EatingWolfAnimation();
            //  aiAgent.Stop();
        }
        
        wolfState = newState;
    }


    public void Spawn()
    {
       //
    }

    /// <summary>
    /// nepoužívá se vubec
    /// </summary>
    /// <param name="target"></param>
    /// <param name="priority"></param>
    public void SetTarget(Transform target, int priority)
    {
        //
    }

    public void FenceBuild()
    {
        if (aiAgent == null || aiAgent.pathEndPosition == null)
        {
            return;
        }
        if (wolfState != AIStates.Hunting)
        {
            return;
        }
        float distance = 0.0f;
        Vector3 targetToCalc = aiAgent.pathEndPosition;
       
        NavMeshPath newPath = new NavMeshPath();
        if (aiAgent.CalculatePath(targetToCalc, newPath))
        {
            for (int i = 0; i < newPath.corners.Length - 1; i++)
            {
                distance += Vector3.Distance(newPath.corners[i], newPath.corners[i + 1]);
            }
            distance += Vector3.Distance(newPath.corners[0], this.transform.position);
            if (distance >= aiAgent.remainingDistance * 2.0f && distance>5.0f)
            {
                ChangeMood();
               // Debug.Log("překážka změna cíle");
            }
        }
        
      
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
        if (aiAgent == null || aiAgent.destination == null)
        {
            return;
        }
        if (aiAgent.destination == position)
        {
            ChangeMood();
        }
    }

    public void SetDead()
    {
       //doplnit 
    }

    public GameObject FindFood(bool near= false)
    {
        Collider[] colliders;
        if (near)
        {
           colliders = Physics.OverlapSphere(transform.position, observableArea / 4.0f);
        }
        else
        {
            colliders = Physics.OverlapSphere(transform.position, observableArea / 2.0f);
        }
        float distanceFood = float.MaxValue;
        //SheepAI returnSheepAI =null;
        GameObject food = null;
        GameObject returnFood = null;
        //Debug.Log(colliders.Length + "finded maybe food");
        foreach (Collider col in colliders)
        {
             SheepAI sheep = col.GetComponent<SheepAI>();
             Meat meat = col.GetComponent<Meat>();
             if (sheep != null)
             {
                 food = sheep.gameObject;
             }
             else if (meat != null)
             {
                 food = meat.gameObject;
             }
             if (food != null)
            {
                if (near)
                {
                    if (Physics.Raycast(this.transform.position, this.transform.position - food.transform.position))
                    {
                        continue;
                    }
                }
                float distance = Vector3.Distance(food.transform.position, this.transform.position);
                NavMeshPath path = new NavMeshPath();
                float pathDistance = 0;
                aiAgent.CalculatePath(food.transform.position, path);
                for (int i = 0; i < path.corners.Length - 1; i++)
                {
                    pathDistance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                }
                if ((distance + pathDistance) / 2.0f < distanceFood)
                {
                    distanceFood = (distance + pathDistance) / 2.0f;
                    //returnSheepAI = sheep;
                    returnFood = food;
                }
                else
                {
                    Debug.Log("too farr food");
                }
                   
            }
        }
        if (returnFood != null)
        {
            return returnFood.gameObject;
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        //hlídat časem asi líp ale co se dá dělat
        SheepAI sheep = other.gameObject.GetComponent<SheepAI>();
        if (sheep != null)
        {
            LevelController controller = FindObjectOfType<LevelController>();
            if (controller != null)
            {

                EatingWolfAnimation();
                foreach (GameObject sheepGO in controller.Sheeps)
                {
                    if (sheepGO == null) { continue; }
                    SheepAI sheepAI = sheepGO.GetComponent<SheepAI>();
                    if (sheepAI != null)
                    {
                        sheepAI.DeadOccurs(other.gameObject.transform.position);
                    }
                }

                foreach (GameObject wolfGO in controller.Wolves)
                {
                    if (wolfGO == null) { continue; }
                    WolfAI wolfAI = wolfGO.GetComponent<WolfAI>();
                    if (wolfAI != null)
                    {
                        wolfAI.DeadOccurs(other.gameObject.transform.position);
                    }
                }

                Debug.Log("snězena ovečka");
                ChangeMood(AIStates.Eat);
                sheep.SetDead();
                controler.AnimalDied(Animals.Sheep);
                Destroy(sheep.gameObject);
                gameObjectTarget = null;
            }
        }
        else
        {
            //Debug.Log("kolize ne ovečka");
        }
    }

	
	//Animation methods
	
	void CalmWolfAnimation(){
		anim.SetBool ("Waiting", true);
	}
	
	void RunningWolfAnimation(){
		anim.SetBool ("Waiting", false);
	}
	
	void EatingWolfAnimation(){
		anim.SetTrigger("Eating");
	}

   
    //pak sem ještě patří když v okolí bude ovečka/maso? tak jít do hunting
}
