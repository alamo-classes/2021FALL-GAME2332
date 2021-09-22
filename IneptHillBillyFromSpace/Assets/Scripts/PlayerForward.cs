using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerForward : MonoBehaviour
{
    public new Transform camera;
    Transform player;

    private void Start()
    {
        player = GetComponent<Transform>();
    }


    void Update()
    {
        if (Input.GetKey(KeyCode.W))
            player.forward = camera.forward;



    }
}
