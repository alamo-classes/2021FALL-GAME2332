using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour
{
    public new Transform camera;
    Transform player;

    Vector3 v;

    private void Start()
    {
        player = GetComponent<Transform>();
    }


    void Update()
    {

        if (Input.GetKey(KeyCode.W))
        {
            player.forward = camera.forward;
        }

        v = player.rotation.eulerAngles;

        player.rotation = Quaternion.Euler(0, v.y, 0);

    }
}
