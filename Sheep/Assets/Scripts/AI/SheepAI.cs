using UnityEngine;
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
    /// <summary>
    /// pokud změnu nálady způsobý objekt (specální item, zájem ovečky o třeba kámen/jezero) tak se tímhle zajistí aby se při změně nálady ovečka vybrala něco jinýho
    /// nový objekt zájmu
    /// </summary>
    public GameObject moodChangeObject;


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

    private float speed = 2.0f;

    public AIStates State { get { return sheepState; } set { sheepState = value; } }

   
    private Vector3 rotationHelp;
    /// <summary>
    /// složí k tomu aby se neměli cíle moc brzy po sobě
    /// </summary>
    private float helpNormalCounter = 1.0f;
  

    // Use this for initialization
    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        ChangeMood();
      //  aiAgent.SetDestination(new Vector3(-8.0f, 0, -5.0f));
      //  remainingDistance = aiAgent.remainingDistance;

      //  aiAgent.updatePosition = false;
       // rotationHelp = transform.rotation.eulerAngles;
    }


    void Update()
    {
        if (lastMoodChange <= 0)
        {
            //  ChangeTargetNormal();
            ChangeMood();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ChangeMood(AIStates.Scared);
                ChangeTargetScared(hit.point);
            }

        }

        
        switch (sheepState)
        {
            case AIStates.Normal: UpdateNormal(); break;
            case AIStates.Interested: UpdateInterested(); break;
            case AIStates.Scared: UpdateScared(); break;
        }
    }


    void UpdateNormal()
    {
        lastMoodChange = lastMoodChange - Time.deltaTime;
        helpNormalCounter = helpNormalCounter- Time.deltaTime;
        
        if (!aiAgent.updatePosition && Mathf.Abs(rotationHelp.y - transform.rotation.eulerAngles.y) < 1.2f)
        {
            Debug.Log("star moving - rotating complete");
            aiAgent.updatePosition = true;
        }
        else
        {
            rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpNormalCounter <=0)
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
        helpNormalCounter = helpNormalCounter - Time.deltaTime;

        if (!aiAgent.updatePosition && Mathf.Abs(rotationHelp.y - transform.rotation.eulerAngles.y) < 0.6f)
        {
            Debug.Log("star moving - rotating complete 2");
            aiAgent.updatePosition = true;
        }
        else
        {
            rotationHelp = transform.rotation.eulerAngles;

        }

        if (aiAgent.remainingDistance <1.0f ||(aiAgent.updatePosition && aiAgent.remainingDistance >= remainingDistance && helpNormalCounter <= 0))
        {
           
           // ChangeTargetInterested();
        }
        remainingDistance = aiAgent.remainingDistance;
    }
    
    void ChangeTargetInterested()
    {
        Debug.Log("curious target");
        Collider[] colliders = Physics.OverlapSphere(transform.position, observableArea);
        /// zde doplnit nějaký algoritmus na hledání  objektů
        /// tent prozatím ignoruje vzdálenosti
        List<GameObjectInfo> interestingObjects = new List<GameObjectInfo>();
        int count =0;
        Debug.Log(colliders.Length);
        foreach (Collider col in colliders)
        {
            GameObjectInfo interestedGameObject = col.gameObject.GetComponent<GameObjectInfo>();
            if (interestedGameObject != null)
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
        NewtargetAquired();
      
    }

    
    void UpdateScared()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {

                ChangeTargetScared(hit.point);
            }
            
        }
        lastMoodChange = lastMoodChange - Time.deltaTime*1.5f;
       // helpNormalCounter = helpNormalCounter - Time.deltaTime * 0.9f;

      

      /*  if ( aiAgent.remainingDistance >= remainingDistance && helpNormalCounter <= 0)
        {
            ChangeMood();
        }*/
     
    }


    void ChangeTargetScared( Vector3 scaredThing)
    {
        Debug.Log("scared target");
        Vector3 dir = this.transform.position - scaredThing;
        dir = (dir.normalized) * 45.0f;
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
       // helpNormalCounter = 1.0f;
       // NewtargetAquired();
    }

   
    
    private void NewtargetAquired()
    {
        aiAgent.updatePosition = false;
        rotationHelp = transform.rotation.eulerAngles;
        remainingDistance = aiAgent.remainingDistance;
        helpNormalCounter = 1.0f;
    }


    //zatin nedokončené

    //nepoužívá se vubec
    public void SetTarget(Transform target, int priority)
    {
        throw new System.NotImplementedException();
    }


    public void FenceBuild()
    {
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
        SetTarget(goal.transform.position, priority);
    }

    public void DeadOccurs(Vector3 position)
    {
        throw new System.NotImplementedException();
        //volám levem manager že je mrtvo
    }

    public bool CheckDead()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// zde otestovat a dodělat nějaké komplikovanější přechody?
    /// </summary>
    public void ChangeMood()
    {
       // aiAgent.Resume();
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        if (sheepState == AIStates.Interested)
        {
            Debug.Log("normal state");
            sheepState = AIStates.Normal;
        }
        else if (sheepState == AIStates.Normal)
        {
            Debug.Log("Interest state");
            sheepState = AIStates.Interested;
            ChangeTargetInterested();

        }
        else if (sheepState == AIStates.Scared)
        {
            Debug.Log("now not scared");
            sheepState = AIStates.Normal;
        }
     //   aiAgent.Resume();
       // ChangeTargetInterested();
    }

    public void ChangeMood( AIStates newState)
    {
        lastMoodChange = moodChange + Random.Range(0, moodChangeRange * 2) - moodChangeRange;
        sheepState =newState;
    }


}
