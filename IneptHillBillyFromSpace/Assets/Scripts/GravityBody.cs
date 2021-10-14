using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBody : MonoBehaviour
{
    public  GravityAttractor    attractor;      //The obj that THIS will be attracted to gravitationally    
    public  Rigidbody           rigidBody;      //The Rigidbody component of THIS game obj

    void Awake()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if ( attractor != null && rigidBody != null )
            attractor.attract( rigidBody );
    }
}
