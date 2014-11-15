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

    private float speed = 2.7f;

    public AIStates State { get { return wolfState; } set { wolfState = value; } }

  //  private Vector3 rotationHelp;
    /// <summary>
    /// složí k tomu aby se neměli cíle moc brzy po sobě
    /// </summary>
    private float helpMoodChangeCounter = 1.0f;


    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
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
            case AIStates.Full: UpdateFull(); break;
        }
    }


    void UpdateNormal()
    {
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpMoodChangeCounter = helpMoodChangeCounter - Time.deltaTime;

        if (!aiAgent.updatePosition && Vector3.Angle(new Vector3(aiAgent.nextPosition.x, 0, aiAgent.nextPosition.z) - new Vector3(transform.position.x, 0, transform.position.z), transform.forward) < 0.5f)
        {
            Debug.Log("star moving - rotating complete");
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
            Debug.Log("star moving - rotating complete 2");
            aiAgent.updatePosition = true;
        }
        else
        {
           // rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.remainingDistance < 1.0f || (aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpMoodChangeCounter <= 0))
        {

            // ChangeTargetInterested();
        }
        remainingDistance = aiAgent.remainingDistance;
    }

    void ChangeTargetTracking()
    {
        Debug.Log("curious target");
        Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        /// zde doplnit nějaký algoritmus na hledání  objektů
        /// tent prozatím ignoruje vzdálenosti
        List<GameObjectInfo> interestingObjects = new List<GameObjectInfo>();
        int count = 0;
        Debug.Log(colliders.Length);
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
        Debug.Log(count);
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

        aiAgent.speed = speed * 0.75f;
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
            Debug.Log("hunting lost");
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
        aiAgent.speed = speed * 1.5f;
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
        Debug.Log("change mood??");
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        if (wolfState != AIStates.Full || wolfState != AIStates.Hunting)
        {
            GameObject target = FindFood();
            if (target != null)
            {
                wolfState = AIStates.Hunting;

                SetHuntingTarget(target);
                return;
            }
            
        }

        Debug.Log("change mood");
        if (wolfState == AIStates.Tracking)
        {
            Debug.Log("normal state");
            wolfState = AIStates.Normal;
        }
        else if (wolfState == AIStates.Normal)
        {
            Debug.Log("tracking state");
            wolfState = AIStates.Tracking;
            ChangeTargetTracking();

        }
        else if (wolfState == AIStates.Hunting)
        {
            Debug.Log("now not hunt");
            wolfState = AIStates.Normal;
        }
        else if (wolfState == AIStates.Full)
        {
            Debug.Log("again hungry");
            wolfState = AIStates.Normal;

        }
    }

    public void ChangeMood(AIStates newState)
    {
        wolfState = newState;
    }


    public void Spawn()
    {
        throw new System.NotImplementedException();
    }

    public void SetTarget(Transform target, int priority)
    {
        throw new System.NotImplementedException();
    }


    public void FenceBuild()
    {
        if (wolfState == AIStates.Hunting || wolfState == AIStates.Tracking)
        {
            for (int i = 0; i < aiAgent.path.corners.Length - 1; i++)
            {

                RaycastHit[] obstacles = Physics.RaycastAll(aiAgent.path.corners[i], aiAgent.path.corners[i + 1]);
                foreach (RaycastHit hit in obstacles)
                {
                    // if hit plot tak změna stavu hunting -tracking, tracking -> hledání nových cílů
                }
            }
        }
        Debug.Log("curious target");
        Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        // pokud najdu plot
        //vezmu si jeho pozici a zavolám
        //ChangeTargetScared(pozice)
    }

    public void SetTarget(Vector3 position, float priority)
    {
        throw new System.NotImplementedException();
    }

    public void SetTarget(GameObject goal, float priority)
    {
        throw new System.NotImplementedException();
    }

    public void DeadOccurs(Vector3 position)
    {
        if (aiAgent.destination == position)
        {
            ChangeMood();
        }
    }

    public void SetDead()
    {
        throw new System.NotImplementedException();
    }

    public GameObject FindFood()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7.0f);
        Debug.Log(colliders.Length + "finded maybe food");
        foreach (Collider col in colliders)
        {
            SheepAI sheep = col.GetComponent<SheepAI>();
            if (sheep != null)
            {
                Debug.Log("nalezena ovečka");
                
                return sheep.gameObject;
            }
        }
        return null;
    }

    void OnTriggerEnter(Collider other)
    {
        //hlídat časem asi líp ale co se dá dělat
        SheepAI sheep = other.gameObject.GetComponent<SheepAI>();
        if (sheep != null)
        {
            Debug.Log("snězena ovečka");
            ChangeMood(AIStates.Full);
            sheep.SetDead();
            Destroy(sheep.gameObject);
        }
        else
        {
            Debug.Log("kolize ne ovečka");
        }
    }

    //pak sem ještě patří když v okolí bude ovečka/maso? tak jít do hunting
}
