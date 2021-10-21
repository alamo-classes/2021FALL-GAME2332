using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public  float       walkSpeed   = 5f;     //Player's normal movement speed
    public  float       sprintSpeed = 10f;    //Player's movement speed when sprinting
    private float       moveSpeed   = 5f;     //Storage for the player's current movement speed

    public  float       jumpHeight  = 5f;     //How high the player jumps
    public  LayerMask   jumpSurfaces;         //Layer masks that tag which gameObjects can be jumped on
    private bool        isGrounded  = true;   //Boolean indicator telling if player is on the ground
    private Transform   groundChecker = null;        //Child gameObj that tells if the player is on the ground

    private Rigidbody   rigidBody;      //Reference to the player's rigidbody component
    private Vector3     inputs;         //Storage for the player's input
    private Vector3 v;


    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();  //Get the reference to the rigidbody component
        inputs = Vector3.zero;                  //Set the vector to zero
        groundChecker = transform.Find( "GroundChecker" ).GetComponent<Transform>(); //Get the reference to the groundChecker child gameObj.'s Transform comp.
    }

    // Update is called once per frame
    void Update()
    {
      //Check if the player is on the ground
      isGrounded = Physics.CheckSphere( groundChecker.position, .2f, jumpSurfaces, QueryTriggerInteraction.Ignore );

        //Set the vector to the Player's press of WASD
        Vector3 movementX = Input.GetAxis("Horizontal") * Camera.main.transform.right;
        Vector3 movementZ = Input.GetAxis("Vertical") * Camera.main.transform.forward;

        inputs = movementX + movementZ;

        v = transform.rotation.eulerAngles;                 // stops player object from leaning with Camera
        transform.rotation = Quaternion.Euler(0, v.y, 0);

        //Rotate the player to face the direction theyre facing
        if ( inputs != Vector3.zero)
            transform.forward = inputs;

      //If the player hits the jump button AND is on the ground,
      //    Launch them into the air
      if ( Input.GetButtonDown("Jump") && isGrounded )
         rigidBody.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);

      //If the player holds down the sprint button,
      //    Set their speed to sprintSpeed
      if ( Input.GetButtonDown("Sprint") )
         moveSpeed = sprintSpeed;
      
      //If the player let go of sprint button,
      //    Set their speed back to walkSpeed
      if ( Input.GetButtonUp("Sprint") )
         moveSpeed = walkSpeed;

   }

    void FixedUpdate()
    {
        //Using the inputs vector actually move the player
        rigidBody.MovePosition( rigidBody.position + transform.TransformDirection( inputs ) * moveSpeed * Time.fixedDeltaTime );
    }
}
