using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{

    public int walkRadius = 10;
    public int idleTimer = 10;

    NavMeshAgent agent;

    public bool Idle = true;
    public float time = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

    }
    
    void Update()
    {
        time += Time.deltaTime;


        if (Idle)
        {
            while(time >= idleTimer)
            {
               agent.destination = IdleWalk();
            }
        }

     

    }


    Vector3 IdleWalk ()
    {
        time = 0;

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;

        
    }


    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            Idle = false;
            agent.destination = col.transform.position;
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            time = 0;
            Idle = true;
        }
    }


}
