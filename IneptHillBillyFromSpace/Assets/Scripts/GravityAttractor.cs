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
    public  Vector3     gravCenter          = Vector3.zero;     //Center of the attractor's body
    public  float       gravForce           = -9.8f;            //The force the attractor pulls bodies towards it
    public  bool        weakenByDistance    = false;            //Does the pull weaken the farther the bodies is
    public  float       attractorMass       = 100f;             //Mass of the attractor

    void Awake()
    {
        //getComponent<RigidBody>().mass;
    }

    //Uses a Raycast to get the surface norm relative to the body's up
    Vector3 findSurface( Rigidbody attractedBody )
    {
        float distance = Vector3.Distance( this.transform.position, attractedBody.transform.position );
        Vector3 surfaceNorm = Vector3.zero;

        RaycastHit hit;
        if ( Physics.Raycast( attractedBody.transform.position, -attractedBody.transform.up, out hit, distance ) )
        {
            surfaceNorm = hit.normal;
        }

        return surfaceNorm;
    }

    //Aligns the attracted gravityBody's rotation to the surface of the attractor's body
    void orientBody( Rigidbody attractedBody, Vector3 surfaceNorm )
    {
        attractedBody.transform.localRotation = Quaternion.FromToRotation( attractedBody.transform.up, surfaceNorm ) * attractedBody.rotation;
    }

    //Pulls the body towards the center of this gameObj.
    //  Called by the GravityBody script
    public void attract( Rigidbody attractedBody )
    {
        //Vector3 pullVec = findSurface(attractedBody); //Find the surface norm relative to the body
        //orientBody(attractedBody, pullVec); //Orient the body to the surface it's on

        ////Calculate the magnitude, direction, etc. that attractor needs to pull the body in
        //float pullForce = 0f;

        ////If the force pulling the bodies does NOT get weaker
        ////      Then the force is the same
        ////Else
        ////      The force is greater the closer the body is
        //if (!weakenByDistance)
        //{
        //   //Inverse square law -> grav const * ( (mass1 * mass2) / distance^2 )
        //   pullForce = gravForce * ((attractorMass * attractedBody.mass)
        //       / Mathf.Pow(Vector3.Distance(this.transform.position + gravCenter, attractedBody.transform.position), 2));
        //}
        //else
        //{
        //   pullForce = gravForce * (attractorMass * attractedBody.mass)
        //       * Vector3.Distance(this.transform.position, attractedBody.transform.position);
        //}

        ////Get distance vector b/w body and planet's gravitational center
        //pullVec = attractedBody.transform.position - this.transform.position;

        ////Pull in that direction
        //attractedBody.AddForce(pullVec.normalized * pullForce * Time.deltaTime);

        //Get the upward direction based from the position of the body and the center of the attractor
        Vector3 gravityUp = (attractedBody.transform.position - this.transform.position).normalized;
        Vector3 bodyUp = attractedBody.transform.up; //Get the upward direction based on the body

        attractedBody.AddForce(gravityUp * gravForce); //Pull the body towards the attractor

        //Get the Rotation that we want to orient the body to
        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp) * attractedBody.transform.rotation;
        attractedBody.transform.rotation = Quaternion.Slerp(attractedBody.transform.rotation, targetRotation,
              50 * Time.deltaTime); //Rotate the body to the targetRotation over time
    }

   void OnTriggerEnter( Collider other )
   {
      GravityBody body = other.GetComponent<GravityBody>();

      //If the other obj. is a GravityBody
      //      Then set it's current attraction to THIS
      if (body != null)
      {
         other.GetComponent<GravityBody>().attractor = this;
      }
   }
}
