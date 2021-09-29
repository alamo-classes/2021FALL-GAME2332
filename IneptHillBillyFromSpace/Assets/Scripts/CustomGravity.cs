using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGravity : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static vector3 GetGravity(Vector3 position) {
        return Physics.gravity;
    }

    public static Vector3 GetUpAxis(Vector3 position) {
        return -Physics.gravity.normalized;
    }

    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis) {
        upAxis = -Physics.gravity.normalized;
        return Physics.gravity;
    }
}
