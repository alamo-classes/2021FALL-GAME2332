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

    private     Transform       player;         //Reference to the player
    private     NavMeshAgent    agent;          //Reference to this obj's NavMesh Agent component
    private     float           agentSpeed;     //NavMesh movement speed. Maximum movement speed of enemy
    private     Animator        animator;       //Reference to this obj's Animator component

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag( "Player" ).transform;
        agent  = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //animator = GetComponent<Animator>();

        if ( agent != null )
        {
            agentSpeed = agent.speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
