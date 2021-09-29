using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public  float       moveSpeed = 5f;     //Player's movement speed

    private Rigidbody   rigidBody;      //Reference to the player's rigidbody component
    private Vector3     inputs;         //Storage for the player's input


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();  //Get the reference to the rigidbody component
        inputs = Vector3.zero;                  //Set the vector to zero
    }

    // Update is called once per frame
    void Update()
    {
        //Set the vector to the Player's press of WASD
        inputs.x = Input.GetAxis( "Horizontal" );
        inputs.z = Input.GetAxis( "Vertical" );

        //Rotate the player to face the direction theyre facing
        if ( inputs != Vector3.zero)
         transform.forward = inputs;

   }

    void FixedUpdate()
    {
        //Using the inputs vector actually move the player
        rigidBody.MovePosition( rigidBody.position + inputs * moveSpeed * Time.fixedDeltaTime );
    }
}
