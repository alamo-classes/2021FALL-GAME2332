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
               agent.destination = idleWalk();
            }
        }



    }


    Vector3 idleWalk ()
    {
        time = 0;

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;

        
    }


}
