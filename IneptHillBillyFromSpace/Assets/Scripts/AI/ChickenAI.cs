using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenAI : MonoBehaviour
{
    public      string          searchTag;          //Tag that the chicken will be looking for
    public      GameObject[]    targets;            //Array of the targets with the searchTag tag
    public      float           playerRange;        //How far the chicken can be from the player
    public      float           collectibleRange;   //How close the chicken has to be to a search target before moving towards it
    private     GameObject      closestCollect;     //The closest gameObject from targets to the chicken
    private     float           closestCollectDist; //The distance b/w chicken and closestCollect

    private     Transform       player;         //Reference to the player
    private     NavMeshAgent    agent;          //Reference to this obj's NavMesh Agent component
    private     float           agentSpeed;     //NavMesh movement speed. Maximum movement speed of enemy
    private     Animator        animator;       //Reference to this obj's Animator component

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag( "Player" ).transform;
        agent  = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();

        if ( searchTag != null )
        {
            targets = GameObject.FindGameObjectsWithTag( searchTag );
        }

        if ( agent != null )
        {
            agentSpeed = agent.speed;
            agent.Warp(transform.position);
        }
    }

    void Update()
    {
        if ( agent.speed == 0 )
        {
            playIdleAnimation();
        }
        else
        {
            playWalkingAnimation();
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( agent.isOnNavMesh )
        {
            agent.enabled = true;
            //If there are targets AND the chicken is in range of a collectible
            //  chicken moves towards the closest collectible
            //Else
            //  follow player
            if ( targets != null && inCollectibleRange() )
            {
                //Debug.Log("Chicken: Target in Range");

                //If the chicken is farther away from it's agent stopping dist. from the closest collectible
                //  move towards the collectible
                //Else
                //  stop moving
                if ( Vector3.Distance( transform.position, closestCollect.transform.position ) > agent.stoppingDistance )
                {
                    agent.destination = closestCollect.transform.position;
                    agent.speed = agentSpeed;
                    //Debug.Log("Chicken going to Target");
                }
                else
                {
                    agent.speed = 0;
                    //Debug.Log("Chicken stopping at Target");
                }
            }
            else
            {
                //Debug.Log("Chicken: Target not in Range");
                //If the chicken is about to be out of playerRange
                //  chicken moves towards the player
                //Else If the chicken is close enough to the player 
                //  stop moving
                if ( Vector3.Distance( transform.position, player.position ) > playerRange )
                {
                    agent.destination = player.position;
                    agent.speed = agentSpeed;
                    //Debug.Log("Chicken going to Player");
                }
                else if ( Vector3.Distance( transform.position, player.position ) < agent.stoppingDistance )
                {
                    agent.speed = 0;
                    //Debug.Log("Chicken stopping at Player");
                }
            }
        }
        else
        {
            agent.enabled = false;
        }
    }

    //=============================================================================================================================
    // inCollectibleRange - finds the closest collectible in targets list and returns a boolean of whether the collectible 
    //      is within collectible range
    //
    private bool inCollectibleRange()
    {
        getClosestCollectible();

        return closestCollectDist <= collectibleRange;
    }

    //===============================================================================================================================
    // getClosestCollectible - sets the closestCollectible variable to the collectible closest to the chicken and returns it
    //
    private GameObject getClosestCollectible()
    {
        GameObject closestObj = null;

        if ( targets != null )
        {
            closestObj = targets[0];
            float closestObjDist = Vector3.Distance(transform.position, closestObj.transform.position);

            foreach ( GameObject currentObj in targets )
            {
                if ( currentObj != null )
                {
                    if ( !(currentObj.GetComponent<Collectable>().isPickedUp) )
                    {
                       float currentObjDist = Vector3.Distance(transform.position, currentObj.transform.position);

                       //If the closest object distance is greater than the current obj's distance
                       //  set the closest obj and its distance to currentObj
                       if ( closestObjDist > currentObjDist )
                       {
                          closestObj = currentObj;
                          closestObjDist = currentObjDist;
                       }
                    }
                }
            }

            closestCollect = closestObj;
            closestCollectDist = closestObjDist;
        }

        return closestObj;
    }

    private void playIdleAnimation()
    {
        if ( animator != null )
        {
            animator.SetBool( "isWalking", false );     //Turns off walking animation

            string animationName;
            int animationIndex = Random.Range( 0, 9 );  //Random number b/w 0 and the number of idle animations + eating animation
                                                        //  range is inclusives on both ends
            if ( animationIndex < 9 )
            {
                animationName = "idle_"+ animationIndex;
            }
            else
            {
                animationName = "eat";
            }

            animator.SetTrigger( animationName );
            //Debug.Log( "Playing "+ animationName + " Idle animation" );
        }
    }

    private void playWalkingAnimation()
    {
        if ( animator != null )
        {
            animator.SetBool( "isWalking", true );
            //Debug.Log( "Playing Walking animation" );
        }
    }
}
