using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{

    public int walkRadius = 10; // radius that they can walk out to when idle
    public int idleTimer = 10; // time it takes them to walk to a new position when idle

    NavMeshAgent agent; // instantiate the navmesh agent

    public bool Idle = true; // bool for if the enemy is idle
    public float time = 0f; // used to count time

    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); // assign the nav mesh
        animator = GetComponent<Animator>();

        time = Random.Range(0.0f, 1.2f); // choose random starting time so that groups of enemies dont all move in unison

    }
    
    void Update()
    {
        time += Time.deltaTime; // increases time by seconds


        if (Idle) // if allowed to idle
        {
            while(time >= idleTimer) // while the time is less than their idle time
            {
               agent.destination = IdleWalk(); // set destination of nav mesh agent to random position
            }
        }

        //Play the animations depending on movespeed
        if ( animator != null )
        {
            if ( agent.speed == 0 )
            {
                animator.SetBool( "IsIdle", true );
                animator.SetBool( "IsMoving", false );
            }
            else
            {
                animator.SetBool( "IsIdle", false );
                animator.SetBool( "IsMoving", true );
            }
        }
     

    }


    Vector3 IdleWalk () // chooses a random position to walk to
    {
        time = 0;

        Vector3 randomDirection = Random.insideUnitSphere * walkRadius; // sets a random point in a circle with the radius of walkradius

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);

        return hit.position;

        
    }


    private void OnTriggerStay(Collider col) // when object with tag "Player" is in a trigger collider stop idling and move towars it
    {
        if(col.gameObject.tag == "Player")
        {
            Idle = false;
            agent.destination = col.transform.position;
        }
    }

    private void OnTriggerExit(Collider col) // when object with player tag has exited wait and idle
    {
        if (col.gameObject.tag == "Player")
        {
            time = Random.Range(0.0f, 1.2f); // make time random again so if player evades group of ai they wont be in unison when idle
            Idle = true;
        }
    }


}
