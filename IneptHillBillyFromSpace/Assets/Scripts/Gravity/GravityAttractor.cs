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

    //============================================================================================================
    // Finds the surfaceNorm relative to the attractedBody's position and the attractor's position
    //   and returns it as a Vector3
    //
    //   param attractedBody - RigidBody component of the gameObj. that we want to find the norm from
    //
    //   return surfaceNorm - Vector3 the surfaceNorm relative to attractedBody
    //
    public Vector3 findSurfaceNorm( Rigidbody attractedBody )
    {
        //Get the upward direction based from the position of the body and the center of the attractor
        Vector3 surfaceNorm = ( attractedBody.transform.position - this.transform.position ).normalized;

        return surfaceNorm;
    }

    //============================================================================================================
    // Aligns the attracted gravityBody's rotation to the surface norm of the attractor's body
    // 
    //   param attractedBody - RigidBody component of the gameObj. that we want to orient to the attractor
    //   param surfaceNorm - Vector3 the surface norm relative to the attractedBody's position on the attractor
    //
    public void orientBody( Rigidbody attractedBody, Vector3 surfaceNorm )
    {
        Vector3 bodyUp = attractedBody.transform.up; //Get the upward direction based on the body
        
        //Get the Rotation that we want to orient the body to
        Quaternion targetRotation = Quaternion.FromToRotation( bodyUp, surfaceNorm ) * attractedBody.transform.rotation;
        attractedBody.transform.rotation = Quaternion.Slerp( attractedBody.transform.rotation, targetRotation,
              50 * Time.deltaTime ); //Rotate the body to the targetRotation over time
    }

    //=============================================================================================================
    // Pulls the attractedBody towards the center of this gameObj.
    //   Called by the GravityBody script
    //
    //   param attractedBody - Rigidbody component of gameObj. that we want to pull towards the center of attractor
    //
    public void attract( Rigidbody attractedBody )
    {
        //Get the upward direction based from the position of the body and the center of the attractor
        Vector3 surfaceNorm = findSurfaceNorm(attractedBody);

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
