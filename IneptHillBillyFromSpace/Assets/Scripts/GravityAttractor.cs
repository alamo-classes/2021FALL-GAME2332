using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//GravityAttractor is a large bodies object that pulls GravityBody objects towards its center
//
//  Place this script on the child of this the planet (the object that the bodies will move on) w/ a trigger and the parent
//      should have a collider and ( optional right now ) rigidbody
//
public class GravityAttractor : MonoBehaviour
{
    public  float       gravForce           = -9.8f;            //The force the attractor pulls bodies towards it

    void Awake()
    {
        //attractorMass = getComponent<RigidBody>().mass;
    }

    //Aligns the attracted gravityBody's rotation to the surface norm of the attractor's body
    public void orientBody( Rigidbody attractedBody, Vector3 surfaceNorm )
    {
         Vector3 bodyUp = attractedBody.transform.up; //Get the upward direction based on the body

         //Get the Rotation that we want to orient the body to
         Quaternion targetRotation = Quaternion.FromToRotation( bodyUp, surfaceNorm ) * attractedBody.transform.rotation;
         attractedBody.transform.rotation = Quaternion.Slerp( attractedBody.transform.rotation, targetRotation,
               50 * Time.deltaTime ); //Rotate the body to the targetRotation over time
   }

    //Pulls the body towards the center of this gameObj.
    //  Called by the GravityBody script
    public void attract( Rigidbody attractedBody )
    {
        //Get the upward direction based from the position of the body and the center of the attractor
        Vector3 surfaceNorm = (attractedBody.transform.position - this.transform.position).normalized;

        attractedBody.AddForce( surfaceNorm * gravForce ); //Pull the body towards the attractor

        orientBody( attractedBody, surfaceNorm ); //Orient the body upwards from the surface norm
    }

   void OnTriggerEnter( Collider other )
   {
      GravityBody body = other.GetComponent<GravityBody>();

      //If the other obj. is a GravityBody
      //      Then set it's current attraction to THIS
      if ( body != null )
      {
         other.GetComponent<GravityBody>().attractor = this;
      }
   }
}
