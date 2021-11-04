using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    public Transform reference;
    public float upDistance = 0.5f;
    public float backDistance = 0.75f;
    public float trackingSpeed = 0.5f;
    public float rotationSpeed = 1.0f;

    private Vector3 v3To;
    private Quaternion qTo;

    void LateUpdate()
    {
        Vector3 v3Up = (target.position - reference.position).normalized;
        v3To = target.position - target.forward * backDistance + v3Up * upDistance;
        transform.position = Vector3.Lerp(transform.position, v3To, trackingSpeed * Time.deltaTime);

        qTo = Quaternion.LookRotation(target.position - transform.position, v3Up);
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, rotationSpeed * Time.deltaTime);
    }
}