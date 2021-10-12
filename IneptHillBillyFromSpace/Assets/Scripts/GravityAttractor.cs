using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public  Vector3     gravCenter          = Vector3.zero;     //Center of the attractor's body
    public  float       gravConstaint       = -9.8f;            //The force the attractor pulls bodies towards it
    public  bool        weakenByDistance    = false;            //Does the pull weaken the farther the bodies is
    public  float       attractorMass       = 100f;             //Mass of the attractor

    void Awake()
    {
        //getComponent<RigidBody>().mass;
    }

    Vector3 findSurface( RigidBody attractedBody )
    {
        float distance = Vector3.Distance( this.transform.position, attractedBody.transform.position );
        Vector3 surfaceNorm = Vector3.zero;

        Raycast hit;
        if ( Physics.Raycast( attractedBody.transform.position, attractedBody.transform.up, out hit, distance ) )
        {
            surfaceNorm = hit.normal;
        }

        return surfaceNorm;
    }

    //Aligns the attracted gravityBody's rotation to the surface of the attractor's body
    void orientBody( RigidBody attractedBody, Vector3 surfaceNorm )
    {
        attractedBody.transform.localRotation = Quaternion.FromToRotation( attractedBody.transform.up, surfaceNorm ) * attractedBody.rotation;
    }
}
